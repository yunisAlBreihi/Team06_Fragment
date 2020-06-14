using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : CompanionState
{
    [SerializeField, Tooltip("The Min and Max random time before starting to wander")] private Vector2 randomTimeRangeUntilWander = new Vector2(1.0f,2.0f);
    [SerializeField, Tooltip("How far the player go before starting to follow")] private float distanceToFollowPlayer = 10.0f;
    [SerializeField, Tooltip("How Close the player should be before moving towards the next waypoint")] private float distanceFromPlayerBeforeNextWaypoint = 10.0f;

    private float wanderTimer = 0.0f;
    private float timeUntilWander = 0.0f;
    WaypointsManager waypointsManager;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        waypointsManager = animator.GetComponent<WaypointsManager>();
        timeUntilWander = Random.Range(randomTimeRangeUntilWander.x, randomTimeRangeUntilWander.y);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (waypointsManager.WaypointLists.Count > 0 && Vector3.Distance(player.transform.position, animator.transform.position) < distanceFromPlayerBeforeNextWaypoint)
        {
            animator.SetBool(Transitions.isGoingToWaypointName, true);
            animator.SetInteger(Transitions.stateName, (int)Transitions.Transition.FOLLOWWAPOINTS);
        }
        else if (waypointsManager.WaypointLists.Count == 0)
        {
            FollowPlayerIfOutOfRange(animator, distanceToFollowPlayer);
            //SetStateToWander(animator);
        }
    }

    private void SetStateToWander(Animator animator) 
    {
        if (wanderTimer >= timeUntilWander)
        {
            animator.SetInteger(Transitions.stateName, (int)Transitions.Transition.WANDER);
            wanderTimer = 0.0f;
        }
        wanderTimer += Time.deltaTime;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
