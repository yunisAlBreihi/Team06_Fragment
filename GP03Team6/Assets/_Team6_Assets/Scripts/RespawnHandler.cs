using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RespawnHandler : MonoBehaviour
{
    public List<RespawnPoint> ListofRespawns = new List<RespawnPoint>();
    public GameObject Player;
    private void Start()
    {
        if(Player == null)
        {
            Player = PlayerMovement.MyPlayer.gameObject;//GameObject.FindGameObjectWithTag("Player");
        }
        for (int i = 0; i < ListofRespawns.Count; i++)
        {
            ListofRespawns[i].StartRespawnPoint(this);
        }
    }
    public void SetNewRespawnPoint()
    {
        for (int i = 0; i < ListofRespawns.Count; i++)
        {
            if (ListofRespawns[i].currentActive == true)
            { 
                ListofRespawns[i].currentActive = false;
                break;
            }
        }
    }
    public void Respawn(Transform currentLocation)
    {
        for (int i = 0; i < ListofRespawns.Count; i++)
        {
            if (ListofRespawns[i].currentActive == true)
            {
                currentLocation.gameObject.SetActive(false);
                currentLocation.position = ListofRespawns[i].respawnLocation.position;
                currentLocation.gameObject.SetActive(true);
                break;
            }
        }
    }
}
