using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    private MeshRenderer childRenderer;
    private NavMeshAgent navMeshAgent;

    public delegate void LostPath();
    public static event LostPath OnLostPath;

    private bool lostPath = false;

    private void Awake()
    {
        childRenderer = GetComponentInChildren<MeshRenderer>();
        childRenderer.material.color = Color.red;

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (player == null)
        {
            player = PlayerMovement.MyPlayer;
        }
    }

    private void Update()
    {
        if (navMeshAgent.CalculatePath(player.transform.position, navMeshAgent.path))
        {
            navMeshAgent.SetDestination(player.transform.position);
        }

        if (lostPath == false && navMeshAgent.hasPath == false)
        {
            lostPath = true;
            OnLostPath();
            StartCoroutine("ResetLostPathCooldown");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.Respawn();
        }
    }

    IEnumerator ResetLostPathCooldown()
    {
        yield return new WaitForSeconds(10.0f);
        lostPath = false;
    }
}