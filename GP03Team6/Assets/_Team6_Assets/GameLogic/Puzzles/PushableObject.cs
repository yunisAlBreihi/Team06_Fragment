using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PushableObject : InteractibleObject
{

    //Put this script on a handle, attached to the object to move

    [SerializeField] private PlayerMovement p;

    private Vector3 respawnPos;
    private Quaternion respawnRot;

    private Vector3 extents;
    private float groundDist = 1.2f;
    private RaycastHit hit;

    private float yVelocity;

    private Rigidbody rb;

    public Transform[] handles;

    public float  originalAngle;

    public Quaternion originalRot;

    [SerializeField] private bool onGround = false, useGrav = true;

    [SerializeField] private GameObject pushable;

    private void Awake()
    {
        rb = pushable.GetComponent<Rigidbody>();
        
        extents = pushable.GetComponent<Collider>().bounds.extents;

        if(p == null)
        {
            p = ManagerForScenes.playerMovement;//GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        }
        if(pushable == null)
        {
            pushable = transform.root.gameObject;
        }
        respawnPos = transform.position;
        respawnRot = transform.rotation;

        originalRot = pushable.transform.rotation;

        originalAngle = Vector3.Angle(pushable.transform.up, Vector3.up);
    }

    public override void InteractedWith(GameObject other)
    {
        p = other.GetComponent<PlayerMovement>();
        Transform handle = FindHandles();
        useGrav = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        canInteract = false;
        p.boxMoveState.pushScript = this;
        p.boxMoveState.box = pushable;
        p.boxMoveState.handle = handle;
        p.boxMoveState.targetPos = handle.position;
        p.StartCoroutine(p.SwitchState(p.boxMoveState));
    }

    public Transform FindHandles()
    {
        Transform t = this.gameObject.transform;
        float l = 100f;
        foreach (Transform item in handles)
        {
            float d = Vector3.Distance(item.position, p.gameObject.transform.position);
            if (d < l)
            {
                l = d;
                t = item;
            }
        }
        return t;
    }

    public void Respawn()
    {
        transform.rotation = respawnRot;
        transform.position = respawnPos;
    }
    public void Released()
    {
        useGrav = true;
        rb.isKinematic = false;
        rb.useGravity = true;
        canInteract = true;
    }

    private void Update()
    {
        if (useGrav)
        {
          //  GravityAccel();
        }
       
    }

    private void GravityAccel()
    {
        yVelocity -= 8f * Time.deltaTime;

        bool b = Physics.BoxCast(pushable.transform.position, extents, Vector3.down, out hit, pushable.transform.rotation, Mathf.Abs(yVelocity * Time.deltaTime));

        Debug.Log("Am I hitting something?: " + b + "   Extents: " + extents + "   yVel: " + yVelocity + "   pos : " + pushable.transform.position);
        if (!b)
        {
            pushable.transform.position += new Vector3(0f, yVelocity * Time.deltaTime, 0f);
        }
        else if (b)
        {
            pushable.transform.position = new Vector3(pushable.transform.position.x, hit.point.y + extents.y + 0.01f, pushable.transform.position.z);
            yVelocity = 0f;
        }
       
    }

    public bool GroundCheck()
    {
        if (Physics.BoxCast(pushable.transform.position + Vector3.up, extents, Vector3.down, out hit ,pushable.transform.rotation, groundDist))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
