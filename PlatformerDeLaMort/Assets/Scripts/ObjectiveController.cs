using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// La classe permettant de g�rer l'objectif d'un niveau.
/// Sert principalement � d�tecter le contact avec le joueur.
/// </summary>
public class ObjectiveController : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private AudioSource _backgroundSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Arr�t de la musique principale
            _backgroundSound.Stop();

            //Joue le clip audio de fin de tableau
            gameObject.GetComponent<AudioSource>().Play();

            _gameManager.LevelPassed = true;
        }
    }
}
