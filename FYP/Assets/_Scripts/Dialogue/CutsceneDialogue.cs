using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneDialogue : MonoBehaviour
{
    [SerializeField] float wait = 1;
    [SerializeField] GameObject DialogueBox;
    public string nextLine;
    [SerializeField] DialogueHandler handler;

    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(wait);

        DialogueBox.SetActive(true);

        handler.dialogueText = DialogueBox.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

        handler.findCutsceneDialogue(nextLine, this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            handler.findCutsceneDialogue(nextLine, this);
    }
}
