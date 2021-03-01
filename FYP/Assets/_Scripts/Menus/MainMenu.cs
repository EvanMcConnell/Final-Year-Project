using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //start, main, newGame, loadGame, options
    [SerializeField] Transform[] screens;
    int activeScreen = 0;

    [SerializeField] AudioSource menuAudio;
    [SerializeField] AudioClip[] menuSounds;

    //[SerializeField] MenuButton[] main, newGame, loadGame, options;
    int activeButton = 0;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        

        switch (activeScreen)
        {
            case 0:
                if (Input.anyKey)
                    changeScreen(1);
                break;

            case 1:

                break;
        }
    }

    public void changeScreen(int newScreen)
    {
        screens[activeScreen].gameObject.SetActive(false);
        screens[newScreen].gameObject.SetActive(true);
        activeScreen = newScreen;
        PlayMenuAudio(1);
    }

    public void PlayMenuAudio(int sound) => menuAudio.PlayOneShot(menuSounds[sound]);


    public void Continue() => StartCoroutine(LoadScene("Hub"));

    public IEnumerator LoadScene(string sceneName)
    {
        PlayMenuAudio(1);
        yield return new WaitForSecondsRealtime(menuSounds[1].length);
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void quitToDesktop() => Application.Quit();
}
