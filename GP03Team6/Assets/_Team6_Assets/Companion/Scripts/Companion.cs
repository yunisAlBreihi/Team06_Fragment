using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour
{
    private PlayerMovement player;

    private Animator animator;
    private NavMeshAgent navMeshAgent;
    public NavMeshAgent CompanionNavMeshAgent { get => navMeshAgent; set => navMeshAgent = value; }
    public PlayerMovement Player => player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        player = PlayerMovement.MyPlayer;
    }

    private void OnEnable()
    {
        CompanionEvents.OnMove += SetMoveTarget;
    }

    private void OnDisable()
    {
        CompanionEvents.OnMove -= SetMoveTarget;
    }

    private void SetMoveTarget(GameObject gameObject)
    {
        animator.SetInteger(Transitions.stateName, (int)Transitions.Transition.MOVE);
        animator.GetBehaviour<Move>().TargetGameObject = gameObject;
    }
}
