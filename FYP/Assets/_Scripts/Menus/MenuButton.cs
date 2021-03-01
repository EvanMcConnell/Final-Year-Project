using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    MainMenu menuHandler;
    string buttonText;
    TMPro.TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        menuHandler = GameObject.Find("MenuHandler").GetComponent<MainMenu>();
        text = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        buttonText = text.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        focusButton();
    }

    private void OnMouseExit()
    {
        unfocusButton();
    }

    public void focusButton()
    {
        print("we in here");
        menuHandler.PlayMenuAudio(0);
        text.text = "/ " + buttonText + " \\";
    }

    public void unfocusButton()
    {
        text.text = buttonText;
    }
}
