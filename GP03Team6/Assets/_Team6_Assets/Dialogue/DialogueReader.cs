using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class DialogueReader : MonoBehaviour
{
    [SerializeField, Tooltip("Link the character here")] private PlayerMovement playerMovement;
    [SerializeField, Tooltip("This is used to specify which Text object displays Berthas dialogue")] private DialogueText berthaDialogue;
    [SerializeField, Tooltip("This is used to specify which Text object displays Ojnars dialogue")] private DialogueText ojnarDialogue;
    [SerializeField, Range(1.0f, 10.0f), Tooltip("The speed of the player during slowdown," +
        " keep this the same as the players movement speed as max, otherwise he will speed up")]
    private float playerSlowdownSpeed = 2.0f;
    [SerializeField, Tooltip("If the dialogue slows down the player while reading or not")] private bool slowDown = false;

    private bool isReading;

    private List<ConversationScriptable> conversationList = new List<ConversationScriptable>();
    private List<string> conversationSoundList = new List<string>();
    Dialogue currentDialogue;
    DialogueText currentDialogueText;

    private int currentDialogueIndex = 0;
    private float originalSpeed;
    private float originalRotationSpeed;

    enum SpeedChange
    {
        DOWN,
        UP,
    }

    private void Awake()
    {
        if (berthaDialogue == null)
        {
            berthaDialogue = GameObject.FindGameObjectWithTag("BerthaDialogue").GetComponent<DialogueText>();
        }

        if (ojnarDialogue == null)
        {
            ojnarDialogue = GameObject.FindGameObjectWithTag("OjnarDialogue").GetComponent<DialogueText>();
        }
        Debug.Assert(berthaDialogue != null, "You need to specify a Player with an attached Dialogue");
        Debug.Assert(ojnarDialogue != null, "You need to specify a Companion with an attached Dialogue");
    }

    private void Start()
    {
        if (playerMovement == null)
        {
            playerMovement = PlayerMovement.MyPlayer;
        }

        if (playerMovement != null)
        {
            originalSpeed = playerMovement.groundSettings.speed;
            originalRotationSpeed = playerMovement.groundSettings.rotationLerpSpeed;
        }
    }

    public void AddConversation(ConversationScriptable conversationScriptable, string conversationSound)
    {
        conversationList.Add(conversationScriptable);
        conversationSoundList.Add(conversationSound);

        if (isReading == false)
        {
            StartCoroutine(ReadConversation());
            if (slowDown == true)
            {
                StartCoroutine(ChangePlayerSpeed(SpeedChange.DOWN));
            }
        }
    }

    private IEnumerator ReadConversation()
    {
        FMODUnity.RuntimeManager.PlayOneShot(conversationSoundList[0]);
        isReading = true;

        while (conversationList.Count > 0)
        {
            if (currentDialogueText == null || currentDialogueText.IsReading == false)
            {
                currentDialogue = conversationList[0].Dialogues[currentDialogueIndex];

                if (currentDialogue.DialogueOwner == CharacterDialogue.BERTHA)
                {
                    currentDialogueText = berthaDialogue;
                }
                else if (currentDialogue.DialogueOwner == CharacterDialogue.OJNAR)
                {
                    currentDialogueText = ojnarDialogue;
                }

                if (currentDialogueText.IsReading == false)
                {
                    currentDialogueText.ReadDialogue(currentDialogue);
                    currentDialogueIndex += 1;
                }

                if (currentDialogueIndex == conversationList[0].Dialogues.Count)
                {
                    conversationList.RemoveAt(0);
                    conversationSoundList.RemoveAt(0);
                    currentDialogueIndex = 0;
                    isReading = false;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
        if (slowDown == true)
        {
        	StartCoroutine(ChangePlayerSpeed(SpeedChange.UP));
        }
    }

    private IEnumerator ChangePlayerSpeed(SpeedChange speedChange)
    {
        float speedDelta = 0.0f;

        if (speedChange == SpeedChange.DOWN)
        {
            playerMovement.abilities.jumpDisabled = true;
        }

        while (speedDelta <= 1.0f)
        {
            if (speedChange == SpeedChange.DOWN)
            {
                playerMovement.groundSettings.speed = Mathf.Lerp(originalSpeed, playerSlowdownSpeed, speedDelta);
            }
            else
            {
                playerMovement.groundSettings.speed = Mathf.Lerp(playerSlowdownSpeed, originalSpeed, speedDelta);
            }
            speedDelta += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        if (speedChange == SpeedChange.UP)
        {
            playerMovement.abilities.jumpDisabled = false;
        }
    }
}