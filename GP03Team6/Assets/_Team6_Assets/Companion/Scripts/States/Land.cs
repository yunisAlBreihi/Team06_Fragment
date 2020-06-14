using UnityEngine;

public class Land : CompanionState
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool(Transitions.isGoingToWaypointName) == true)
        {
            animator.SetInteger(Transitions.stateName, (int)Transitions.Transition.FOLLOWWAPOINTS);
        }
        else if (animator.GetBool(Transitions.isFollowingPlayerName) == true)
        {
            animator.SetInteger(Transitions.stateName, (int)Transitions.Transition.FOLLOWPLAYER);
        }
        else
        {
            animator.SetInteger(Transitions.stateName, (int)Transitions.Transition.MOVE);
        }
    }
}
