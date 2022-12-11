using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// Classe permettant de gérer le text à afficher à l'écran du texte au dessus du joueur
/// Le texte est suivi par la caméra du joueur
/// </summary>
public class PlayerDisplayText : MonoBehaviour
{
    private readonly string _playerPassedText = "PASSED";
    private readonly string _playerLostText = "TRY AGAIN";
    private readonly string _gameOverText = "GAME OVER";
    private readonly string _gameWonText = "YOU WON";
    private readonly string _gamePauseText = "GAME PAUSED";

    private int _lives;


    /// <summary>
    /// Affiche le texte voulu selon l'état de la partie.
    /// </summary>
    /// <param name="state">L'état de la partie de type enum GameState</param>
    public void DisplayGameStateText(GameState state)
    {
        switch (state)
        {
            case GameState.PlayerPassed:
                gameObject.GetComponent<TextMeshProUGUI>().text = _playerPassedText;
                gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;
                break;
            case GameState.PlayerLostLife:
                gameObject.GetComponent<TextMeshProUGUI>().text = _playerLostText;
                gameObject.GetComponent<TextMeshProUGUI>().color = Color.grey;
                break;
            case GameState.GameOver:
                gameObject.GetComponent<TextMeshProUGUI>().text = _gameOverText;
                gameObject.GetComponent <TextMeshProUGUI>().color = Color.red;
                break;
            case GameState.PlayerWon:
                gameObject.GetComponent<TextMeshProUGUI>().text = _gameWonText;
                gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;
                break;
            case GameState.Paused:
                gameObject.GetComponent<TextMeshProUGUI>().text = _gamePauseText;
                gameObject.GetComponent<TextMeshProUGUI>().color = Color.grey;
                break;
        } 
    }

    /// <summary>
    /// Affiche le nombre de vies restantes pendant 2 secondes.
    /// </summary>
    public void ShowLives(int lives)
    {
        _lives = lives;
        StartCoroutine("ShowRemainingLives");
    }

    /// <summary>
    /// Permet d'éffacer le text au dessus du joueur
    /// </summary>
    public void EraseDisplayText()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = "";
    }


    /*
     * Coroutines | https://docs.unity3d.com/Manual/Coroutines.html
     *            |
     *            V
     */
    IEnumerator ShowRemainingLives()
    {
        string singularOrPlural = _lives == 1 ? "LIFE" : "LIVES";

        gameObject.GetComponent<TextMeshProUGUI>().text = $"{_lives} {singularOrPlural} LEFT!";
        gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;

        //yield on a new YieldInstruction that waits for 2 seconds.
        yield return new WaitForSeconds(1.5f);

        EraseDisplayText(); 

    }

}
