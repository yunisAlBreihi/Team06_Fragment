using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using UnityEngine;

public class BeamMoveState : IMovement
{

     PlayerMovement p;
   
    public GameObject obj;
    public Transform handle;

    private Quaternion targetRot;
    
    private float rotationRate;
    private Vector3 traceHeightOffset;

    private bool reachedHandle = false;

    public BeamMoveState(PlayerMovement player)
    {
     p = player;
    }

    public void Start()
    {
        p.anim.SetBool("grab", true);
        if (handle == null || obj == null)
        {
            Debug.Log("No handle / no object to rotate. Check references.");
            p.StartCoroutine(p.SwitchState(p.groundMoveState));
        }
        Vector3 from = new Vector3(obj.transform.position.x, 0f, obj.transform.position.z);
        Vector3 to = new Vector3(handle.position.x, 0f, handle.position.z);
        targetRot = Quaternion.LookRotation(from - to, Vector3.up);
        traceHeightOffset = new Vector3(0f, 0.01f, 0f);
        p.StartCoroutine(FailSafe());
    }
    private IEnumerator FailSafe()
    {
        yield return new WaitForSeconds(2.5f);
        if (reachedHandle == false)
        { 
            p.StartCoroutine(p.SwitchState(p.groundMoveState));
        }
    }
    // Update is called once per frame
    public Vector3 UpdateState(Vector3 input, Vector3 vel)
    {
        if (reachedHandle == false)
        {
            return GetInPosition();
        }
        else
        {
            if(p.input.Buttons() == "Interact")
            {
                p.StartCoroutine(p.SwitchState(p.groundMoveState));
            }

            return RadialMovement(Input.GetAxis(p.input.hor));
        }
        
    }


    //Getting into position on the handle
    private Vector3 GetInPosition()
    {
        Vector3 cur= p.gameObject.transform.position;
        cur.y = 0;
        Vector3 tar = handle.position;
        tar.y = 0;

        Vector3 dir = (tar - cur);
        dir = Vector3.ClampMagnitude(dir, 1f);

        if (dir.magnitude < 0.5f)
        {
            dir = dir.normalized * 0.2f;
        }

        p.gameObject.transform.rotation = Quaternion.RotateTowards(p.gameObject.transform.rotation, targetRot, Time.deltaTime* 180f);


        if ((cur - tar).magnitude < 0.05f && Quaternion.Angle(p.gameObject.transform.rotation, targetRot) < 1f)
        {
            p.gameObject.transform.rotation = targetRot;
            p.gameObject.transform.position = new Vector3(tar.x, p.gameObject.transform.position.y, tar.z);

            reachedHandle = true;
            return Vector3.zero;
        }

        Vector3 move = p.groundMoveState.CalcMovement(dir, p.cc.velocity);
        move.y = p.groundMoveState.GravityAccel();

        return move;

    }

    private Vector3 RadialMovement(float hInput)
    {
        
        float rotLerp = (Mathf.Abs(hInput)< Mathf.Abs(rotationRate) ? 2f * Time.deltaTime : 0.3f * Time.deltaTime);
        rotationRate = Mathf.MoveTowards(rotationRate, hInput*0.7f, rotLerp);
        Quaternion deltaRot = Quaternion.AngleAxis(-rotationRate, Vector3.up);
      
        Quaternion rot =  deltaRot * obj.transform.rotation;
        obj.transform.rotation = rot;

        Vector3 newPos = handle.position;
        newPos.y = p.gameObject.transform.position.y;

        Vector3 look = obj.transform.position - p.gameObject.transform.position;
        look.y = 0f;

        p.gameObject.transform.rotation = Quaternion.LookRotation(look, Vector3.up);

        p.anim.SetFloat("turn", Input.GetAxis(p.input.hor));

        return (newPos - p.gameObject.transform.position)/Time.deltaTime; 

    }
    public void ExitState()
    {
        p.anim.SetBool("grab", false);

    }
}
