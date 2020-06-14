using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EyeMovement : MonoBehaviour
{
    [SerializeField] private PlayerMovement p;
    private GameObject player;

    float randomTime;

    private void Start()
    {
        p = PlayerMovement.MyPlayer;
        player = p.gameObject;
        //StartCoroutine(MoveEyeRandomly());
    }

    void Update()
    {
        Vector3 forward = player.transform.position - transform.position;
        Quaternion rot = Quaternion.Euler(0f,-90f,0f);
        transform.rotation = Quaternion.LookRotation(forward, Vector3.up) * rot;

    }

    IEnumerator MoveEyeRandomly() 
    {
        randomTime = Random.Range(1.0f, 3.0f);
        yield return new WaitForSeconds(randomTime);
        transform.Rotate(new Vector3(0, Random.Range(-120.0f, -60.0f), Random.Range(-40.0f, 80.0f)),Space.Self);
        StartCoroutine(MoveEyeRandomly());
    }
}
