using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTriggerHandler : MonoBehaviour
{
    //[SerializeField] GameObject dialogueBox;
    GameObject prompt;
    WaitForSecondsRealtime waitASec = new WaitForSecondsRealtime(1);
    [SerializeField] LayerMask mapSpriteLayer;

    void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.tag == "Shop")
        {
            try { 
                prompt = coll.gameObject;
                prompt.GetComponent<DialoguePromptHandler>().enabled = true;
                prompt.GetComponent<SpriteRenderer>().enabled = true;
            }
            catch { 
                print("no children found on game object");
            }
        }
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.tag == "LevelTrigger")
        {
            GameObject.Find("TransitionSprite").GetComponent<Animator>().Play("Fade_in");
            yield return waitASec;
            SceneManager.LoadSceneAsync(other.name);
        }
        if (other.gameObject.tag == "MapSprite")
        {
            other.GetComponent<SpriteRenderer>().enabled = true;
        }
        yield return null;
    }

    void OnTriggerExit(Collider coll)
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
