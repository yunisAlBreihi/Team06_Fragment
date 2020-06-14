using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOnDeath : DialogueTriggerBase
{
    private void OnDisable()
    {
        if (hasTriggered == false)
        {
            TriggerDialogue();
        }
    }
}