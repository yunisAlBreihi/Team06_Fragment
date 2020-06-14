using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSound : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string selectSound;
    FMOD.Studio.EventInstance soundEvent;

    private void Awake()
    {
        soundEvent = FMODUnity.RuntimeManager.CreateInstance(selectSound);
    }

    private void Start()
    {
        soundEvent.start();
    }

    void Update()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundEvent, GetComponent<Transform>(), GetComponent<Rigidbody>());
    }
}
