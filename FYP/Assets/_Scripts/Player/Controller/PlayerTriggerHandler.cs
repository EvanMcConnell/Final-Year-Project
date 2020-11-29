using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerHandler : MonoBehaviour
{
    [SerializeField] GameObject dialogueBox;
    GameObject prompt;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Shop")
        {
            try { 
                prompt = coll.gameObject;
                print(prompt.name);
                prompt.GetComponent<DialoguePromptHandler>().enabled = true;
                prompt.GetComponent<SpriteRenderer>().enabled = true;
            }
            catch { 
                print("no children found on game object");
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Shop")
        {
            try
            {
                prompt = coll.gameObject;
                print(prompt.name);
                prompt.GetComponent<DialoguePromptHandler>().enabled = false;
                prompt.GetComponent<SpriteRenderer>().enabled = false;
            }
            catch
            {
                print("no children found on game object");
            }
        }
    }
}
