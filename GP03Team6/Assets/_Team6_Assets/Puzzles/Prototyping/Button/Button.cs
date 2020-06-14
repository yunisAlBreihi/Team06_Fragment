using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : InteractableBase
{
    [SerializeField] Vector3 endPosition;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float timeBeforeClosing = 3.0f;

    private Vector3 startPosition;

    private bool triggerEvent = false;
    private void Awake()
    {
        startPosition = transform.position;
    }

    public override void OnInteract()
    {
        if (isTriggered == false)
        {
            isTriggered = true;
            StartCoroutine("Open"); 
            linkedInteractable.OnInteract();
        }
    }

    IEnumerator Open()
    {
        while (triggerEvent == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, Time.deltaTime);

            if (Vector3.Distance(transform.position, endPosition) < 0.01f)
            {
                triggerEvent = true;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        triggerEvent = false;
        StartCoroutine("WaitOnTop");
        StopCoroutine("Open");
    }
    IEnumerator WaitOnTop()
    {
        while (triggerEvent == false)
        {
            triggerEvent = true;
            yield return new WaitForSeconds(timeBeforeClosing);
        }
        triggerEvent = false;
        StartCoroutine("Close");
        StopCoroutine("WaitOnTop");
    }

    IEnumerator Close()
    {
        while (triggerEvent == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, Time.deltaTime);

            if (Vector3.Distance(transform.position, startPosition) < 0.01f)
            {
                triggerEvent = true;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        triggerEvent = false;
        isTriggered = false;
        StopCoroutine("Close");
    }
}
