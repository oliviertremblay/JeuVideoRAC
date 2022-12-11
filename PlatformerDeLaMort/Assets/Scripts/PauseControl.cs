using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Permet de facilement mettre la partie sur pause
/// Tiré de https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/
/// </summary>
public class PauseControl : MonoBehaviour
{
    public static bool gameIsPaused;

    [SerializeField] PlayerController _playerController;
    [SerializeField] PlayerDisplayText _playerDisplayText;
    [SerializeField] GameManager _gameManager;
    [SerializeField] AudioSource _backgroundSound;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }
    void PauseGame()
    {
        if (gameIsPaused)
        {
            _gameManager.GameState = GameState.Paused;
            _gameManager.ShowQuitButton(true);
            _playerController.DisableControls();
            _playerDisplayText.DisplayGameStateText(_gameManager.GameState);
            _backgroundSound.Pause();
            Time.timeScale = 0f;
        }
        else
        {
            _gameManager.ShowQuitButton(false);
            _gameManager.GameState = GameState.Playing;
            _playerController.EnableControls();
            _playerDisplayText.EraseDisplayText();
            _backgroundSound.Play();
            Time.timeScale = 1;
        }
    }
}
