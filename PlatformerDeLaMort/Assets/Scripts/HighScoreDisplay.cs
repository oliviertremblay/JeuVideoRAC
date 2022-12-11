using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI Score;
    public void DisplayHighScore(float score)
    {
        Score.text = string.Format("{0:000000}", score);
    }
    public void HideEntryDisplay()
    {
        Score.text = "";
    }
}
