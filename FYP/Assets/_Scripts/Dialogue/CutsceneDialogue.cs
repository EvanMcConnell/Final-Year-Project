using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneDialogue : MonoBehaviour
{
    [SerializeField] float wait = 1;
    [SerializeField] GameObject DialogueBox;
    [SerializeField] string firstLine;
    public string nextLine;
    [SerializeField] DialogueHandler handler;
    [SerializeField] Character character;
    [SerializeField] Image portrait;

    void OnEnable() {
        Debug.Log("enabled, starting cutscene", gameObject);
        StartCoroutine(startCutscene()); }

    private IEnumerator startCutscene()
    {
        GameObject Dialogue = GameObject.Find("Dialogue");

        handler = Dialogue.GetComponent<DialogueHandler>();
        DialogueBox = Dialogue.transform.GetChild(1).gameObject;
        portrait = DialogueBox.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>();
        print("protrait: "+portrait.gameObject.name);

        yield return new WaitForSecondsRealtime(wait);

        Time.timeScale = 0;

        DialogueBox.SetActive(true);

        handler.characterName = character.name;

        handler.possiblePortraits = character.portraits;

        handler.portrait = portrait;

        handler.findCutsceneDialogue(firstLine, this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            handler.findCutsceneDialogue(nextLine, this);
        }
    }
}
