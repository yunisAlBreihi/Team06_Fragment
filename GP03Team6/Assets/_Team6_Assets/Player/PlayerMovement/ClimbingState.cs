using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingState : IMovement
{
    private bool inPosition = false;
    private bool recieveInput = false;
    private bool climb = false;

    private float releaseCount;
    private bool runFailSafe;
    ClimbCheck climbCheck;
    PlayerMovement p;

    private Quaternion targetRot;

    private Vector3 climbTargetPos;
    private Vector3 targetPos;
    private Vector3 rHand, lHand;

    public ClimbingState(PlayerMovement player)
    {
        p = player;
        climbCheck = p.gameObject.GetComponent<ClimbCheck>();
    }

    public void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(climbCheck.settings.grabSound, p.gameObject);
        p.anim.SetBool("climbing", true);
        runFailSafe = true;
        releaseCount = 0f;
        targetRot = climbCheck.targetRot;
        climbTargetPos = climbCheck.climbUpTarget;
        recieveInput = false;
        climb = false;
        inPosition = false;
        targetPos = climbCheck.targetCenter;
        rHand = climbCheck.rHand;
        lHand = climbCheck.lHand;
        if (runFailSafe)
        {
            p.StartCoroutine(FailSafe(false));
        }
    }

    private IEnumerator FailSafe(bool climbingUp)
    {
        runFailSafe = false;
        yield return new WaitForSeconds(2f);
        runFailSafe = true;
        if (inPosition == false && climbingUp == false)
        {
            p.StartCoroutine(p.SwitchState(p.airState));
        }
        if(!((climbTargetPos - p.gameObject.transform.position).magnitude < 0.1f) && climbingUp == true)
        {

        }
    }
    
    public Vector3 UpdateState(Vector3 input, Vector3 vel)
    {
        if (inPosition == false)
        {
            return(LerpToTarget());
        }
        else 
        {
            if (recieveInput)
            {
                float forwardInput = (Vector3.Project(input, p.gameObject.transform.forward).normalized == p.gameObject.transform.forward) ? Vector3.Project(input, p.gameObject.transform.forward).magnitude : -Vector3.Project(input, p.gameObject.transform.forward).magnitude;
                if (forwardInput < -0.55f)
                {
                    releaseCount += Time.deltaTime;
                    if (releaseCount > 0.2f)
                    {
                        p.StartCoroutine(climbCheck.ClimbCD(0.2f));
                        p.StartCoroutine(p.SwitchState(p.airState));
                    }
                }
                else
                {
                    releaseCount = 0f;
                }
                if (/*forwardInput > 0.55f ||*/ p.input.Buttons() == "Jump")
                {
                    p.anim.SetTrigger("climbUp");
                    FMODUnity.RuntimeManager.PlayOneShotAttached(climbCheck.settings.climbSound, p.gameObject);
                    climb = true;
                    recieveInput = false;
                }
            }
            if (climb)
            {
                if(runFailSafe)
                {
                    p.StartCoroutine(FailSafe(true));
                }
                return ClimbUp();
            }
        }
        //Debug.Log("I'm in position");
        return Vector3.zero;
    }

    private Vector3 ClimbUp()
    {
        if ((climbTargetPos - p.gameObject.transform.position).magnitude < 0.1f)
        {
            p.StartCoroutine(p.SwitchState(p.airState));
            return Vector3.zero;
        }
        else
        {
            if (p.gameObject.transform.position.y < climbTargetPos.y)
            {
                return Vector3.up * climbCheck.settings.climbSpeed;
            }
            else
            {
                Vector3 dir = (climbTargetPos - p.gameObject.transform.position).normalized;
                return dir * climbCheck.settings.climbSpeed;
            }
        }
    }

    private Vector3 LerpToTarget()
    {
        if ((p.gameObject.transform.position - targetPos).magnitude < climbCheck.settings.entrySpeed * Time.deltaTime && p.transform.rotation == targetRot)
        {
            p.StartCoroutine(InputDelay());
            inPosition = true;
            return Vector3.zero;
        }

        p.transform.rotation = Quaternion.RotateTowards(p.gameObject.transform.rotation, targetRot, Time.deltaTime * 360f);

        Vector3 dir = (targetPos - p.gameObject.transform.position).normalized;
        return dir * climbCheck.settings.entrySpeed;
    }


    public IEnumerator InputDelay ()
    {
        yield return new WaitForSeconds(0.1f);
        recieveInput = true;
    }

    public void ExitState()
    {
        p.anim.SetBool("climbing", false);
     
    }
}
