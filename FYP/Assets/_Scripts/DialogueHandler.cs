using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Xml.Linq;
using System.Linq;

public class DialogueHandler : MonoBehaviour
{
    string currentText;
    string[] optionsText;
    [SerializeField] Button[] options;
    public TMPro.TextMeshProUGUI dialogueText, choice1Text, choice2Text;
    XElement[] optionsArray, foundPortraitsArray;
    public string characterName = "Diana";
    public DialoguePromptHandler currentCharacterPrompt;
    public GameObject shopBox;
    public Sprite[] possiblePortraits;
    public Image portrait;

    [SerializeField] ResourceManager rm;

    void OnEnable()
    {
        //findDialogue("01");
    }

    public void findDialogue(string id)
    {
        XElement file = XElement.Load("./Assets/xml/dialogues.xml");


        IEnumerable<XElement> dialogues =
            from e in file.Descendants("character")
            where e.Attribute("name").Value == characterName
            from q in e.Descendants("dialogue")
            where q!=null && q.Attribute("id").Value == id
            select q;


        optionsText = null;

        foreach (XElement z in dialogues)
        {
            dialogueText.text = z.Attribute("content").Value;
            findOptions(z);
            if (z.Attribute("emotion").Value != null) findPortrait(z);
            //portrait.sprite = possiblePortraits[1];
        }

    }

    public void selectChoice(int button)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (optionsArray[button].Attribute("target").Value == "-1") { currentCharacterPrompt.toggleDialogue(); }
        else if(optionsArray[button].Attribute("target").Value == "shop") { shopBox.SetActive(true); }
        else { findDialogue("0"+optionsArray[button].Attribute("target").Value);
            print("0"+optionsArray[button].Attribute("target").Value); }
    }

    void findOptions(XElement a)
    {
        IEnumerable<XElement> options =
            from e in a.Descendants()
            select e;

         optionsArray = options.ToArray();

        choice1Text.text = optionsArray[0].Attribute("content").Value;
        choice2Text.text = optionsArray[1].Attribute("content").Value;

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

    public void closeShopWindow(GameObject window)
    {
        print(EventSystem.current.currentSelectedGameObject.name);
        window.SetActive(false);
    }



}
