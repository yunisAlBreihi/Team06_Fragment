using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerPuzzle : MonoBehaviour
{
    List<LazerDirection> Directions = new List<LazerDirection>();
    LazerDirection CurrentTarget = null;
    LineRenderer MyLineRender;
    public LayerMask lazerMask;
    RaycastHit myRayhit;
    public bool isRecieving = false; //send beam
    public bool canRecieve = true; //check if can recieve beam
    private int lazerIndexFrom, lazerIndexTo;

    [FMODUnity.EventRef]
    [SerializeField] public string endSound;
    [FMODUnity.EventRef]
    [SerializeField] public string rerouteSound;
    private bool shouldPlay = true;

    [Header("EndOfPuzzle")]

    public bool theEnd;
    public List<Material> mat = new List<Material>();
    [SerializeField]private Shader shader;
    bool ReachedMyGoal = false;

    void Start()
    {
        foreach (MeshRenderer item in GetComponentsInChildren<MeshRenderer>())
        {
            if (item.material.shader == shader)
            {
                mat.Add(item.material);
            }
        }

        canRecieve = true;
        isRecieving = false;
        MyLineRender = GetComponent<LineRenderer>();
        ReachedMyGoal = false;
        if (MyLineRender.positionCount != 2) //Making sure there are a start and an end position 
        {
            MyLineRender.positionCount = 2;
        }

        foreach (LazerDirection lazer in transform.GetComponentsInChildren<LazerDirection>()) //get all children with lazerDirection
        {
            Directions.Add(lazer);
            lazer.lazerRecieve = false;
            lazer.lazerSend = false;
        }

    }

    void Update()
    {
        if (isRecieving)
        {
            if (theEnd)
            {
                foreach(Material item in mat)
                {
                    item.SetFloat("_ActiveAmount", 1f);
                }
                if(ReachedMyGoal == false)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached(endSound, this.gameObject);
                }
                ReachedMyGoal = true;
                return; //temp solution
            }
            else
            {
                if(shouldPlay == true)
                {
                    FMODUnity.RuntimeManager.PlayOneShotAttached(rerouteSound, this.gameObject);
                    shouldPlay = false;
                }
            }
            //MyOutput.Update(Directions[lazerIndexFrom], Directions[lazerIndexTo],MyLineRender);
            MyLineRender.SetPosition(0, Directions[lazerIndexTo].transform.position);
            MyLineRender.SetPosition(1, GetEndPosition(Directions[lazerIndexTo]));
        }
        if (canRecieve)
        {
            if (theEnd)
            {

            }
            else
            {
                shouldPlay = true;
            }
            if (CurrentTarget)
            {
                CurrentTarget.LazerDisconnected();
                CurrentTarget.transform.parent.GetComponent<LazerPuzzle>().canRecieve = true;
                CurrentTarget.transform.parent.GetComponent<LazerPuzzle>().isRecieving = false;
                Directions[lazerIndexTo].lazerSend = false;
                CurrentTarget = null;
            }
            for (int i = 0; i < Directions.Count; i++)
            {
                if (Directions[i].lazerRecieve)
                {
                    isRecieving = true;
                    lazerIndexFrom = i;
                    canRecieve = false;
                    if (!theEnd)
                    {
                        lazerIndexTo = GetLazerIndexTo(lazerIndexFrom);
                        Directions[lazerIndexTo].lazerSend = true;
                    }
                }
                foreach (Material item in mat)
                {
                    item.SetFloat("_ActiveAmount", 0f);
                }
                ReachedMyGoal = false;


            }
            MyLineRender.SetPosition(0, Directions[lazerIndexTo].transform.position);   //Find BETTER solution for this please!!!
            MyLineRender.SetPosition(1, Directions[lazerIndexTo].transform.position);   //Find BETTER solution for this please!!!

        }
    }
    int GetLazerIndexTo(int indexFrom)
    {
        float tempAngle = 0;
        int tempIndex = 0;
        for (int i = 0; i < Directions.Count; i++)
        {
            if (i != indexFrom && !Directions[i].lazerRecieve)
            {
                float tempA = Vector3.Angle(Directions[indexFrom].LazerFrom, Directions[indexFrom].transform.forward);
                float tempB = Vector3.Angle(Directions[indexFrom].LazerFrom, Directions[i].transform.position);
                tempAngle = tempB - tempA;
                tempIndex = i;
            }
        }
        for (int i = 0; i < Directions.Count; i++)
        {

            if (Directions[indexFrom] != Directions[i] && !Directions[i].lazerRecieve) //No need to check if it already have been checked or if it is the same
            {
                float tempA = Vector3.Angle(Directions[indexFrom].LazerFrom, Directions[indexFrom].transform.forward);
                float tempB = Vector3.Angle(Directions[indexFrom].LazerFrom, Directions[i].transform.position);
                float tempC = tempB - tempA;
                if (tempC > tempAngle)
                {
                    tempAngle = tempC;
                    tempIndex = i;
                }
            }
        }
        return tempIndex;

    }
    Vector3 GetEndPosition(LazerDirection lazerDirectionStart)
    {
        if (Physics.BoxCast(
            lazerDirectionStart.transform.position,
            lazerDirectionStart.transform.localScale,
            lazerDirectionStart.transform.forward,
            out myRayhit,
            Quaternion.identity,
            300,
            lazerMask)

            )
        {
            if (myRayhit.transform.GetComponent<LazerDirection>())
            {
                if (!myRayhit.transform.GetComponent<LazerDirection>().lazerSend || !myRayhit.transform.GetComponent<LazerDirection>().lazerRecieve)
                {
                    for (int i = 0; i < myRayhit.transform.GetComponent<LazerDirection>().transform.parent.GetComponent<LazerPuzzle>().Directions.Count; i++)
                    {
                        if (myRayhit.transform.GetComponent<LazerDirection>().transform.parent.GetComponent<LazerPuzzle>().Directions[i].lazerRecieve)
                            return myRayhit.point;
                    }
                    if (CurrentTarget)
                    {

                        CurrentTarget.LazerDisconnected();
                        CurrentTarget.transform.parent.GetComponent<LazerPuzzle>().canRecieve = true;
                        CurrentTarget.transform.parent.GetComponent<LazerPuzzle>().isRecieving = false;
                        ReachedMyGoal = false;
                        Directions[lazerIndexTo].lazerSend = false;

                        CurrentTarget = myRayhit.transform.GetComponent<LazerDirection>();
                        //CurrentTarget.LazerFrom = transform.position;
                        CurrentTarget.LazerFrom = transform.position;
                        CurrentTarget.LazerConnected();
                    }
                    else if (!CurrentTarget && myRayhit.transform.GetComponent<LazerDirection>())
                    {
                        CurrentTarget = myRayhit.transform.GetComponent<LazerDirection>();
                        //CurrentTarget.LazerFrom = transform.position;
                        CurrentTarget.LazerFrom = transform.position;
                        CurrentTarget.LazerConnected();
                    }
                }
            }
            else if (!myRayhit.transform.GetComponent<LazerDirection>())
            {
                if (CurrentTarget)
                {
                    CurrentTarget.LazerDisconnected();
                    CurrentTarget.transform.parent.GetComponent<LazerPuzzle>().canRecieve = true;
                    CurrentTarget.transform.parent.GetComponent<LazerPuzzle>().isRecieving = false;
                    ReachedMyGoal = false;
                    CurrentTarget = null;
                }
               // return lazerDirectionStart.transform.forward * 300 + lazerDirectionStart.transform.position;
            }
            else
            {
                return lazerDirectionStart.transform.forward * 300 + lazerDirectionStart.transform.position;
            }
            return myRayhit.point;
        }
        else
        {
            if (CurrentTarget)
            {
                CurrentTarget.LazerDisconnected();
                CurrentTarget.transform.parent.GetComponent<LazerPuzzle>().canRecieve = true;
                CurrentTarget.transform.parent.GetComponent<LazerPuzzle>().isRecieving = false;
                ReachedMyGoal = false;
                CurrentTarget = null;
            }
            return lazerDirectionStart.transform.forward * 300 + lazerDirectionStart.transform.position;
        }
    }

    public bool ReachedGoal()
    {
        return ReachedMyGoal;
    }
}
