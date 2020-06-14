using System.Net;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string musicToPlay;
    FMOD.Studio.EventInstance Audio;


    //private SoundSystem soundSystem;

    private void Awake()
    {
        Audio = FMODUnity.RuntimeManager.CreateInstance(musicToPlay);
        //soundSystem.PlaySound(0);
    }
    private void Start()
    {
        Audio.start();
    }
}

