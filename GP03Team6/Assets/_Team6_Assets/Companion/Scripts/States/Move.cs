using UnityEngine;
using UnityEngine.AI;

public class Move : CompanionState
{
    [SerializeField, Tooltip("The distance before changing to the next state")] private float DistanceToChangeState = 3.0f;

    private Companion companion;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        companion = animator.GetComponent<Companion>();

        companion.CompanionNavMeshAgent = animator.GetComponent<NavMeshAgent>();

        companion.CompanionNavMeshAgent.speed = 16;

        //shouldZip = IfShouldZip(animator.transform);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        companion.CompanionNavMeshAgent.enabled = true;
        companion.CompanionNavMeshAgent.speed = 8;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        companion.CompanionNavMeshAgent.SetDestination(targetGameObject.transform.position);
        SetNextState(animator);
        JumpOverGaps(animator);
    }

    private void SetNextState(Animator animator)
    {
        if (Vector3.Distance(animator.transform.position, targetGameObject.transform.position) < companion.CompanionNavMeshAgent.stoppingDistance)
        {
            if (targetGameObject.CompareTag("Enemy"))
            {
                SetAttackTarget(animator);
                animator.SetInteger(Transitions.stateName, (int)Transitions.Transition.ATTACK);
            }
            else if (targetGameObject.CompareTag("Intractable"))
            {
                SetInteractTarget(animator);
                animator.SetInteger(Transitions.stateName, (int)Transitions.Transition.INTERACT);
            }
            else
            {
                animator.SetInteger(Transitions.stateName, (int)Transitions.Transition.IDLE);
            }
        }
    }

    private void SetAttackTarget(Animator animator)
    {
        animator.GetBehaviour<Attack>().TargetGameObject = targetGameObject;
    }

    private void SetInteractTarget(Animator animator)
    {
        animator.GetBehaviour<Interact>().TargetGameObject = targetGameObject;
    }

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
