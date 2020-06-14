using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WakeUpFadePP : MonoBehaviour
{
    Volume volume;

    private float lerpTime = 0;

    private void Awake()
    {
        volume = GetComponent<Volume>();
    }

    private void Update()
    {
        volume.weight = Mathf.Lerp(1,0, lerpTime);
        lerpTime += Time.deltaTime * 0.2f;
    }
}
