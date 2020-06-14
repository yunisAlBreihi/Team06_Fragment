using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement p = other.gameObject.GetComponent<PlayerMovement>();
            p.Respawn();
        }
        else if (other.gameObject.GetComponentInChildren<PushableObject>() != null) 
        {
            other.gameObject.GetComponentInChildren<PushableObject>().Respawn();
        }
    }




}
