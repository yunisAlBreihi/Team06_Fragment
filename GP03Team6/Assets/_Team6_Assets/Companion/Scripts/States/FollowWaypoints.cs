using UnityEngine;
using UnityEngine.AI;

public class FollowWaypoints : CompanionState
{
    WaypointsManager waypointsManager;
    NavMeshAgent agent;
    Waypoint nextWaypoint;
    Vector3 nextDest;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        waypointsManager = animator.GetComponent<WaypointsManager>();
        nextWaypoint = waypointsManager.GetClosestWaypoint();
        nextDest = nextWaypoint.transform.position;
        agent.SetDestination(nextDest);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        JumpOverGaps(animator);
        SetIdle(animator);
    }

    private void SetIdle(Animator animator)
    {
        if (Vector3.Distance(animator.transform.position, nextDest) < agent.stoppingDistance)
        {
            nextWaypoint.DisableWayPoint();
            animator.SetBool(Transitions.isGoingToWaypointName, false);
            animator.SetInteger(Transitions.stateName, (int)Transitions.Transition.IDLE);
        }
    }
}
