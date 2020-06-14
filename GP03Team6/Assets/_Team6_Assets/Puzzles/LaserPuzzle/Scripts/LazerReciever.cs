using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerReciever : MonoBehaviour
{
    [SerializeField] private bool isRecieving = false;
    public bool canRecieve = true;
    void Start()
    {
        canRecieve = true;
    }
    public bool RecievingLazer()
    {
        return isRecieving;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
