using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : CompanionState
{
    [SerializeField, Tooltip("The distance before changing to the next state")] private float DistanceToChangeState = 3.0f;

    private NavMeshAgent navMeshAgent;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, animatorStateInfo, layerIndex);
        navMeshAgent = animator.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent.SetDestination(player.transform.position);
        ChangeToNextState(animator);
        JumpOverGaps(animator);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent.enabled = true;
    }

    private void ChangeToNextState(Animator animator)
    {
        if (Vector3.Distance(player.transform.position, animator.transform.position) < navMeshAgent.stoppingDistance)
        {
            animator.SetInteger(Transitions.stateName, (int)Transitions.Transition.IDLE);
            animator.SetBool(Transitions.isFollowingPlayerName, false);
        }
    }
}
