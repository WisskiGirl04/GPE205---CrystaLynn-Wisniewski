using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIController : Controller
{
    public enum AIState { Idle, Seek, Flee, TargetPlayerOne, Patrol, Attack, TargetClosestPawn, SeekClosestPawn};
    public AIState currentState;
    //private float lastStateChangeTime;
    public GameObject target;
    public float fleeDistance;
    public Transform[] patrolPoints;
    public float patrolPointStopDistance;
    private int currentPatrolPoint = 0;
    public bool loopPatrol;
    public Pawn targetPawn;
    public float hearingDistance;
    public float fieldOfView;


    // Get a list of all the tanks (pawns)
    public Pawn[] allTanks;

    public Pawn closestTank;
    public float closestTankDistance;


    // Start is called before the first frame update
    public override void Start()
    {
        if (pawn != null)
        {
            // If(when) we have a GameManager
            if (GameManager.instance != null)
            {
                // And we have an ai enemy(ies) list
                if (GameManager.instance.aiList != null)
                {
                    // Add the PlayerControllerObject that is being created to the list
                    GameManager.instance.aiList.Add(this);
                    // the .gameObject is because i made this a gameobject list in game manager
                }
            }
            base.Start();
            //Debug.Log("ai controller setting start state");
            ChangeState(AIState.Patrol);
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        // Make Decisions
        MakeDecisions();
        // Run the parents update
         base.Update();  
    }

    public virtual void MakeDecisions()
    {
        if(pawn == null)
        {
            Destroy(gameObject);
        }
        if (pawn != null)
        {


            switch (currentState)
            {
                case AIState.Idle:
                    // Do work
                    DoIdleState();
                    // Check for transitions
                    if (IsHasTarget() && IsCanSee(target))
                    {
                        ChangeState(AIState.Seek);
                    }
                    /*if (!IsHasTarget() && IsDistanceLessThan(target, 10))
                    {
                        ChangeState(AIState.Seek);
                    }
                    else
                    {
                        ChangeState(AIState.TargetPlayerOne);
                    }*/
                    break;
                case AIState.Seek:
                    // Do work
                    // Check for transitions
                    if (IsHasTarget() && IsDistanceLessThan(target, 10))
                    {
                        ChangeState(AIState.Attack);
                    }
                    break;
                /*            case AIState.Flee:
                                // Do work
                                DoFleeState();
                                // Check for transition
                                if (IsHasTarget() && IsDistanceLessThan(target, 10))
                                {
                                    //ChangeState(AIState.Idle);
                                    ChangeState(AIState.Patrol);
                                }
                                else
                                {
                                    //ChangeState(AIState.TargetPlayerOne);
                                    ChangeState(AIState.Idle);
                                }
                                break;*/
                case AIState.SeekClosestPawn:
                    // Do Work
                    DoPawnSeekState();
                    break;
                case AIState.Attack:
                    // Do work
                    if (IsHasTarget())
                    {
                        DoAttackState();
                    }
                    if (!IsHasTarget())
                    {
                        ChangeState(AIState.Idle);
                    }
                    break;
                case AIState.Patrol:
                    // Do work
                    DoPatrolState();
                    break;
                case AIState.TargetPlayerOne:
                    DoChooseTargetState();
                    ChangeState(AIState.Seek);
                    break;
                case AIState.TargetClosestPawn:
                    DoTargetClosest();
                    ChangeState(AIState.SeekClosestPawn);
                    break;


                    //IsCanSee(target)
                    //IsCanHear(target)
            }
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

    public void Seek(GameObject target)
    {
        // RotateTowards the Funciton
        pawn.RotateTowards(target.transform.position);
        // Move Forward
        pawn.MoveForward();
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

    public void Seek(Pawn target)
    {
        // Seek the pawn's transform!
        // Seek(targetPawn.transform);
        pawn.RotateTowards(target.transform.position);
        pawn.MoveForward();
        
    }

    protected virtual void DoPawnSeekState()
    {
        Debug.Log("seeking ~ " + target.name);
        Seek(target);
    }

    protected virtual void DoSeekState()
    {
        //Seek the target
        Seek(target);
    }

    protected virtual void DoAttackState()
    {
        if (target != null)
        {
            // Chase
            Seek(target);
            // Shoot
            Shoot();
        }
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
            //...
            // if statement keeps coming back as not haveing an instance of an object for the object reference
            //if(this.gameObject.GetComponent<Pawn>().GetComponent<Health>().currentHealth > 0)
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
        // lastStateChangeTime = Time.time;
        //Debug.Log(lastStateChangeTime);
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

    protected void TargetNearestTank()
    {
       // Get a list of all the tanks (pawns)
        Pawn[] allTanks = FindObjectsOfType<Pawn>();

        // Assume that the first tank is closest
        Pawn closestTank = allTanks[0];
        float closestTankDistance = Vector3.Distance(pawn.transform.position, closestTank.transform.position);

        // Iterate through them one at a time
        foreach (Pawn tank in allTanks)
        {
            if (tank != this.pawn)
            {
                Debug.Log("tank added - " + tank);
                // If this one is closer than the closest
                // (remember we assume the first tank in the list is the closest at first)
                if (Vector3.Distance(pawn.transform.position, tank.transform.position) <= closestTankDistance)
                {
                    // It is the closest
                    closestTank = tank;
                    closestTankDistance = Vector3.Distance(pawn.transform.position, closestTank.transform.position);
                }
            }
        }

        // Target the closest tank
        target = closestTank.gameObject;
        Debug.Log("targeting = " + target.name);
    }

    protected virtual void DoTargetClosest()
    {
        // Do Work
        TargetNearestTank();
    }

    public bool IsCanHear(GameObject target)
    {
        // Get the target's NoiseMaker
        NoiseMaker noiseMaker = target.GetComponent<NoiseMaker>();
        // If they don't have one, they can't make noise, so return false
        if (noiseMaker == null)
        {
            return false;
        }
        // If they are making 0 noise, they also can't be heard
        if (noiseMaker.volumeDistance <= 0)
        {
            return false;
        }
        // If they are making noise, add the volumeDistance in the noisemaker to the hearingDistance of this AI
        float totalDistance = noiseMaker.volumeDistance + hearingDistance;
        // If the distance between our pawn and target is closer than this...
        if (Vector3.Distance(pawn.transform.position, target.transform.position) <= totalDistance)
        {
            // ... then we can hear the target
            return true;
        }
        else
        {
            // Otherwise, we are too far away to hear them
            return false;
        }
    }

    public bool IsCanSee(GameObject target)
    {
        // Find the vector from the agent to the target
        Vector3 agentToTargetVector = target.transform.position - pawn.transform.position;
        // Find the angle between the direction our agent is facing (forward in local space) and the vector to the target.
        float angleToTarget = Vector3.Angle(agentToTargetVector, pawn.transform.forward);
        Debug.Log(angleToTarget);
        // if that angle is less than our field of view
        if (angleToTarget < fieldOfView)
        {
            Debug.Log("In field of view!");
            Debug.Log("True!");
            return true;
        }
        else
        {
            return false;
        }
    }
    public void OnDestroy()
    {
        // If we have a GameManager
        if (GameManager.instance != null)
        {
            // And it has a player(s) list
            if (GameManager.instance.aiList != null)
            {
                // Unregister with the list
                GameManager.instance.aiList.Remove(this);

            }
        }
    }

}



// Other Seek Ideas.. Incomplete
/*    public void Seek(Controller targetController)
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
    } */

