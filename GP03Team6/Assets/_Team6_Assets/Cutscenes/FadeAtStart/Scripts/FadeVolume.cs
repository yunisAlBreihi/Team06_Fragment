using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class FadeVolume : MonoBehaviour
{
    public delegate void HasFaded();
    public static event HasFaded OnHasFaded;

    enum FadeType 
    {
        IN,
        OUT,
        OUTIN,
    }

    [SerializeField] Image fadeSprite;
    [SerializeField] float timeBeforeFade = 2.0f;
    [SerializeField] float fadeTime = 3.0f;
    [SerializeField] FadeType fadeType;
    [SerializeField, Tooltip("Whether to lock the camera while fading or not")] bool lockCamera = true;
    [SerializeField] bool isEndCutscene = false;

    private Volume volume;

    private bool isFading = false;
    private bool isTriggered = false;
    private float fadeSmoothness;

    private bool hasSentBroadcast = false;

    private CameraController camController;

    private void Awake()
    {
        volume = GetComponent<Volume>();
        fadeSmoothness = 1.0f / 30.0f;
    }
    private void Start()
    {
        camController = Camera.main.GetComponent<CameraController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered == false && other.CompareTag("Player"))
        {
            isTriggered = true;
            fadeSprite.enabled = true;
            StartCoroutine(WaitBeforeFade(timeBeforeFade,fadeType));
        }
    }

    private IEnumerator WaitBeforeFade(float time, FadeType fadeType) 
    {
        yield return new WaitForSeconds(time);

        StartCoroutine(Fade(fadeType));
    }

    private IEnumerator Fade(FadeType fadeType)
    {
        isFading = true;
        float fadeDelta = 0;
        float fadeProgress = 0;
        Color tempColor;
        if (fadeType == FadeType.IN)
        {
            tempColor = new Color(0, 0, 0, 1);
        }
        else
        {
            tempColor = new Color(0, 0, 0, 0);
            if (lockCamera == true)
            {
                camController.SwitchSensitivity();
            }
        }

        while (isFading)
        {
            fadeSprite.color = tempColor;
            if (fadeType == FadeType.IN)
            {
                fadeProgress = Mathf.Lerp(1.0f, 0.0f, fadeDelta);
            }
            else
            {
                fadeProgress = Mathf.Lerp(0.0f, 1.0f, fadeDelta);
            }

            volume.weight = fadeProgress;
            tempColor.a = fadeProgress;

            fadeDelta += 1 / fadeTime * fadeSmoothness;

            if (fadeDelta >= 1.0f + (1 / fadeTime * fadeSmoothness) * 2.0f)
            {
                isFading = false;
            }

            yield return new WaitForSeconds(fadeSmoothness);
        }

        if (fadeType == FadeType.IN)
        {
            camController.SwitchSensitivityBack();
        }

        if (fadeType == FadeType.OUTIN)
        {
            if (hasSentBroadcast == false)
            {
                hasSentBroadcast = true;
                OnHasFaded();
                StartCoroutine(WaitBeforeFade(timeBeforeFade, FadeType.IN));
            }
        }
        else if (fadeType == FadeType.OUT)
        {
            if (hasSentBroadcast == false)
            {
                hasSentBroadcast = true;
                OnHasFaded();
                StartCoroutine(WaitBeforeDisabling());
            }
        }
    }

    IEnumerator WaitBeforeDisabling() 
    {
        yield return new WaitForSeconds(0.8f);
        gameObject.SetActive(false);
        if (isEndCutscene == false)
        {
            fadeSprite.enabled = false;

        }
    }
}
