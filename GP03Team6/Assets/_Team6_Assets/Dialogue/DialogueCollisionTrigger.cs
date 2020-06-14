using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCollisionTrigger : DialogueTriggerBase
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TriggerDialogue();
        }
    }
}