using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAggresive : AIController
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        this.gameObject.name = "AIAggressive Controller";
    }

    // Update is called once per frame
    public override void Update()
    {
        MakeDecisions();
    }

    public override void MakeDecisions()
    {
        switch (currentState)
        {
            case AIState.Idle:
                // Do work
                DoIdleState();
                // Check for transitions
                if (IsCanSee(target))
                {
                    ChangeState(AIState.Seek);
                }
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
                    ChangeState(AIState.Attack);
                }
                else
                {
                    //ChangeState(AIState.TargetPlayerOne);
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
            case AIState.TargetClosestPawn:
                DoTargetClosest();
                ChangeState(AIState.SeekClosestPawn);
                break;


                //IsCanSee(target)
                //IsCanHear(target)
        }
    }
}
