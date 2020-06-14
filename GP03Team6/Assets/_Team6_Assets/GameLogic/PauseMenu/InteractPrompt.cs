using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractPrompt : MonoBehaviour
{
    private PlayerMovement p;

    public Sprite gamepad, keyboard;
    public Image i;
    public Text t;

    private bool b;

    private Vector4 tCol, iCol;

    void Start()
    {
        b = false;
        p = PlayerMovement.MyPlayer;
        t.text = "Interact";
        if(p.input.useGamePad == true)
        {
            i.sprite = gamepad;
        }
        else
        {
            i.sprite = keyboard;
        }
        iCol = i.color;
        tCol = t.color;
        iCol.w = 0f;
        tCol.w = 0f;
        i.color = iCol;
        t.color = tCol;
    }

    private void Update()
    {
    }
    public void StartFadeIn()
    {
        b = true;
        StartCoroutine(FadeIn());
    }
    private IEnumerator FadeIn()
    {
        yield return new WaitForEndOfFrame();
        while ((iCol.w < 1f || tCol.w < 1f) && b)
        {
            iCol.w = Mathf.MoveTowards(iCol.w, 1f, Time.deltaTime);
            tCol.w = Mathf.MoveTowards(tCol.w, 1f, Time.deltaTime);
            i.color = iCol;
            t.color = tCol;

            yield return new WaitForEndOfFrame();
        }

    }

    public void StartFadeOut()
    {
        b = false;
        StartCoroutine(FadeOut());
    }
    private IEnumerator FadeOut()
    {
        yield return new WaitForEndOfFrame();
        while ((iCol.w > 0f || tCol.w > 0f) && !b)
        {
            iCol.w = Mathf.MoveTowards(iCol.w, 0f, Time.deltaTime);
            tCol.w = Mathf.MoveTowards(tCol.w, 0f, Time.deltaTime);
            i.color = iCol;
            t.color = tCol;
            yield return new WaitForEndOfFrame();
        }
    }

}
