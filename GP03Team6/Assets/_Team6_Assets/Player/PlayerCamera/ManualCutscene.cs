using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cinemachine;

public class ManualCutscene : MonoBehaviour
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

    public Transform moveTarget, startTarget;
    [SerializeField]private Vector3 move, start;

    private void Start()
    {
        start = startTarget.position;
        cam = Camera.main;
        camScript = cam.GetComponent<CameraController>();
        dolly = vc.GetCinemachineComponent<CinemachineTrackedDolly>();
        player = PlayerMovement.MyPlayer.gameObject;
        cm = cam.GetComponent<CinemachineBrain>();
        vc.LookAt = lookAt;
        p = player.GetComponent<PlayerMovement>();
        move = moveTarget.position;
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && canEnter)
        {
            canEnter = false;
            camScript.enabled = false;
            vc.m_Priority = 100;
            startPos = path.EvaluatePosition(0f);
            StartCoroutine(p.SwitchState(p.cutsceneState));
            StartCoroutine(SmoothIn());
        }
    }

    private IEnumerator SmoothIn()
    {
        while (Vector3.Distance(cam.transform.position, startPos) > 0.03f  || Quaternion.Angle(Quaternion.LookRotation((lookAt.position - cam.transform.position), Vector3.up), cam.transform.rotation)> 1f || !inBeginPosition)
        {
            inBeginPosition = MoveToTarget(start);

            cam.transform.position = Vector3.Lerp(cam.transform.position, startPos, Time.deltaTime);
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, startPos, Time.deltaTime* 2f);
            cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, Quaternion.LookRotation((lookAt.position - cam.transform.position), Vector3.up), Time.deltaTime * 70f);
            yield return new WaitForEndOfFrame();
        }
        cm.enabled = true;
        active = true;
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

    private bool MoveToTarget(Vector3 target)
    {
        Vector3 cur = player.transform.position;
        cur.y = 0f;
        Vector3 tar = target;
        tar.y = 0f;
        if (Vector3.Distance(cur, tar) > 0.5f)
        {
            Vector3 v = (tar - cur);
            Debug.Log("Adding to move   " + v.normalized * p.groundSettings.speed);
            p.addMoveVector = v.normalized * p.groundSettings.speed;
            p.addToMove = true;
            player.transform.rotation = Quaternion.LookRotation(v, Vector3.up);
            return false;
        }
        else
        {
            return true;
        }
    }

    private void Update()
    {
        if(active == true)
        {
            Debug.Log("SHOULD BE MOVING");

            MoveToTarget(move);

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
        StartCoroutine(p.SwitchState(p.groundMoveState));
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
