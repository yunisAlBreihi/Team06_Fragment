using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerDirection : MonoBehaviour
{

    public bool lazerRecieve = false; //recive beam
    public bool lazerSend = false;
    public Vector3 LazerFrom = Vector3.zero; //Where is the lazer befor this?

    public void LazerConnected()
    {
        lazerRecieve = true;
    }
    public void LazerDisconnected()
    {
        lazerRecieve = false;
    }
}
