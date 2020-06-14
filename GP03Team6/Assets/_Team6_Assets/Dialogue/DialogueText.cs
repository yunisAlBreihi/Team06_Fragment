using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueText : MonoBehaviour
{
    [SerializeField] Image background;

    private const float fadeSmoothness = 1 * 0.016666667f;
    private Text text;
    private Color originalColor;
    private Color originalBGColor;
    private bool isReading = false;

    public Text Text => text;
    public Color OriginalColor => originalColor;

    public bool IsReading => isReading;

    private enum FadeInOrOut
    {
        IN,
        OUT,
    }

    private void Awake()
    {
        text = gameObject.GetComponent<Text>();
        originalColor = text.color;
        originalBGColor = background.color;

        if (text == null)
        {
            Debug.Log("could not find Text on object: " + gameObject.name);
        }
        text.text = "";
        background.color = Color.clear;
    }

    public void ReadDialogue(Dialogue dialogue)
    {
        isReading = true;
        StartCoroutine(WaitBeforeReading(dialogue));
    }

    private IEnumerator WaitBeforeReading(Dialogue dialogue)
    {
        yield return new WaitForSeconds(dialogue.TimeBeforeStarting);
        StartCoroutine(FadeDialogue(dialogue, FadeInOrOut.IN));
    }

    private IEnumerator FadeDialogue(Dialogue dialogue, FadeInOrOut fadeInOrOut)
    {
        yield return FadeInOrOutDialogue(this, dialogue, fadeInOrOut);

        if (fadeInOrOut == FadeInOrOut.IN)
        {
            StartCoroutine(FadePause(dialogue, FadeInOrOut.OUT));
        }
        else if (fadeInOrOut == FadeInOrOut.OUT)
        {
            isReading = false;
        }
    }


    private IEnumerator FadeInOrOutDialogue(DialogueText dialogueText, Dialogue dialogue, FadeInOrOut fadeInOrOut)
    {
        float textColorFadeTime = 0;

        if (fadeInOrOut == FadeInOrOut.IN)
        {
            dialogueText.Text.text = dialogue.DialogueText;
        }
        while (textColorFadeTime <= 1 + fadeSmoothness)
        {
            if (fadeInOrOut == FadeInOrOut.IN)
            {
                dialogueText.Text.color = Color.Lerp(Color.clear, dialogueText.OriginalColor, textColorFadeTime);
                background.color = Color.Lerp(Color.clear, originalBGColor, textColorFadeTime);
            }
            else
            {
                dialogueText.Text.color = Color.Lerp(dialogueText.OriginalColor, Color.clear, textColorFadeTime);
                background.color = Color.Lerp(originalBGColor, Color.clear, textColorFadeTime);
            }
            textColorFadeTime += Time.deltaTime / dialogue.FadeTime;
            yield return new WaitForSeconds(fadeSmoothness);
        }
    }

    private IEnumerator FadePause(Dialogue dialogue, FadeInOrOut fadeInOrOut)
    {
        yield return new WaitForSeconds(dialogue.TimeBeforeFadingDialogue);
        StartCoroutine(FadeDialogue(dialogue, fadeInOrOut));
    }
}