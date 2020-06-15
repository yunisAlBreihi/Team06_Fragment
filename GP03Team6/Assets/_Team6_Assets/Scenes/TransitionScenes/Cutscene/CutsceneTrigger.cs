using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField]
    public Camera cam;

    [SerializeField]
    public PlayableDirector timeline;

    private bool cutseneActive = false;
    private bool camMove = false;

    private PlayerMovement pl;
    private CameraController camMain;

    [SerializeField]
    public GameObject cutsceneCh;

    private bool enterCutscene = false;

    private Vector3 camOriginPos;
    private Quaternion camOriginRot;

    public List<string> PreviousToUnload = new List<string>();

    private Vector3 startCamPosition;
    private Quaternion startCamRotation;
    private float cutscenenLerpDelta = 0;
    private float camMoveSpeed = 0.60f;

    [SerializeField]
    public GameObject blackMomentRoom;

    private void Start()
    {
        //pl = PlayerMovement.MyPlayer;
        camMain = CameraController.PlayerCamera;

        camOriginPos = cam.gameObject.transform.position;
        camOriginRot = cam.gameObject.transform.rotation;

        timeline.time = 0f;
    }

    void Update()
    {
        if (cutseneActive)
        {
            cam.transform.rotation = Quaternion.Slerp(startCamRotation, camOriginRot, cutscenenLerpDelta);
            cam.transform.position = Vector3.Lerp(startCamPosition, camOriginPos, cutscenenLerpDelta);

            cutscenenLerpDelta += Time.deltaTime * camMoveSpeed;

            if (Vector3.Distance(camOriginPos, cam.transform.position) < 0.1f)
            {
                timeline.Play();
                cutseneActive = false;
            }
        }

        if (camMove)
        {
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, camMain.transform.rotation, Time.deltaTime * 3f);
            cam.transform.position = Vector3.Lerp(cam.transform.position, camMain.transform.position, Time.deltaTime * 3f);

            if (Vector3.Distance(camMain.transform.position, cam.transform.position) < 0.1f)
            {
                cam.gameObject.SetActive(false);
                camMain.SwitchSensitivityBack();
                //blackMomentRoom.GetComponent<Collider>().enabled = false;
                for (int i = 0; i < PreviousToUnload.Count; i++)
                {
                    ManagerForScenes.Instance.Unload(PreviousToUnload[i]);
                }
                pl.airSettings.maxFallSpeed = -90f;
                pl.anim.SetBool("grounded", false);
                camMove = false;
                enterCutscene = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player") && (!enterCutscene))
        {
            enterCutscene = true;
            camMain.SwitchSensitivity();
            pl = other.gameObject.GetComponent<PlayerMovement>();
            StartCoroutine(pl.SwitchState(pl.cutsceneState));

            cam.transform.position = camMain.transform.position;
            cam.transform.rotation = camMain.transform.rotation;
            startCamPosition = cam.transform.position;
            startCamRotation = cam.transform.rotation;
            cam.gameObject.SetActive(true);
            cutseneActive = true;
        }
    }

    public void CutsceneEnd()
    {
        pl.gameObject.transform.position = cutsceneCh.gameObject.transform.position + Vector3.up * 1f;
        pl.gameObject.transform.rotation = cutsceneCh.gameObject.transform.rotation;

        camMain.SetPositionBehindPlayer();

        cutsceneCh.SetActive(false);
        cutseneActive = false;
    }

    public void CameraMove()
    {
        camMove = true;
    }

}
