using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int levelCount, currentLevel;
    [SerializeField] GameObject circle, bar;
    Transform levelProgressScreen;

    WaitForSecondsRealtime 
        sixSecs = new WaitForSecondsRealtime(6),
        oneSec = new WaitForSecondsRealtime(1),
        threeSecs = new WaitForSecondsRealtime(3);
    

    private void Start()
    {
        GameObject.Find("Transition Sprite").GetComponent<Animator>().Play("Idle");

        GameObject manager = GameObject.Find("Level Manager");
        if (manager)
        {
            LevelManager lm = manager.GetComponent<LevelManager>();
            lm.currentLevel++;
            if(lm.levelCount == lm.currentLevel)
            {
                GameObject.Find("Level").GetComponent<LevelGenerator>().nextLevelName = "Hub";
            }

            lm.startLoadScreenCoroutine();
            
            print("A");

            Destroy(gameObject);

        }
        else
        {
            gameObject.name = "Level Manager";

            DontDestroyOnLoad(gameObject);

            levelProgressScreen = transform.GetChild(0);

            StartCoroutine(loadScreen());
            print("B");
        }
    }
    void clearLoadScreen()
    {
        foreach (Transform x in levelProgressScreen.GetChild(0))
            Destroy(x.gameObject);
    }

    public void startLoadScreenCoroutine() => StartCoroutine(loadScreen());

    public IEnumerator loadScreen()
    {
        levelProgressScreen.gameObject.SetActive(true);

        Transform horizontalGroup = levelProgressScreen.GetChild(0);

        //print("making bar");

        for(int i = 1; i <= levelCount; i++)
        {
            GameObject newCircle = Instantiate(circle, horizontalGroup);

            if(i < currentLevel)
            {
                newCircle.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                newCircle.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else if (i == currentLevel)
            {
                newCircle.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                StartCoroutine(currentLevelDot(newCircle.GetComponentInChildren<Animator>()));
            }
            else if (i > currentLevel)
            {
                newCircle.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                newCircle.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }

            if(i<levelCount)
                Instantiate(bar, horizontalGroup);
        }

        //print("bar made");

        yield return sixSecs;

        //print("hide bar");
        transform.GetChild(0).GetChild(1).GetComponent<Animator>().Play("Fade_in");
        //print("bar hidden");

        yield return oneSec;

        //print("clearing screen");
        clearLoadScreen();
        //print("screen cleared");
        transform.GetChild(0).gameObject.SetActive(false);
        //print("reveal level");
        GameObject.Find("Transition Sprite").GetComponent<Animator>().Play("Fade_out");
        //print("level revealed");

        foreach (GameObject x in GameObject.FindGameObjectsWithTag("Player"))
            x.GetComponent<PlayerMovement>().enabled = true;

        yield return oneSec;

        GameObject.Find("HUD").transform.GetChild(0).GetComponent<Animator>().enabled = true;
    }

    IEnumerator currentLevelDot(Animator anim)
    {
        yield return threeSecs;
        anim.enabled = true;

        
    }
}

