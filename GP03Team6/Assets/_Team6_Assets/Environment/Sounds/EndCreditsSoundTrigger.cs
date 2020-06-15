using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCreditsSoundTrigger : MonoBehaviour
{
    [SerializeField] private GameObject SoundToCreate;

    private void OnEnable()
    {
        FadeVolume.OnHasFaded += DestroySound;
        SoundToCreate = Instantiate(SoundToCreate);
    }

    private void OnDisable()
    {
        FadeVolume.OnHasFaded -= DestroySound;
    }

    private void DestroySound()
    {
        Destroy(SoundToCreate);
    }
}
