using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIController : Controller
{
    public enum AIState { Idle, Seek, StateThree, StateFour };
    public AIState currentState;
    private float lastStateChangeTime;
    public GameObject target;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        ChangeState(AIState.Idle);
    }

    // Update is called once per frame
    public override void Update()
    {
        // Make Decisions
        MakeDecisions();
        // Run the parents update
        // base.Update();  
    }

    public void MakeDecisions()
    {
        switch (currentState)
        {
            case AIState.Idle:
                // Do work
                DoIdleState();
                // Check for transitions
                if(IsDistanceLessThan(target, 10))
                {
                    ChangeState(AIState.Seek);
                }
                break;
            case AIState.Seek:
                // Do work
                DoAttackState();
                // Check for transitions
                if(!IsDistanceLessThan(target, 10))
                {
                    ChangeState(AIState.Idle);
                }
                break;
        }
    }

    public void Idle()
    {
        // Do nothing
    }
    protected virtual void DoIdleState()
    {
        // Do nothing
    }

    // Previously had public void Seek(Vector3 targetPosition)
    public void Seek(GameObject target)
    {
        // RotateTowards the Funciton
        pawn.RotateTowards(target.transform.position);
        // Move Forward
        pawn.MoveForward();
    }
    public void Seek(Controller targetController)
    {
        // RotateTowards the Function
        pawn.RotateTowards(targetController.transform.position);
        pawn.MoveForward();
    }
    public void Seek(Vector3 targetPosition)
    {
        // RotateTowards the Funciton
        pawn.RotateTowards(targetPosition);
        // Move Forward
        pawn.MoveForward();
    }
    public void Seek(Transform targetTransform)
    {
        // Seek the position of our target Transform
        Seek(targetTransform.position);
        pawn.MoveForward();
    }
    public void Seek(Pawn targetPawn)
    {
        // Seek the pawn's transform!
        Seek(targetPawn.transform);
        pawn.MoveForward();
    }
    protected virtual void DoAttackState()
    {
        // Chase
        Seek(target);
        // Shoot
        Shoot();
    }

    public virtual void ChangeState (AIState newState)
    {
        // Change the current state
        currentState = newState;
        // Save the time we changed states
        lastStateChangeTime = Time.time;    
    }

    protected bool IsDistanceLessThan(GameObject target, float distance)
    {
        if(Vector3.Distance (pawn.transform.position, target.transform.position) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Shoot()
    {
        // Tell the pawn to shoot
        pawn.Shoot();
    }

}
