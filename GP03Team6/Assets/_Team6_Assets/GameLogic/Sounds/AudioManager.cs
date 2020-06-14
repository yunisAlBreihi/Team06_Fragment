using UnityEngine;

public class AudioManager : MonoBehaviour
{
    FMOD.Studio.Bus Master;
    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;
    FMOD.Studio.Bus VoiceLine;
    float masterVolume = 1f;
    float musicVolume = 1f;
    float sfxVolume = 1f;
    float voiceVolume = 1f;

    private void Awake()
    {
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Master/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/Master/SFX");
        VoiceLine = FMODUnity.RuntimeManager.GetBus("bus:/Master/VoiceLines");
    }

    private void Update()
    {
        Master.setVolume(masterVolume);
        Music.setVolume(musicVolume);
        SFX.setVolume(sfxVolume);
        VoiceLine.setVolume(voiceVolume);
    }

    public void MasterVolumeLevel(float newVolumeLevel)
    {
        masterVolume = newVolumeLevel;
    }
    public void MusicVolumeLevel(float newVolumeLevel)
    {
        musicVolume = newVolumeLevel;
    }
    public void SFXVolumeLevel(float newVolumeLevel)
    {
        sfxVolume = newVolumeLevel;
    }
    public void VoiceVolumeLevel(float newVolumeLevel)
    {
        voiceVolume = newVolumeLevel;
    }
}
