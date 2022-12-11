using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Permet de controller le temps restant pendant la compl�tion d'un tableau
/// </summary>
public class TimeDisplayControl : MonoBehaviour
{
    /// <summary>
    /// Affiche le temps sur la composante de texte de l'objet
    /// <param name="time">Le temps restant pour compl�ter le tableau</param>
    /// </summary>
    public void DisplayTime(double time)
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = time.ToString();
    }
}
