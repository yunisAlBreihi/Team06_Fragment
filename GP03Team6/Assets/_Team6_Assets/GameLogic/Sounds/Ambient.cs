using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambient : MonoBehaviour
{
    private SoundSystem ss;

    private void Awake()
    {
        ss = GetComponent<SoundSystem>();
    }

    void Start()
    {
        ss.PlaySound(0);
    }
}
