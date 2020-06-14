using UnityEngine;
using UnityEngine.AI;

public class Wander : CompanionState
{
    [SerializeField, Tooltip("The max Distance to wander from the current position")] private float maxWanderDistance = 2.0f;

    private Vector3 startPosition;
    Vector2 randomCircleDistance;
    Vector3 randomPos;
    private NavMeshAgent navMeshAgent;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        navMeshAgent = animator.GetComponent<NavMeshAgent>();

        startPosition = animator.transform.position;
        randomCircleDistance = Random.insideUnitCircle * maxWanderDistance;
        randomPos = new Vector3(startPosition.x + randomCircleDistance.x, startPosition.y, startPosition.z + randomCircleDistance.y);

        MoveToRandomPosition(animator.transform);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SetStateToIdle(animator);
    }

    private void MoveToRandomPosition(Transform transform) 
    {
        randomPos = new Vector3(startPosition.x + randomCircleDistance.x, startPosition.y, startPosition.z + randomCircleDistance.y);

        navMeshAgent.SetDestination(randomPos);
    }

    private void SetStateToIdle(Animator animator) 
    {
        if (navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            animator.SetInteger(Transitions.stateName, (int)Transitions.Transition.IDLE);
        }
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
