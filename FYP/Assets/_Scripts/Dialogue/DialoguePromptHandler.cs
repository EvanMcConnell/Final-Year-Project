using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePromptHandler : MonoBehaviour
{
    [SerializeField] PlayerMovement playerControlScript;
    [SerializeField] GameObject dialogueBox;
    //[SerializeField] GameObject shopBox;
    [SerializeField] DialogueHandler dialogueHandler;
    [SerializeField] Character character;
    [SerializeField] string defaultDialogueOption;
    [SerializeField] bool cutscene;
    [SerializeField] private CutsceneDialogue cutsceneDialogue;
    [SerializeField] Image portrait;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        dialogueHandler = DialogueHandler.Instance;
        dialogueBox = dialogueHandler.transform.GetChild(0).gameObject;
        playerControlScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (cutscene)
                cutsceneDialogue.gameObject.SetActive(true);            
            else
                StartCoroutine(toggleDialogue());
        }
    }

    public IEnumerator toggleDialogue()
    {
        dialogueHandler.currentCharacterPrompt = this;
        dialogueHandler.possiblePortraits = character.portraits;
        
        yield return new WaitForEndOfFrame();
        
        playerControlScript.enabled = !playerControlScript.isActiveAndEnabled;
        playerControlScript.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        dialogueHandler.portrait = portrait;
        dialogueHandler.shopBox.SetActive(false);
        dialogueBox.SetActive(!dialogueBox.activeInHierarchy);
        dialogueHandler.characterName = character.name;
        print("control test" + playerControlScript.enabled);
        if(playerControlScript.enabled == false)
            dialogueHandler.findDialogue(defaultDialogueOption);
    }
}
