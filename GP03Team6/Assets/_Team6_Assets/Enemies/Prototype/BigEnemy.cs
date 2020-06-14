using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigEnemy : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField]private Transform player;

    private Material mat;
    public float baseSpeed = 5f, speedVariance = 3f, noiseAmount = 0.1f, distanceThreshhold = 10f;
    private float addSpeed, currentSpeed;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        agent = GetComponent<NavMeshAgent>();
        mat = GetComponent<MeshRenderer>().material;
        agent.speed = baseSpeed;
        agent.angularSpeed = 100f;
        StartCoroutine(SpeedBurst());
    }

    void Update()
    {
        if((transform.position - player.position).magnitude > distanceThreshhold)
        {
            addSpeed = Mathf.Lerp(addSpeed, 4f, Time.deltaTime * 4f);
        }
        else
        {
            addSpeed = Mathf.Lerp(addSpeed, 0f, Time.deltaTime * 4f);
        }
    }
    private IEnumerator SpeedBurst()
    {
        float f = UnityEngine.Random.Range(2f, 2.5f);
        float f2 = UnityEngine.Random.Range(6f, 7f);

        while (Mathf.Abs(currentSpeed - f)>0.01f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, f, Time.deltaTime * 10f);
            agent.speed = currentSpeed + addSpeed;
            yield return new WaitForEndOfFrame();
        }
        while (Mathf.Abs(currentSpeed - f2) > 0.01f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, f2, Time.deltaTime * 5f);
            agent.speed = currentSpeed + addSpeed;
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(SpeedBurst());
    }
}
