using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public enum State
    {
        Idle,
        Run,
        Jump,
        Fall,
        Injured
    }

    protected State state = State.Idle;

    protected virtual void Update()
    {
        switch (state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Run:
                Run();
                break;
            case State.Jump:
                Jump();
                break;
            case State.Fall:
                Fall();
                break;
            case State.Injured:
                Injured();
                break;
            default:
                break;
        }
    }

    protected virtual void Idle()
    {
        
    }

    protected virtual void Run()
    {

    }

    protected virtual void Jump()
    {

    }

    protected virtual void Fall()
    {

    }

    protected virtual void Injured()
    {

    }
}
