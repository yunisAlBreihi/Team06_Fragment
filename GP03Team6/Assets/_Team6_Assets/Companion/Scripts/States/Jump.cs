using UnityEngine;
using UnityEngine.AI;

public class Jump : CompanionState
{
    Companion companion;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        companion = animator.GetComponent<Companion>();
        companion.CompanionNavMeshAgent = animator.GetComponent<NavMeshAgent>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        IsAtEndOfJump(animator);
    }
}
