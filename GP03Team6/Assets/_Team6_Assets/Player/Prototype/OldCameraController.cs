using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OldCameraController : MonoBehaviour
{
    private Camera cam;

    public Transform target;    //Cameras LookAt Target
    public Transform player;     


    public Vector3 offset = new Vector3(0f, 4f, -6f); //Offset from player
    private Vector3 pos;

    private void Start()
    {
        cam = GetComponent<Camera>();
        pos = offset;
    }

    private void LateUpdate()
    {
        UpdatePosition(player.GetComponent<InputManager>().CameraInput());
        UpdateRotation();
    }

    public void UpdatePosition(Vector2 input)    
    {
        pos = Quaternion.AngleAxis(input.x, Vector3.up) * pos;
        transform.position = pos + player.position;
        UpdateRotation();
    }

    public void UpdateRotation()
    {
        transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);

    }



}
