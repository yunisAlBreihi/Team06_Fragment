using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class InteractableBase : MonoBehaviour
{
    protected bool isTriggered = false;
    public bool IsTriggered => isTriggered;

    [SerializeField] protected InteractableBase linkedInteractable;
    public InteractableBase LinkedInteractable => linkedInteractable;

    public abstract void OnInteract();
}
