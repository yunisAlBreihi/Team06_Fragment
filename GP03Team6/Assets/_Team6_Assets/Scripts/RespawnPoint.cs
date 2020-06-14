using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public Transform respawnLocation;
    public bool currentActive = false;
    private RespawnHandler rspwnHandle;
    public void StartRespawnPoint(RespawnHandler respawnHandler)
    {
        rspwnHandle = respawnHandler;
        respawnLocation = transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            rspwnHandle.SetNewRespawnPoint();
            currentActive = true;
        }
    }

}
