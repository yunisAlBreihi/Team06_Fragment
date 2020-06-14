using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerBase : MonoBehaviour
{
    [SerializeField, Tooltip("Specify which Dialogue reader to use")] protected DialogueReader dialogueReader;
    [SerializeField, Tooltip("Specify which Dialogue to start reading from")] protected ConversationScriptable conversationScriptable;
    [FMODUnity.EventRef]
    [SerializeField] private string conversationSound;

    public string ConversationSound => conversationSound;

    protected bool hasTriggered = false;

    private void Awake()
    {
        if (dialogueReader == null)
        {
            dialogueReader = GameObject.FindGameObjectWithTag("DialogueReader").GetComponent<DialogueReader>();
        }
    }
    protected void TriggerDialogue() 
    {
        if (dialogueReader != null)
        {
            if (hasTriggered==false)
            {
                hasTriggered = true;
                dialogueReader.AddConversation(conversationScriptable, conversationSound);
            }
        }
    }
}
