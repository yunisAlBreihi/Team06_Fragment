using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    [FMODUnity.EventRef]
    [SerializeField] public string lvlOne;
    [FMODUnity.EventRef]
    [SerializeField] public string lvlTwo;
    [FMODUnity.EventRef]
    [SerializeField] public string lvlThree;

    void Start()
    {
        //FMODUnity.RuntimeManager.PlayOneShot(lvlOne);
    }
    
    public void PlayNext(string s)
    {
        FMODUnity.RuntimeManager.PlayOneShot(s);
    }
}
