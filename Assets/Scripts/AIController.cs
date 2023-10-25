using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIController : Controller
{
    public enum AIState { Idle, Seek, Flee, StateFour };
    public AIState currentState;
    private float lastStateChangeTime;
    public GameObject target;
    public float fleeDistance;

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
            case AIState.Flee:
                // Do work
                DoFleeState();
                // Check for transition
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
        Seek(targetController.transform.position);
    }
    public void Seek(Vector3 targetPosition)
    {
        // RotateTowards the Function
        pawn.RotateTowards(targetPosition);
        // Move Forward
        pawn.MoveForward();
    }
    public void Seek(Transform targetTransform)
    {
        // Seek the position of our target Transform
        Seek(targetTransform.position);
    }
    public void Seek(Pawn targetPawn)
    {
        // Seek the pawn's transform!
        Seek(targetPawn.transform);
    }
    protected virtual void DoAttackState()
    {
        // Chase
        Seek(target);
        // Shoot
        Shoot();
    }

    protected virtual void Flee()
    {
        // Find the Vector to our target
        Vector3 vectorToTarget = target.transform.position - pawn.transform.position;
        // Find the Vector away from our target by multiplying by -1
        Vector3 vectorAwayFromTarget = -vectorToTarget;
        // Find the vector we would travel down in order to flee
        Vector3 fleeVector = vectorAwayFromTarget.normalized * fleeDistance;
        // Seek the point that is "fleeVector" away from our current position
        Seek(pawn.transform.position + fleeVector);
        // Find distance the target is from player
        float targetDistance = Vector3.Distance(target.transform.position, pawn.transform.position);
        // Find the percentage of our FleeDistance that the target is away from our AI
        float percentOfFleeDistance = targetDistance / fleeDistance;
        // Clamp so we don't flee "backwards" and don't flee too far
        percentOfFleeDistance = Mathf.Clamp01(percentOfFleeDistance);
        // Reverse percentage so AI flees farther the closer it is to player
        float flippedPercentOfFleeDistance = 1 - percentOfFleeDistance;
        // Rotate towards target distance??
        //pawn.RotateTowards()
        // Move forward???
    }
    protected virtual void DoFleeState()
    {
        // Flee
        Flee();
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
