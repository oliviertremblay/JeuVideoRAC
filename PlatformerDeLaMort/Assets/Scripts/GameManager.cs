using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public enum GameState { PlayerPassed, PlayerLostLife, GameOver, PlayerWon, Paused, Playing }

/// <summary>
/// La classe principale pour g�rer l'�tat d'une partie.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Permet de rapidement v�rifier si le joueur a �chou� ou non le tableau lorsque le temps maximal est atteint
    /// </summary>
    public bool TimesUp
    {
        get
        {
            if (_gameTime >= _maxTime)
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Permet de savoir si le joueur a pass� le tableau ou pas
    /// </summary>
    public bool LevelPassed = false;


    /// <summary>
    /// Permet de connaitre l'�tat du jeu
    /// </summary>
    public GameState GameState = GameState.Playing;

    /// <summary>
    /// Retourne le temps restant pour compl�ter le tableau
    /// </summary>
    public double RemainingTime { get => Math.Round(((double)_maxTime - (double)_gameTime), 0); }

    private static double _score;

    private readonly float _maxTime = 60.0f;
    private float _gameTime = 0.0f;
    private static int _lives = 3; //Static permet de ne pas perdre trace des vies lorsqu'on recharge la sc�ne


    [SerializeField] PlayerController _playerController;
    [SerializeField] PlayerDisplayText _playerDisplayText;
    [SerializeField] TimeDisplayControl _timeDisplayControl;
    [SerializeField] GameObject _quitButton;

    /// <summary>
    /// Permet de quitter l'application
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    /// <summary>
    /// Permet de 'afficher ou non le boutton pour quitter l'application
    /// <param name="showButton">Mettre true si on veut afficher le boutton, mettre false si on veut cacher le boutton</param>
    /// </summary>
    public void ShowQuitButton(bool showButton)
    {
        _quitButton.SetActive(showButton);
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        GameState = GameState.Playing;

        _playerDisplayText.ShowLives(_lives);
        ShowQuitButton(false);
    }

    private void Update()
    {
        _timeDisplayControl.DisplayTime(RemainingTime);

        IncrementTime();
        CheckIfLevelIsPassed();
    }

    private void IncrementTime()
    {
        if(GameState == GameState.Playing)
        {
            _gameTime += Time.deltaTime;
        }       
    }

    private void CheckIfLevelIsPassed()
    {
        if (LevelPassed && !TimesUp)
        {
            PlayerPassed();
        }
        if (TimesUp && !LevelPassed)
        {
            PlayerLost();
        }
    }

    private void PlayerPassed()
    {        
        GameState = GameState.PlayerPassed;

        _playerController.DisableControls();

        StartCoroutine("GoToNextScene");
    }

    private void PlayerLost()
    {
        _gameTime = 0.0f;
        _lives--;

        _playerController.DisableControls();

        if (_lives <= 0)
        {
            GameState = GameState.GameOver;
            StartCoroutine("ResetEntireGame");
        }
        else
        {
            GameState = GameState.PlayerLostLife;
            StartCoroutine("ResetScene");
        }
    }

    private void SaveAndResetScore()
    {
        //On enregistre le score de cette partie qui sera utilis� dans la sc�ne des high scores
        PlayerPrefs.SetFloat("currentScore", (float)_score);
        _score = 0;
    }

    /*
     * Coroutines | https://docs.unity3d.com/Manual/Coroutines.html
     *            |
     *            V
     */
    IEnumerator ResetScene()
    {
        _playerDisplayText.DisplayGameStateText(GameState);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator ResetEntireGame()
    {
        _playerDisplayText.DisplayGameStateText(GameState);

        yield return new WaitForSeconds(3);

        if (GameState == GameState.PlayerWon)
        {
            //Mise � jour du score lorsque le joueur r�ussit � passer le dernier niveau (on doit le faire apr�s le yield return)
            _score += RemainingTime * _lives * 10;
        }

        SaveAndResetScore();

        //Reset lives
        _lives = 3;

        //Aller � la derni�re scene qui est celle des high scores
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }

    IEnumerator GoToNextScene()
    {
        //Si le joueur a pass� tous les niveaux (avant derni�re sc�ne), on affiche la victoire et on va � la sc�ne des high scores � l'aide de la coroutine "ResetEntireGame"
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 2)
        {
            GameState = GameState.PlayerWon;
            StartCoroutine("ResetEntireGame");
        }
        else
        {
            _playerDisplayText.DisplayGameStateText(GameState);

            yield return new WaitForSeconds(3);

            //Mise � jour du score lorsque le joueur r�ussit � passer le niveau (on doit le faire apr�s le yield return)
            _score += RemainingTime * _lives * 10;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
