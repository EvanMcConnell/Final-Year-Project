using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    void Start()
    {
        float vol;

        mixer.GetFloat("MasterVolume", out vol);
        GameObject.Find("Master Volume").transform.GetChild(1).GetComponent<Slider>().value = Mathf.Pow(10, vol / 20);

        mixer.GetFloat("MusicVolume", out vol);
        GameObject.Find("Music Volume").transform.GetChild(1).GetComponent<Slider>().value = Mathf.Pow(10, vol / 20);

        mixer.GetFloat("EffectsVolume", out vol);
        GameObject.Find("Effects Volume").transform.GetChild(1).GetComponent<Slider>().value = Mathf.Pow(10, vol / 20);
    }

    public void setVolume(float sliderValue)
    {
        switch (EventSystem.current.currentSelectedGameObject.transform.parent.name)
        {
            case "Master Volume":
                mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
                break;

            case "Music Volume":
                mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
                break;

            case "Effects Volume":
                mixer.SetFloat("EffectsVolume", Mathf.Log10(sliderValue) * 20);
                break;
        }
    }
}
