using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePromptHandler : MonoBehaviour
{
    [SerializeField] PlayerMovement playerControlScript;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject shopBox;
    [SerializeField] DialogueHandler dialogueHandler;
    [SerializeField] Character character;
    [SerializeField] string defaultDialogueOption;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            toggleDialogue();
        }
    }

    public void toggleDialogue()
    {
        playerControlScript.enabled = !playerControlScript.isActiveAndEnabled;
        playerControlScript.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        dialogueHandler.shopBox.SetActive(false);
        dialogueBox.SetActive(!dialogueBox.activeInHierarchy);
        dialogueHandler.characterName = character.name;
        dialogueHandler.possiblePortraits = character.portraits;
        dialogueHandler.findDialogue(defaultDialogueOption);
        dialogueHandler.currentCharacterPrompt = this;
    }
}
