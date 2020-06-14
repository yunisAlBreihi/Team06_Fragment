using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundTrigger : MonoBehaviour
{
    [SerializeField] private GameObject SoundToCreate;

    private void OnEnable()
    {
        FadeVolume.OnHasFaded += DestroySound;
    }

    private void OnDisable()
    {
        FadeVolume.OnHasFaded -= DestroySound;
    }

    private void Awake()
    {
        SoundToCreate = Instantiate(SoundToCreate);
    }

    private void DestroySound()
    {
        Destroy(SoundToCreate);
    }
}
