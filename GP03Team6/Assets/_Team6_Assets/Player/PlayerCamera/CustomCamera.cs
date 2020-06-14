using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cinemachine;

public class CustomCamera : MonoBehaviour
{
    private Camera cam;
    private CameraController camScript;
    private CinemachineBrain cm;
    private GameObject player;

    [SerializeField] private CinemachinePathBase path;
    [SerializeField] private CinemachineVirtualCamera vc;
    [SerializeField] private CinemachineTrackedDolly dolly;

    private Vector3 target, startPos;
    [SerializeField] private Vector3 lookAtOffset;
    private Transform lookAt;

    [SerializeField] private float camLerpSpeed, camRotSpeed = 3f, yLook = 0.5f;

    private float startDist, currentDist;

    private bool active = false;

    private void Start()
    {
        cam = Camera.main;
        camScript = cam.GetComponent<CameraController>();
        dolly = vc.GetCinemachineComponent<CinemachineTrackedDolly>();
        player = PlayerMovement.MyPlayer.gameObject;
        cm = cam.GetComponent<CinemachineBrain>();
        lookAt = camScript.playerFocusPoint;
        vc.LookAt = lookAt;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lookAt.position += lookAtOffset;
            vc.m_Priority = 100;
            camScript.enabled = false;
            startDist = path.FindClosestPoint(player.transform.position, 0, -1, 10);
            dolly.m_PathPosition = startDist;
            active = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lookAt.position -= lookAtOffset;
            vc.m_Priority = 10;
            SetCameraRotation();
            camScript.enabled = true;
            active = false;
            StartCoroutine(SmoothOut());
        }
    }

    private void Update()
    {
        if (active)
        {
            currentDist = path.FindClosestPoint(player.transform.position, 0, -1, 30);
            //dolly.m_PathPosition = currentDist;
            cam.transform.position = Vector3.Lerp(cam.transform.position, path.EvaluatePosition(currentDist), Time.deltaTime * camLerpSpeed);
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, path.EvaluatePosition(currentDist), Time.deltaTime * camLerpSpeed);
            Vector3 look = lookAt.position - cam.transform.position;
            look.y = 0f;
            look = look.normalized;
            look.y = yLook;
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.LookRotation(look, Vector3.up), Time.deltaTime * camRotSpeed);
            cam.transform.rotation = Quaternion.RotateTowards(cam.transform.rotation, Quaternion.LookRotation(look, Vector3.up), Time.deltaTime * camRotSpeed);
        }
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

    private IEnumerator SmoothOut()
    {
        camScript.smoothOut = true;
        yield return new WaitForSeconds(1.5f);
        camScript.smoothOut = false;
    }
}
