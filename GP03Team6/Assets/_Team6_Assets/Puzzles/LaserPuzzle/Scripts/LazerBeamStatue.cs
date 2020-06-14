using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LazerBeamStatue : MonoBehaviour
{
    LineRenderer myLineRender;
    public LayerMask lazerMask;
    RaycastHit myRayhit;
    public bool shouldShoot = true; //if it should shoot or not
    LazerDirection CurrentTarget = null;

    public void Start()
    {
        myLineRender = GetComponent<LineRenderer>();
        if (myLineRender.positionCount < 2)
        {
            myLineRender.positionCount = 2;
        }

    }


    public void Update()
    {
        if (shouldShoot)
        {
            myLineRender.SetPosition(0, transform.position);
            myLineRender.SetPosition(1, GetEndPosition(this.transform));
        }
    }
    Vector3 GetEndPosition(Transform lazerDirectionStart)
    {
        if (Physics.BoxCast(
            lazerDirectionStart.transform.position,
            lazerDirectionStart.transform.localScale * 0.5f,
            lazerDirectionStart.transform.forward,
            out myRayhit,
            Quaternion.LookRotation(lazerDirectionStart.transform.forward, Vector3.up),
            300,
            lazerMask)
            )
        {
            if (
                CurrentTarget && //If I have a reciever already
                myRayhit.transform.GetComponent<LazerDirection>() && //and I hit something with <LazerDirection>
                CurrentTarget != myRayhit.transform.GetComponent<LazerDirection>() && //And it is not the same as my target
                !myRayhit.transform.GetComponent<LazerDirection>().lazerRecieve //And my new target doesn't already recieve
                )
            {
                CurrentTarget.LazerDisconnected(); //then disconnect my current target
                CurrentTarget.transform.parent.GetComponent<LazerPuzzle>().canRecieve = true; //then set my current target available to recieve again
                CurrentTarget.transform.parent.GetComponent<LazerPuzzle>().isRecieving = false; //then tell it that it is no longer recieving

                CurrentTarget = myRayhit.transform.GetComponent<LazerDirection>(); //set my new target to currenttarget
                CurrentTarget.LazerFrom = transform.position; //Tell it where the lazer is comming froms
                                                              // CurrentTarget.LazerFrom = transform.forward;
                CurrentTarget.LazerConnected(); //tell it that it is connected to something
            }
            else if (
                !CurrentTarget &&
                myRayhit.transform.GetComponent<LazerDirection>() &&
                !myRayhit.transform.GetComponent<LazerDirection>().lazerRecieve)
            {
                CurrentTarget = myRayhit.transform.GetComponent<LazerDirection>();
                CurrentTarget.LazerFrom = transform.position;
                // CurrentTarget.LazerFrom = transform.forward;
                CurrentTarget.LazerConnected();
            }
            else if (!myRayhit.transform.GetComponent<LazerDirection>())
            {
                if (CurrentTarget) //if I have a target and is no longer hitting anything
                {
                    CurrentTarget.LazerDisconnected();
                    CurrentTarget.transform.parent.GetComponent<LazerPuzzle>().canRecieve = true;
                    CurrentTarget.transform.parent.GetComponent<LazerPuzzle>().isRecieving = false;
                    CurrentTarget = null;
                }
              //  return lazerDirectionStart.transform.forward * 300 + lazerDirectionStart.transform.position;
            }
            return myRayhit.point;
        }
        else
        {
            if (CurrentTarget) //if I have a target and is no longer hitting anything
            {
                CurrentTarget.LazerDisconnected();
                CurrentTarget.transform.parent.GetComponent<LazerPuzzle>().canRecieve = true;
                CurrentTarget.transform.parent.GetComponent<LazerPuzzle>().isRecieving = false;
                CurrentTarget = null;
            }
            return lazerDirectionStart.transform.forward * 300 + lazerDirectionStart.transform.position;
        }
    }
}
