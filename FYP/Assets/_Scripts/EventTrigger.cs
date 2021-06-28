using System;
using JetBrains.Annotations;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    enum Trigger
    {
        onDisable,
        onDestroy,
        ontriggerEnter,
        ontriggerExit
    }

    [SerializeField] private Trigger type;

    [Header("Door")] [SerializeField] private DoorAnimationHandler door;
    [SerializeField] private bool open, shadow, up;

    [Header("Cutscene")] [SerializeField] private string cutsceneName;
    [SerializeField] private CutsceneDialogue cutscene;

    [Header("Trigger Info")] [SerializeField]
    private string triggerTag;

    [Header("Toggle GameObjects")] [SerializeField]
    private GameObject[] enable, disable;

    private void Start()
    {
        try
        {
            cutscene = GameObject.Find(cutsceneName).GetComponent<CutsceneDialogue>();
        }
        catch
        {
            Debug.Log("EVENT HANDLER - Cutscene name not given", this.gameObject);
        }
    }

    private void OnDisable()
    {
        if (type == Trigger.onDisable)
        {
            if (door) door.toggleDoor(open, shadow, up);
            if (cutscene) cutscene.enabled = true;

            foreach (GameObject x in enable) x.SetActive(true);
            foreach (GameObject x in disable) x.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (type == Trigger.onDestroy)
        {
            if (door) door.toggleDoor(open, shadow, up);
            if (cutscene) cutscene.enabled = true;

            foreach (GameObject x in enable) x.SetActive(true);
            foreach (GameObject x in disable) x.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (type == Trigger.ontriggerEnter && other.CompareTag(triggerTag))
        {
            if (door) door.toggleDoor(open, shadow, up);
            if (cutscene) cutscene.enabled = true;

            foreach (GameObject x in enable) x.SetActive(true);
            foreach (GameObject x in disable) x.SetActive(false);

            this.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (type == Trigger.ontriggerExit && other.CompareTag(triggerTag))
        {
            if (door) door.toggleDoor(open, shadow, up);
            if (cutscene) cutscene.enabled = true;

            foreach (GameObject x in enable) x.SetActive(true);
            foreach (GameObject x in disable) x.SetActive(false);

            this.enabled = false;
        }
    }
}