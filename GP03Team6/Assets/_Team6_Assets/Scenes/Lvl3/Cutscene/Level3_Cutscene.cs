using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Level3_Cutscene : MonoBehaviour
{
    [SerializeField]
    public GameObject cameraPosition;
    private CameraController camGame;
    [SerializeField]
    public Camera camCutscene;

    private PlayerMovement pl;

    private bool startCutscene = false;

    [SerializeField]
    public float moveSpeed;
    [SerializeField]
    public float rotationSpeed;

    [SerializeField]
    public PlayableDirector timeline;

    private void Start()
    {
        pl = PlayerMovement.MyPlayer;
        camGame = CameraController.PlayerCamera;
    }

    void Update()
    {
        if (startCutscene)
        {
            CameraMove();
        }
    }

    private void CameraMove()
    {
        camCutscene.transform.position = Vector3.Slerp(camCutscene.transform.position, cameraPosition.transform.position, Time.deltaTime * moveSpeed);
        camCutscene.transform.rotation = Quaternion.Slerp(camCutscene.transform.rotation, cameraPosition.transform.rotation, Time.deltaTime * rotationSpeed);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(pl.SwitchState(pl.cutsceneState));

            camGame.gameObject.SetActive(false);
            camCutscene.transform.position = camGame.transform.position;
            camCutscene.transform.rotation = camGame.transform.rotation;
            camCutscene.gameObject.SetActive(true);
            startCutscene = true;

            timeline.Play();
        }
    }
}
