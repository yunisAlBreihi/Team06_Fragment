using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BoxMoveState : IMovement
{
    PlayerMovement p;
    public GameObject box;
    private Vector3 extents, boxUp;
    public Vector3 targetPos;
    public PushableObject pushScript;
    public Transform handle;
    [SerializeField] private float rotateSpeed;
    private bool reachedHandle = false;

    private float initGroundCheckDist;

    private float curSpeed;
    private float rotationRate = 0f;



    public BoxMoveState(PlayerMovement player)
    {
        p = player;
        initGroundCheckDist= p.groundCastDistance;
        
    }
    public void Start()
    {
        p.anim.SetBool("grab", true);
        p.groundCastDistance = 0.3f;
        curSpeed = 0f;
        reachedHandle = false;
        extents =  box.GetComponent<Collider>().bounds.extents * 0.9f;
        boxUp = box.transform.up;
        p.StartCoroutine(FailSafe());
        PushableObject pushScript = box.GetComponentInChildren<PushableObject>();
        if (pushScript != null)
        {
            float origAngle = pushScript.originalAngle;

            if (Mathf.Abs(Vector3.Angle(Vector3.up, box.transform.up) - origAngle) > 15f)
            {
                box.transform.rotation = pushScript.originalRot;
            }
        }
    }

   private IEnumerator FailSafe()
    {
        yield return new WaitForSeconds(2.5f);
        if(reachedHandle == false)
        {
            pushScript.Released();
            p.StartCoroutine(p.SwitchState(p.groundMoveState));
        }
    }
    public Vector3 UpdateState(Vector3 input, Vector3 vel)
    {
        if (p.ReturnGrounded() == false)
        {
            pushScript.Released();
            p.StartCoroutine(p.SwitchState(p.airState));
            return Vector3.zero;
        }

        if (reachedHandle == false)
        {
            return MoveToHandle();
        }

        BoxGravity();

        if (p.input.Buttons() == "Interact")
        {
            pushScript.Released();
            p.StartCoroutine(p.SwitchState(p.groundMoveState));
            return Vector3.zero;
        }

       

        if (Mathf.Abs(Input.GetAxis(p.input.ver)) > 0.7f)
        {
            return PushBox(input, vel);
        }

        else if(Mathf.Abs(Input.GetAxis(p.input.hor)) > 0.7f)
        {
            curSpeed = 0f;
            return TurnBox();
        }
        else
        {
            p.anim.SetFloat("push", 0f);
            p.anim.SetFloat("turn", 0f);
        }
        curSpeed = 0f;
        rotationRate = 0f;
        return Vector3.zero;
    }

    private void BoxGravity()
    {
        //RaycastHit hit;
        //Debug.DrawRay(box.transform.position, Vector3.down);
        //Vector3 v = Vector3.up * 0.1f;
        //Debug.Log(box.transform.position + v + "  extents " + extents+ "       " +  Vector3.down + "      " + "    rot   "  + box.transform.rotation);
        //if (Physics.BoxCast(box.transform.position+ v, extents , Vector3.down, out hit, box.transform.rotation, 0.5f))
        //{
        //  //  box.transform.position = new Vector3(box.transform.position.x,hit.point.y + extents.y, box.transform.position.z);

        //}
        //else
        //{
        //    Debug.Log("BOX ISNT GROUNDED!!");
        //    handle.gameObject.GetComponent<PushableObject>().Released();
        //    p.StartCoroutine(p.SwitchState(p.groundMoveState));
        //}
        if(pushScript.GroundCheck())
        {

        }
        else
        {
            Debug.Log("THE BOX ISNT GROUNDED");
            pushScript.Released();
            p.StartCoroutine(p.SwitchState(p.groundMoveState));
        }
    }

    private Vector3 MoveToHandle()
    {
        Vector3 dir = (targetPos - p.gameObject.transform.position);
        dir.y = 0f;
        dir = dir.normalized;

        Vector3 tar = targetPos;
        tar.y = p.gameObject.transform.position.y;

        Vector3 v1 = box.gameObject.transform.position;
        Vector3 v2 = targetPos;
        v2.y = v1.y = 0f;

        Quaternion targetRot = Quaternion.LookRotation(v1 - v2, Vector3.up);

        p.gameObject.transform.rotation = Quaternion.RotateTowards(p.gameObject.transform.rotation, targetRot, Time.deltaTime * 120f);

        if (Vector3.Distance(p.gameObject.transform.position, tar) < 0.05f && p.gameObject.transform.rotation==targetRot)
        {
            reachedHandle = true;
            p.transform.position = tar;
        }
        if (Vector3.Distance(p.gameObject.transform.position, tar) < 0.05f)
        {
            p.transform.position = tar;
            return Vector3.zero;
        }
            return dir * 5f;
    }

    private Vector3 PushBox(Vector3 input, Vector3 vel)
    {
        Vector3 dir = box.transform.position - handle.position;
        dir.y = 0f;
        dir = dir.normalized;

        float tarSpeed = Input.GetAxis(p.input.ver) * p.groundSettings.pushSpeed;

        curSpeed = Mathf.Lerp(curSpeed, tarSpeed, Time.deltaTime * p.groundSettings.pushAccel);
        curSpeed = Mathf.MoveTowards(curSpeed, tarSpeed, Time.deltaTime * p.groundSettings.pushAccel);

        Vector3 move = dir * curSpeed;

        if(curSpeed<0f)
        {
            if(Physics.BoxCast(p.gameObject.transform.position, p.gameObject.GetComponent<CapsuleCollider>().bounds.extents, move, box.transform.rotation, Mathf.Abs(curSpeed * Time.deltaTime), p.boxBlockers))
            {
                return Vector3.zero;
            }
        }

        if (Physics.BoxCast(box.transform.position, extents * 0.99f, move, box.transform.rotation, Mathf.Abs(curSpeed * Time.deltaTime), p.boxBlockers) == false)
        {
            box.transform.position += move * Time.deltaTime;
            move.y = p.groundMoveState.GravityAccel();

            p.anim.SetFloat("push", Input.GetAxis(p.input.ver));

            return move;
        }
        else 
        {
            Debug.Log("THe Box Is Stuck!!");
            return Vector3.zero;
        }
    }

    private Vector3 TurnBox()
    {       
            float hInput = Input.GetAxis(p.input.hor);

            float rotLerp = 1f;
            rotationRate = Mathf.MoveTowards(rotationRate, p.groundSettings.boxTurnSpeed * hInput, rotLerp);
            Quaternion deltaRot = Quaternion.AngleAxis(-rotationRate, Vector3.up);
            
            Quaternion rot = deltaRot * box.transform.rotation;

        Vector3 pushDir = (box.transform.position - p.gameObject.transform.position).normalized;
        pushDir.y = 0f;

        if (Physics.BoxCast(box.transform.position, extents, pushDir, box.transform.rotation, 0.15f, p.boxBlockers) == false)
        {
            box.transform.rotation = rot;

            Vector3 newPos = handle.position;
            newPos.y = p.gameObject.transform.position.y;

            Vector3 look = box.transform.position - p.gameObject.transform.position;
            look.y = 0f;

            p.gameObject.transform.rotation = Quaternion.LookRotation(look, Vector3.up);

            p.anim.SetFloat("turn", Input.GetAxis(p.input.hor));

            return (newPos - p.gameObject.transform.position) / Time.deltaTime;
        }
        else
        {
            return Vector3.zero;
        }
            
            
        //Vector3 inputProjected = Vector3.Project(input, box.transform.right).normalized;
        //Vector3 rotTarget = inputProjected * -1f;
        //Vector3 curRot = Vector3.RotateTowards(box.transform.forward, rotTarget, Time.deltaTime * rotateSpeed, 0f);
        //box.transform.rotation = Quaternion.LookRotation(curRot, boxUp);
            
        //return Vector3.zero;
    }

    public void ExitState()
    {
        p.groundCastDistance = initGroundCheckDist;
        p.anim.SetBool("grab", false);
    }

}