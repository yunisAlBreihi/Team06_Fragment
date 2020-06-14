using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFade : MonoBehaviour
{
    [SerializeField] GameObject fadeCanvas;
    [SerializeField] bool fadeIsActive = true;

    private void Awake()
    {
        if (fadeCanvas == null)
        {
            fadeCanvas = GameObject.FindGameObjectWithTag("FadeCanvas");
        }

        if (fadeCanvas != null && fadeIsActive)
        {
            fadeCanvas.SetActive(true);
        }
    }
}
