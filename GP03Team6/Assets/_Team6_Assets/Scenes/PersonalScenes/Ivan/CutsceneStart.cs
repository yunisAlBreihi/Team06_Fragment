using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class CutsceneStart : MonoBehaviour
{

    [SerializeField]
    public Camera mainCam;
    [SerializeField]
    public CinemachineVirtualCamera cutsceneCam;
    [SerializeField]
    public PlayableDirector cutscene;
    [SerializeField]
    public PlayerMovement pl; 

    private void Awake()
    {
        cutsceneCam.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {


            StartCoroutine(pl.SwitchState(pl.cutsceneState));
            cutsceneCam.gameObject.SetActive(true);

            cutscene.Play();
        }
    }

}
