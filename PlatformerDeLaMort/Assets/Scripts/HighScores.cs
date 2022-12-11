using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Regroupe la gestion des 5 plus hauts scores enregistrés
/// </summary>
public class HighScores : MonoBehaviour
{
    /// <summary>
    /// Tableau accessible à partir de l'éditeur de Unity
    /// </summary>
    public HighScoreDisplay[] highScoreDisplayArray;

    public HighScoreDisplay CurrentScore;

    private float _currentScore;

    private List<float> _scores = new List<float>();    

    void Start()
    {
        GetScores();
        UpdateScoresWithCurrentScore();
        SaveScores();
        UpdateDisplay();
    }

    private void GetScores()
    {
        _scores = new List<float>();

        _scores.Add(PlayerPrefs.GetFloat("highScore1", 0));
        _scores.Add(PlayerPrefs.GetFloat("highScore2", 0));
        _scores.Add(PlayerPrefs.GetFloat("highScore3", 0));
        _scores.Add(PlayerPrefs.GetFloat("highScore4", 0));
        _scores.Add(PlayerPrefs.GetFloat("highScore5", 0));
    }

    private void SaveScores()
    {
        PlayerPrefs.SetFloat("highScore1", _scores[0]);
        PlayerPrefs.SetFloat("highScore2", _scores[1]);
        PlayerPrefs.SetFloat("highScore3", _scores[2]);
        PlayerPrefs.SetFloat("highScore4", _scores[3]);
        PlayerPrefs.SetFloat("highScore5", _scores[4]);
    }

    private void UpdateScoresWithCurrentScore()
    {
        _currentScore = PlayerPrefs.GetFloat("currentScore", 0);
        _scores.Add(_currentScore);
        _scores.Remove(_scores.Min());
    }

    void UpdateDisplay()
    {
        _scores.Sort((float x, float y) => y.CompareTo(x));
        for (int i = 0; i < highScoreDisplayArray.Length; i++)
        {
            if (_scores[i] > 0)
            {
                highScoreDisplayArray[i].DisplayHighScore(_scores[i]);
            }
            else
            {
                highScoreDisplayArray[i].HideEntryDisplay();
            }
        }

        CurrentScore.DisplayHighScore(_currentScore);
        _currentScore = 0;
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(0);
    }
}
