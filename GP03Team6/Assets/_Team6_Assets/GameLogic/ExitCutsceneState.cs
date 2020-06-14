using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCutsceneState : MonoBehaviour
{
    private PlayerMovement p;

    [SerializeField]
    public GameObject shitToHide;

    private void Start()
    {
        p = PlayerMovement.MyPlayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            player.airSettings.maxFallSpeed = -22f;

            shitToHide.SetActive(false);
            if(player.currentState == player.cutsceneState)
            {
                StartCoroutine(player.SwitchState(player.groundMoveState));
            }
        }
    }


}
