using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerCutscene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            GetComponent<CutsceneDialogue>().enabled = true;
    }
}
