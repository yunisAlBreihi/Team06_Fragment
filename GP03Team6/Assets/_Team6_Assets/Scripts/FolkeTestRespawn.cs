using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FolkeTestRespawn : MonoBehaviour
{
    public bool RespawnThePlayer;
    public RespawnHandler respawnHandler;
    private void Start()
    {
        if(respawnHandler == null)
        {
            GameObject.Find("RespawnHandler");
        }
    }
    void Update()
    {   
        if(RespawnThePlayer)
        {
            respawnHandler.Respawn(respawnHandler.Player.transform);
            RespawnThePlayer = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        { 
            RespawnThePlayer = true;
        }
    }
}
