using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cinemachine;

public class ManualCutsceneNoPlayerMovement : MonoBehaviour
{
    private Camera cam;
    private CameraController camScript;
    private CinemachineBrain cm;
    private GameObject player;
    [SerializeField] private PlayerMovement p;

    [SerializeField] private CinemachinePathBase path;
    [SerializeField] private CinemachineVirtualCamera vc;
    [SerializeField] private CinemachineTrackedDolly dolly;

   [SerializeField] private Vector3 target, startPos;
    [SerializeField] private Vector3 lookAtOffset;
    [SerializeField] private Transform lookAt;

    [SerializeField] private float camLerpSpeed, camRotSpeed = 3f, yLook = 0.5f, t = 0f, speedMultiplier = 1f;

    private float s = 0f;
    public float length = 10f;
    private float startDist, currentDist;

    private bool inBeginPosition = false;

    private bool active = false, canEnter = true;

    [SerializeField]private Vector3 move, start;

    private void Start()
    {
        cam = Camera.main;
        camScript = cam.GetComponent<CameraController>();
        dolly = vc.GetCinemachineComponent<CinemachineTrackedDolly>();
        player = PlayerMovement.MyPlayer.gameObject;
        cm = cam.GetComponent<CinemachineBrain>();
        vc.LookAt = lookAt;
        p = player.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canEnter)
        {
            canEnter = false;
            camScript.enabled = false;
            vc.m_Priority = 100;
            startPos = path.EvaluatePosition(0f);

            cam.transform.position = startPos;
            cm.enabled = true;
            active = true;
        }
    }

  

    private IEnumerator SmoothOut()
    {
        cm.enabled = false;
        SetCameraRotation();
        camScript.smoothOut = true;

        yield return new WaitForSeconds(1.5f);
        camScript.smoothOut = false;
    }

    private void SetCameraRotation()
    {

        float yaw = Vector3.Angle(Vector3.forward, Vector3.ProjectOnPlane(cam.transform.forward, Vector3.right));

        if (cam.transform.forward.x < 0f)
        {
            yaw *= -1f;
        }

        camScript.orbitAngles = new Vector2(20f, yaw);
        camScript.focusPoint = cam.transform.position;
    }

    private void Update()
    {
        if(active == true)
        {
            s = Mathf.Lerp(s, speedMultiplier, Time.deltaTime);

            t += Time.deltaTime * s;

            dolly.m_PathPosition = t;

            if(t>length* speedMultiplier)
            {
                EndCutscene();
            }
        }
    }

    private void EndCutscene()
    {
        cm.enabled = false;
        camScript.enabled = true;
        active = false;
        vc.m_Priority = 10;
        StartCoroutine(WaitThenDestroy());
        StartCoroutine(SmoothOut());
    }

    private IEnumerator WaitThenDestroy()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

}
