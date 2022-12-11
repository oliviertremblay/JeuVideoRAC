using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// La classe permettant de gérer le joueur ainsi que son état
/// Sert principalement à gérer son mouvement.
/// Gère également le nombre de vies du joueur.
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Permet de désactiver les contrôles du joueur de façon rudimentaire
    /// </summary>
    public void DisableControls()
    {
        _speed = 0;
        _verticalSpeed = 0;
    }

    /// <summary>
    /// Permet de d'activer les contrôles du joueur de façon rudimentaire
    /// </summary>
    public void EnableControls()
    {
        _speed = 240f;
        _verticalSpeed = 70f;
    }

    private bool _isFacingLeft;
    private Vector2 _facingLeft;

    private Rigidbody2D _rigidbody;
    private float _speed;
    private float _verticalSpeed;
    private float _inputHorizontal;
    private float _inputVertical;
    private bool _grounded = true;

    private Animator _animator;
    //Animation states
    private string _currentState;
    private readonly string PLAYER_IDLE = "Player_Idle";
    private readonly string PLAYER_MOVE = "Player_Move";
    private readonly string PLAYER_JUMP = "Player_Jump";

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        EnableControls();
        _facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {        
        PlayerMovement();      
    }

    //Change l'avatar de direction
    private void Flip()
    {
        if (_isFacingLeft)
        {
            transform.localScale = _facingLeft;
        }
        if (!_isFacingLeft)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    private void PlayerMovement()
    {
        _inputHorizontal = Input.GetAxisRaw("Horizontal");
        _inputVertical = Input.GetAxisRaw("Vertical");

        //Vérifie si on doit changer de direction
        if(_inputHorizontal > 0 && _isFacingLeft)
        {
            _isFacingLeft = false;
            Flip();
        }
        if (_inputHorizontal < 0 && !_isFacingLeft)
        {
            _isFacingLeft = true;
            Flip();
        }

        //Le joueur touche à rien sur le clavier et n'est pas dans les airs
        if(_inputHorizontal == 0 && _inputVertical == 0 && _rigidbody.velocity.y == 0)
        {
            ChangeAnimationState(PLAYER_IDLE);
        }

        Jump();
        MoveLeftAndRight();
    }

    private void MoveLeftAndRight()
    {
        if(_inputHorizontal != 0 && _grounded && _rigidbody.velocity.y == 0)
        {
            ChangeAnimationState(PLAYER_MOVE);
        }
        // Deplacement au sol
        if (_inputHorizontal != 0 && _grounded)
        {           
            if (_inputVertical < 0)
            {
                _rigidbody.AddForce(new Vector2(_inputHorizontal * _speed / 2, 0));
            }
            else
            {
                _rigidbody.AddForce(new Vector2(_inputHorizontal * _speed, 0));
            }
        }
        //Deplacement horizontaal dans les airs
        else if (_inputHorizontal != 0 && !_grounded)
        {
            _rigidbody.AddForce(new Vector2(_inputHorizontal * _speed / 2, 0));
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown("space") && _grounded && _rigidbody.velocity.y == 0)
        {
            ChangeAnimationState(PLAYER_JUMP);
            _rigidbody.AddForce(new Vector2(0, _verticalSpeed), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        /*
         * Si on entre en contact avec le sol ou une plateforme 
         * On considère le joueur comme étant au sol (grounded)
         */
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform")
        {
            _grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        /*
         * Si on perd le contact avec le sol ou une plateforme, 
         * On considère le joueur comme n'étant plus au sol
         */
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Platform")
        {
            _grounded = false;
        }
    }

    private void ChangeAnimationState(string newState)
    {
        //Va empêcher une animation de s'intérompe elle même
        if(_currentState == newState)
            return;

        //Joue l'animation
        _animator.Play(newState);

        //Réassigne l'état d'animation
        _currentState = newState;
    }
}
