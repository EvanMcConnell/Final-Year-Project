using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationHandler : MonoBehaviour
{
    enum state { idle, open, close }
    [SerializeField] state startingState;
    [SerializeField] bool shadow;
    Animator anim;
    private AudioSource audio;
    [SerializeField] private AudioClip[] clips;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        
        anim = GetComponent<Animator>();
        string animation = "";

        switch (startingState)
        {
            case state.idle:
                animation = "idle";
                break;

            case state.open:
                animation = shadow ? "open_shadow" : "open";
                break;

            case state.close:
                animation = shadow ? "close_shadow" : "close";
                break;
        }

        anim.Play(animation);
    }

    public void toggleDoor(bool open, bool shadow, bool up)
    {
        string animation = "";

        animation += open ? "open" : "close";

        animation += shadow ? up ? "_shadow_up" : "_shadow_down" : "";

        anim.Play(animation);
        
        int clipChoice = open ? 1 : 0;
        
        print("playing clip - "+clipChoice +" "+ clips.Length+ " "+gameObject.name);
        audio.PlayOneShot(clips[clipChoice]);
    }

}
