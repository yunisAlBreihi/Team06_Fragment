using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerState : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement.MyPlayer.SwitchState(PlayerMovement.MyPlayer.airState);
        }
    }
}
