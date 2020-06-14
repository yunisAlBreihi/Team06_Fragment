using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterDialogue
{
    BERTHA,
    OJNAR,
}

[System.Serializable]
public class Dialogue
{
    [SerializeField, Tooltip("The dialogue text")] private string dialogueText;
    [SerializeField, Tooltip("Which characters dialogue this is")] private CharacterDialogue dialogueOwner;
    [SerializeField, Range(0.01f, 5.0f), Tooltip("The time before starting the dialogue")] private float timeBeforeStarting = 0.1f;
    [SerializeField, Range(0.01f, 5.0f), Tooltip("The time before the dialogue starts to fade")] private float timeBeforeFadingDialogue = 1.0f;
    [SerializeField, Range(0.01f, 3.0f), Tooltip("The speed in which the dialogue fades")] private float fadeTime = 0.5f;

    public CharacterDialogue DialogueOwner => dialogueOwner;
    public string DialogueText => dialogueText;
    public float TimeBeforeStarting => timeBeforeStarting;
    public float TimeBeforeFadingDialogue => timeBeforeFadingDialogue;
    public float FadeTime => fadeTime;
}

[CreateAssetMenu(fileName = "ConversationScriptable", menuName = "Dialogue/ConversationScriptable")]
public class ConversationScriptable : ScriptableObject
{
    [SerializeField, Tooltip("List of dialogues that make up the conversation")] private List<Dialogue> dialogues;

    public List<Dialogue> Dialogues => dialogues;
}