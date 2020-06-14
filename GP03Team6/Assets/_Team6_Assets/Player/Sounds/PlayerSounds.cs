using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [FMODUnity.EventRef]
    [SerializeField]public string selectSound;
    FMOD.Studio.EventInstance soundEvent;

    private PlayerMovement p;

    private void Start()
    {
        p = GetComponent<PlayerMovement>();
        soundEvent = FMODUnity.RuntimeManager.CreateInstance(selectSound);
        soundEvent.start();
    }

    public void Step()
    {
        if (p.ReturnGrounded())
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(selectSound, p.gameObject);
        }
    }
}
