using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIController : Controller
{
    public enum AIState { Idle, Seek, Flee, TargetPlayerOne, Patrol, Attack};
    public AIState currentState;
    private float lastStateChangeTime;
    public GameObject target;
    public float fleeDistance;
    public Transform[] patrolPoints;
    public float patrolPointStopDistance;
    private int currentPatrolPoint = 0;
    public bool loopPatrol;

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
                if (!IsHasTarget() && IsDistanceLessThan(target, 10))
                {
                    ChangeState(AIState.Seek);
                }
                else
                {
                    ChangeState(AIState.TargetPlayerOne);
                }
                break;
            case AIState.Seek:
                // Do work
                DoAttackState();
                // Check for transitions
                if (IsHasTarget() && IsDistanceLessThan(target, 10))
                {
                    ChangeState(AIState.Idle);
                }
                else
                {
                    ChangeState(AIState.TargetPlayerOne);
                }
                break;
            case AIState.Flee:
                // Do work
                DoFleeState();
                // Check for transition
                if (IsHasTarget() && IsDistanceLessThan(target, 10))
                {
                    //ChangeState(AIState.Idle);
                }
                else
                {
                    ChangeState(AIState.TargetPlayerOne);
                }
                break;
                case AIState.Attack:
                // Do work
                DoAttackState();
                break;
            case AIState.Patrol:
                // Do work
                DoPatrolState();
                break;
            case AIState.TargetPlayerOne:
                DoChooseTargetState();
                ChangeState(AIState.Seek);
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

    protected virtual void DoSeekState()
    {
        //Seek the target
        Seek(target);
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
    }
    protected virtual void DoFleeState()
    {
        // Flee
        Flee();
    }

    protected virtual void Patrol()
    {
        // If we have a enough waypoints in our list to move to a current waypoint
        if (patrolPoints.Length > currentPatrolPoint)
        {
            // Then seek that waypoint
            Seek(patrolPoints[currentPatrolPoint]);
            // If we are close enough, then increment to next waypoint
            if (Vector3.Distance(pawn.transform.position, patrolPoints[currentPatrolPoint].position) < patrolPointStopDistance)
            {
                currentPatrolPoint++;
            }
        }
        else if(loopPatrol == true)
        {
            RestartPatrol();
        }
    }

    protected virtual void DoPatrolState()
    {
        //Start Patrols
        Patrol();
    }

    protected void RestartPatrol()
    {
        // Set the index back to 0
        currentPatrolPoint = 0;
    }

    public virtual void ChangeState (AIState newState)
    {
        // Change the current state
        currentState = newState;
        // Save the time we changed states
        lastStateChangeTime = Time.time;
        Debug.Log(lastStateChangeTime);
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

    public void TargetPlayerOne()
    {
        // If the GameManager exists
        if (GameManager.instance != null)
        {
            // And the array of players exists
            if (GameManager.instance.playersList != null)
            {
                // And there are players in it
                if (GameManager.instance.playersList.Count > 0)
                {
                    //Then target the gameObject of the pawn of the first player controller in the list
                    target = GameManager.instance.playersList[0].pawn.gameObject;
                }
            }
        }
    }
    protected virtual void DoChooseTargetState()
    {
        // Do work
        TargetPlayerOne();
    }
    protected bool IsHasTarget()
    {
        // return true if we have a target, false if we don't
        return (target != null);
    }
}
