﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Xml.Linq;
using System.Linq;
using UnityEngine.SceneManagement;

public class DialogueHandler : MonoBehaviour
{
    public static DialogueHandler Instance;
    string currentText;
    string[] optionsText;
    [SerializeField] Button[] options;
    public TMPro.TextMeshProUGUI dialogueText, cutsceneText;
    [SerializeField] TMPro.TextMeshProUGUI[] choiceTexts;
    XElement[] optionsArray, foundPortraitsArray;
    public string characterName = "Diana";
    public DialoguePromptHandler currentCharacterPrompt;
    public GameObject shopBox;
    public Sprite[] possiblePortraits;
    public Image portrait;
    [SerializeField] float scrollSpeed;
    [SerializeField] AudioClip typingClick;
    [SerializeField] bool shopAvailable = false;
    bool textFinished = true;
    public AudioSource audio;


    [SerializeField] ResourceManager rm;

    void OnEnable()
    {
        //findDialogue("01");
        audio = GetComponent<AudioSource>();
        rm = ResourceManager.Instance;
    }


    private void Start()
    {
        if (Instance)
        {
            this.enabled = false;
        }
        else
        {
            Instance = this;
        }
    }

    public void findDialogue(string id)
    {
        textFinished = false;
        XElement file = XElement.Load("./Assets/xml/dialogues.xml");


        IEnumerable<XElement> dialogues =
            from e in file.Descendants("character")
            where e.Attribute("name").Value == characterName
            from q in e.Descendants("dialogue")
            where q != null && q.Attribute("id").Value == id
            select q;


        optionsText = null;
        foreach (XElement z in dialogues)
        {
            StartCoroutine(writeDialogueText(z.Attribute("content").Value));
            //dialogueText.text = z.Attribute("content").Value;

            findOptions(z);

            if (z.Attribute("emotion").Value != null) findPortrait(z);
        }

    }

    public void findCutsceneDialogue(string id, CutsceneDialogue handler)
    {
        if (textFinished)
        {
            print("setting text finished to false");

            if (id == "-1")
            {
                transform.GetChild(1).gameObject.SetActive(false);
                handler.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {

                textFinished = false;
                XElement file = XElement.Load("./Assets/xml/dialogues.xml");


                IEnumerable<XElement> dialogues =
                    from e in file.Descendants("character")
                    where e.Attribute("name").Value == characterName
                    from q in e.Descendants("dialogue")
                    where q != null && q.Attribute("id").Value == id
                    select q;


                foreach (XElement z in dialogues)
                {
                    StartCoroutine(writeCutsceneText(z.Attribute("content").Value));

                    IEnumerable<XElement> targets =
                        from f in z.Descendants("choice")
                        select f;

                    foreach (XElement x in targets)
                        handler.nextLine = x.Attribute("target").Value;

                    if (z.Attribute("emotion").Value != null) findPortrait(z);
                }

                EventSystem.current.SetSelectedGameObject(null);

            }
        }
        else
        {
            print("setting text finished to true");
            textFinished = true;
        }

    }

    IEnumerator writeCutsceneText(string text)
    {
        cutsceneText.text = "";

        foreach (char c in text)
        {
            if (!textFinished)
            {
                audio.PlayOneShot(typingClick);
                cutsceneText.text += c;
                yield return new WaitForSecondsRealtime(scrollSpeed);
            }
            else
            {
                cutsceneText.text = text;
                break;
            }

        }

        print("done");

        textFinished = true;
    }
    
    IEnumerator writeDialogueText(string text)
    {
        dialogueText.text = "";

        foreach (char c in text)
        {
            if (!textFinished)
            {
                audio.PlayOneShot(typingClick);
                dialogueText.text += c;
                print("letter");
                yield return new WaitForSecondsRealtime(scrollSpeed);
            }
            else
            {
                print("skipped");
                dialogueText.text = text;
                break;
            }

        }

        print("done");

        textFinished = true;
    }

    public void selectChoice(int button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (optionsArray[button].Attribute("target").Value == "-1") { StartCoroutine(currentCharacterPrompt.toggleDialogue()); }
        else if (optionsArray[button].Attribute("target").Value == "shop") { shopBox.SetActive(true); }
        else if (optionsArray[button].Attribute("target").Value == "turnback")
        {
            StartCoroutine(currentCharacterPrompt.toggleDialogue());
            Destroy(GameObject.Find("Level Manager"));
            SceneManager.LoadScene("Hub");
        }else if (optionsArray[button].Attribute("target").Value == "continue")
        {
            StartCoroutine(currentCharacterPrompt.toggleDialogue());
            SceneManager.LoadScene(GameObject.Find("Level Manager").GetComponent<LevelManager>().nextLevelName);
        }
        else
        {
            findDialogue("0" + optionsArray[button].Attribute("target").Value);
            print("0" + optionsArray[button].Attribute("target").Value);
        }
    }

    void findOptions(XElement a)
    {
        IEnumerable<XElement> options =
            from e in a.Descendants()
            select e;

        optionsArray = options.ToArray();

        for (int i = 0; i < choiceTexts.Length; i++)
        {
            choiceTexts[i].text = optionsArray[i].Attribute("content").Value;
        }

        //choice1Text.text = optionsArray[0].Attribute("content").Value;
        //choice2Text.text = optionsArray[1].Attribute("content").Value;
        /*for (int i=0; i<options.Count()-1; i++)
        {
            optionsText[i] = optionsArray[i].Attribute("conetent").Value;
        }*/
    }


    void findPortrait(XElement a)
    {
        /*IEnumerable<XElement> portraits =
            from e in a.Descendants()
            select e;

        foundPortraitsArray = portraits.ToArray();*/

        switch (a.Attribute("emotion").Value)
        {
            case "Neutral":
                portrait.sprite = possiblePortraits[0];
                break;

            case "Frustrated":
                portrait.sprite = possiblePortraits[1];
                break;

            case "Happy":
                portrait.sprite = possiblePortraits[2];
                break;

            default:
                portrait.sprite = possiblePortraits[0];
                break;
        }

    }

    void openShopWindow() => shopBox.SetActive(true);

    public void closeShopWindow(GameObject window) => window.SetActive(false);



}
