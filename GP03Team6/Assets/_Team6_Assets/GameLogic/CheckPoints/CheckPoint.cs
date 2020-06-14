using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]private Transform respawnTransform;


    private void Start()
    {
        respawnTransform = transform.GetChild(0).transform;    
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().respawnRot = respawnTransform.rotation;
            other.gameObject.GetComponent<PlayerMovement>().respawnPos = respawnTransform.position;

        }
    }

}
