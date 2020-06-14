using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class MemoryFragment : MonoBehaviour
{
    private bool isLooking = false;
    private bool hasPressedButton = false;
    private IMovement lastState;

    [SerializeField]
    public Sprite painting;
    [SerializeField]
    public Image img;
    [SerializeField, Range(0.1f, 100f)]
    public float timeToLoad = 0.1f;

    public Text closePrompt;

    private bool b = true, canRemove = false;

    private SoundSystem ss;
    private PlayerMovement pl;

    public bool HasPressedButton { get => hasPressedButton; set => hasPressedButton = value; }

    void Start()
    {
        pl = PlayerMovement.MyPlayer;
        ss = GetComponent<SoundSystem>();
        if(pl.input.useGamePad == true)
        {
            closePrompt.text = "Press A to continue";
        }
        else
        {
            closePrompt.text = "Press E to continue";
        }
        closePrompt.gameObject.SetActive(false);
    }

    private void Update()
    {
        RemoveMemoryOnInteract();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && b || other.gameObject.layer == 14 && b)
        {
            b = false;
            pl = other.gameObject.GetComponent<PlayerMovement>();

            ss.PlaySound(0);
            img.sprite = painting;
            img.gameObject.SetActive(true);
            isLooking = true;

            lastState = pl.currentState;
            StartCoroutine(pl.SwitchState(pl.cutsceneState));

            StartCoroutine(Cooldown());
            StartCoroutine("UpdateColorA");
        }
    }

    private IEnumerator UpdateColorA()
    {
        float numToAdd = 1f / timeToLoad;
        for (float i = 0; i < 2; i += numToAdd * Time.deltaTime)
        {
            img.color = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        StopCoroutine("UpdateColorA");
    }

    private void RemoveMemoryOnInteract()
    {
        if (isLooking)
        {
            if(pl.input.Buttons() == "Interact" && canRemove)
            {
                closePrompt.gameObject.SetActive(false);
                hasPressedButton = true;
                isLooking = false;
                img.gameObject.SetActive(false);
                ss.PlaySound(1);

                StartCoroutine(pl.SwitchState(lastState));

                StartCoroutine(Destroy());
            }

        }
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(4f);
        canRemove = true;
        yield return new WaitForSeconds(2f);
        closePrompt.gameObject.SetActive(true);
        while (closePrompt.color.a < 1f && isLooking)
        {
            closePrompt.color =new Vector4(closePrompt.color.r, closePrompt.color.g, closePrompt.color.b, Mathf.MoveTowards(closePrompt.color.a, 1f, Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }
    }
}
