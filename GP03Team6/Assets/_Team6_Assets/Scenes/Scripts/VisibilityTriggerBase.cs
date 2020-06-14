using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityTriggerBase : MonoBehaviour
{
    [SerializeField, Tooltip("Specify which object to show")] GameObject objectToShow;
    [SerializeField] protected bool visibility;

    protected bool hasTriggered = false;

    protected void SetVisibility()
    {
        if (hasTriggered == false)
        {
            hasTriggered = true;
            objectToShow.SetActive(visibility);
        }
    }

    protected void SetVisibility(bool visibility)
    {
        objectToShow.SetActive(visibility);
    }
}
