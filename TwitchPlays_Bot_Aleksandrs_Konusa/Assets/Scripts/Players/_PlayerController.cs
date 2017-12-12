using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerController : MonoBehaviour
{
    //State machine
    public enum PlayerState
    {

        Idle,
        Forward,
        Back,
        Left,
        Right

    }

    public PlayerState currentState;

    Rigidbody rb;

    private void Start()
    {

        currentState = PlayerState.Idle;
        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {

        //Player move state machine
        switch (currentState)
        {

            case PlayerState.Idle:
                {

                  // rb.velocity = Vector3.zero;
                    break;

                }

            case PlayerState.Forward:
                {
                    rb.MovePosition(transform.position + transform.forward * 0.1f);
                    break;

                }

            case PlayerState.Back:
                {
                    rb.MovePosition(transform.position + -transform.forward * 0.1f);
                    break;

                }

            case PlayerState.Right:
                {
                   rb.MovePosition(transform.position + transform.right * 0.1f);
                    break;

                }


            case PlayerState.Left:
                {
                   rb.MovePosition(transform.position + -transform.right * 0.1f);
                    break;

                }
        }



    }

    //Public functions to change our states depending on the command
    public void StateForward()
    {
        rb.velocity = Vector3.zero;
        currentState = PlayerState.Forward;

    }

    public void StateBack()
    {
        rb.velocity = Vector3.zero;
        currentState = PlayerState.Back;

    }

    public void StateRight()
    {
        rb.velocity = Vector3.zero;
        currentState = PlayerState.Right;

    }

    public void StateLeft()
    {
        rb.velocity = Vector3.zero;
        currentState = PlayerState.Left;

    }

    public void StateIdle()
    {
        rb.velocity = Vector3.zero;
        currentState = PlayerState.Idle;

    }
}
