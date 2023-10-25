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
        switch (currentState)
        {
            case AIState.Idle:
                // Do work for StateOne
                // Check for transitions
                break;
            case AIState.Seek:
                // Do work for Seek
                DoSeekState();
                // Check for transitions
                break;
            case AIState.StateThree:
                // Do work for StateOne
                // Check for transitions
                break;
        }
    }

    public void MakeDecisions()
    {
        Debug.Log("Make Decisions");
    }

    public void Idle()
    {
        // Do nothing
    }
    protected void DoIdleState()
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
    public void DoSeekState()
    {
        // Seek our target
        Seek(target);
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

    public virtual void ChangeState (AIState newState)
    {
        // Change the current state
        currentState = newState;
        // Save the time we changed states
        lastStateChangeTime = Time.time;    
    }

}
