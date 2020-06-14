using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ojnar_FMODPlayer : MonoBehaviour
{
    void PlayFootstepsEvent(string path)
    {
        FMOD.Studio.EventInstance footstep = FMODUnity.RuntimeManager.CreateInstance(path);
        FMODUnity.RuntimeManager.PlayOneShot(path, transform.position);
        footstep.release();
    }

    void PlayVoiceEvent(string path)
    {

    } 
}
