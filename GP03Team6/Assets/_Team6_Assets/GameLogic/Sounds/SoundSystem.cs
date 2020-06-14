using UnityEngine;
using UnityEngine.Assertions;

public class SoundSystem : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string[] soundEvent;
    FMOD.Studio.EventInstance soundState;

    FMOD.Studio.Bus Master;
    float musicVolume = 0.5f;


    private void Awake()
    {
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
    }
    public void PlaySound(int i)
    {
        if (soundEvent[i] != null)
        {
            soundState = FMODUnity.RuntimeManager.CreateInstance(soundEvent[i]);
            soundState.start(); 
        }
    }

    private void Update()
    {
        Master.setVolume(musicVolume);
    }

    public void Volume(float volume)
    {
        musicVolume = volume;
    }
}