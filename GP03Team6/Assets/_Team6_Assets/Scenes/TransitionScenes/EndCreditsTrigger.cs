using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndCreditsTrigger : MonoBehaviour
{
    [SerializeField,Tooltip("The image used for credits")] private Image creditsImage;
    [SerializeField, Tooltip("Zap site Text component")] private Text zapText;
    [SerializeField, Range(0.01f, 20.0f), Tooltip("The time it take to fade in the credits")] private float fadeTime = 1.0f;
    [SerializeField, Range(0.01f, 20.0f), Tooltip("Time before the credits starts fading in")] private float timeToFade = 1.0f;

    Color startColor = new Color(1, 1, 1, 0);
    Color endColor = new Color(1, 1, 1, 1);

    private EndCreditsSoundTrigger ambientSoundTrigger;

    private bool hasTriggered = false;
    float fadeDelta = 0;

    private void Awake()
    {
        creditsImage.color = startColor;
        zapText.color = startColor;

        ambientSoundTrigger = gameObject.GetComponent<EndCreditsSoundTrigger>();
    }
    //private void Start()
    //{
        
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && hasTriggered == false)
        {
            hasTriggered = true;
            StartCoroutine(WaitBeforeStartFade());
        }
    }

    IEnumerator WaitBeforeStartFade() 
    {
        yield return new WaitForSeconds(timeToFade);
        ambientSoundTrigger.enabled = true;
        StartCoroutine(FadeCredits());
    }

    IEnumerator FadeCredits()
    {
        while (fadeDelta <= 1.0f + ((1.0f/30.0f) * 2))
        {
            creditsImage.color = Color.Lerp(startColor, endColor, fadeDelta);
            zapText.color = Color.Lerp(startColor, endColor, fadeDelta);

            fadeDelta += Time.deltaTime / fadeTime;

            yield return new WaitForSeconds(1.0f / 30.0f);
        }
    }
}
