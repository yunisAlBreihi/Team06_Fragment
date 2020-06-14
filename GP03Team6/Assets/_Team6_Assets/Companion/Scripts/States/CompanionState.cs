using UnityEngine;
using UnityEngine.AI;

public class CompanionState : StateMachineBehaviour
{
    protected GameObject targetGameObject;
    public GameObject TargetGameObject { get => targetGameObject; set => targetGameObject = value; }

    protected PlayerMovement player;
    protected float distanceToEnd = 2f;
    protected float timer = 0;
    float distanceToPlayer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<Companion>().Player;
    }

    protected void FollowPlayerIfOutOfRange(Animator animator, float distanceToFollowPlayer)
    {
        distanceToPlayer = Vector3.Distance(player.transform.position, animator.transform.position);
        if (distanceToPlayer > distanceToFollowPlayer)
        {
            animator.SetInteger(Transitions.stateName, (int)Transitions.Transition.FOLLOWPLAYER);
            animator.SetBool(Transitions.isFollowingPlayerName, true);
        }
    }

    protected void JumpOverGaps(Animator animator)
    {
        NavMeshAgent agent = animator.GetComponent<NavMeshAgent>();
        
        if (agent.isOnOffMeshLink)
        {
            if (Vector3.Distance(agent.currentOffMeshLinkData.startPos, animator.transform.position) < 1)
            {
                agent.autoTraverseOffMeshLink = false;
                timer += Time.deltaTime;

                if (timer > 0.2f) 
                {
                    animator.SetInteger(Transitions.stateName, (int)Transitions.Transition.JUMP);
                    agent.autoTraverseOffMeshLink = true;
                }
            }
        }
    }

    protected void IsAtEndOfJump(Animator animator)
    {
        NavMeshAgent agent = animator.GetComponent<NavMeshAgent>();

        if (Vector3.Distance(agent.currentOffMeshLinkData.endPos, animator.transform.position) < distanceToEnd)
        {
            animator.SetInteger(Transitions.stateName, (int)Transitions.Transition.LAND);
        }
    }
}