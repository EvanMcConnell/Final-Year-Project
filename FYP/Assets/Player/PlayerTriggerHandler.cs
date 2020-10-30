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
            try { prompt = coll.gameObject.transform.GetChild(0).gameObject; }
            catch { print("no children found on game object"); }

            
            print(prompt.name);
            prompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Shop")
        {
            try { prompt = coll.gameObject.transform.GetChild(0).gameObject; }
            catch { print("no children found on game object"); }


            print(prompt.name);
            prompt.SetActive(false);
        }
    }
}
