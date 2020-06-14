using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityTrigger : VisibilityTriggerBase
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            SetVisibility();
        }
    }
}
