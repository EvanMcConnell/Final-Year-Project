using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Linq;
using System.Linq;

public class DialogueHandler : MonoBehaviour
{
    string currentText;
    string[] optionsText;
    [SerializeField] Button[] options;
    public TMPro.TextMeshProUGUI dialogueText, choice1Text, choice2Text;
    XElement[] optionsArray;
    public string name = "Diana";
    public DialoguePromptHandler currentCharacterPrompt;

    void OnEnable()
    {
        //findDialogue("01");
    }

    public void findDialogue(string id)
    {
        XElement file = XElement.Load("./Assets/dialogues.xml");


        IEnumerable<XElement> dialogues =
            from e in file.Descendants("character")
            where e.Attribute("name").Value == name
            from q in e.Descendants("dialogue")
            where q!=null && q.Attribute("id").Value == id
            select q;


        optionsText = null;

        foreach (XElement z in dialogues)
        {
            dialogueText.text = z.Attribute("content").Value;
            findOptions(z);
        }

    }

    public void selectChoice(int button)
    {
        if (optionsArray[button].Attribute("target").Value == "-1") { currentCharacterPrompt.toggleDialogue(); }
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


}
