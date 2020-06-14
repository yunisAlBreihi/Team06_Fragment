using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOnRemoveMemory : DialogueTriggerBase
{
    MemoryFragment memoryFragment;

    private void Awake()
    {
        memoryFragment = GetComponent<MemoryFragment>();
    }

    // Update is called once per frame
    void Update()
    {
        if (memoryFragment.HasPressedButton)
        {
            TriggerDialogue();
            memoryFragment.HasPressedButton = false;
        }
    }
}
