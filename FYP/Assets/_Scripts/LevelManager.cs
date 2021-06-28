using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int level1, level2, level3;
    public int levelCount, currentLevel;
    [SerializeField] GameObject circle, bar;
    Transform levelProgressScreen;
    public String nextLevelName;
    [SerializeField] private Animator checkpoint1, checkpoint2, checkpoint3;
    [SerializeField] private Transform group1, group2, group3;

    WaitForSecondsRealtime
        sixSecs = new WaitForSecondsRealtime(6),
        oneSec = new WaitForSecondsRealtime(1),
        threeSecs = new WaitForSecondsRealtime(3);

    private NavMeshAgent[] agents;


    private void Awake()
    {
        GameObject.Find("Transition Sprite").GetComponent<Animator>().Play("Idle");
        levelCount = level1 + level2 + level3 + 3;

        GameObject manager = GameObject.Find("Level Manager");
        if (manager)
        {
            print("found you");
            
            LevelManager lm = manager.GetComponent<LevelManager>();
            lm.currentLevel++;
            
            //LevelGenerator generator = GameObject.Find("Level").GetComponent<LevelGenerator>();
            
            if (lm.currentLevel < lm.level1) lm.nextLevelName = "1";
            else if (lm.currentLevel < lm.level1 + 1 + lm.level2) lm.nextLevelName = "2";
            else if (lm.currentLevel < lm.level1 + 1 + lm.level2 + 1 + lm.level3) lm.nextLevelName = "3";
            else if (lm.currentLevel < lm.level1 + 1 + lm.level2 + 1 + lm.level3) lm.nextLevelName = "4";
            if (lm.currentLevel == lm.level1 ||
                     lm.currentLevel == lm.level1 + 1 + lm.level2)
                lm.nextLevelName = "Turnback";
            else if (lm.currentLevel == lm.level1 + 1 + lm.level2 + 1 + lm.level3)
                lm.nextLevelName = "End";

            print(lm.nextLevelName);

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
        // foreach (Transform x in levelProgressScreen.GetChild(0))
        //     Destroy(x.gameObject);
        foreach (Transform x in group1)
            Destroy(x.gameObject);
        foreach (Transform x in group2)
            Destroy(x.gameObject);
        foreach (Transform x in group3)
            Destroy(x.gameObject);
    }

    public void startLoadScreenCoroutine() => StartCoroutine(loadScreen());

    public IEnumerator loadScreen()
    {
        levelProgressScreen.gameObject.SetActive(true);

        Transform horizontalGroup = levelProgressScreen.GetChild(0).GetChild(0);

        //print("making bar");

        if (currentLevel > level1 + 1)
        {
            checkpoint1.Play("wave");
        }

        if (currentLevel > level1 + 1 + level2 + 1)
        {
            checkpoint2.Play("wave");
        }

        if (currentLevel > level1 + 1 + level2 + 1 + level3 + 1)
        {
            checkpoint3.Play("wave");
        }

        if (currentLevel == level1 + 1)
        {
            checkpoint1.Play("rise");
        }
        else if (currentLevel == level1 + 1 + level2 + 1)
        {
            checkpoint2.Play("rise");
        }
        else if (currentLevel == level1 + 1 + level2 + 1 + level3 + 1)
        {
            checkpoint3.Play("rise");
        }
        
        

        
        for (int i = 1; i <= levelCount; i++)
        {
            bool flag = i == level1 + 1 || i == level1 + 1 + level2 + 1;

            if (flag) continue;
            
            Transform newCircleParent = null;
            bool spawnBar = false;

            if (i <= level1)
            {
                newCircleParent = group1;
                if (i < level1)
                    spawnBar = true;
            }
            else if (i <= level1 + 1 + level2)
            {
                newCircleParent = group2;
                if (i < level1 + 1 + level2)
                    spawnBar = true;
            }
            else if (i <= level1 + 1 + level2 + 1 + level3)
            {
                newCircleParent = group3;
                if (i < level1 + 1 + level2 + 1 + level3)
                    spawnBar = true;
            }


            if (newCircleParent != null)
            {
                GameObject newCircle = Instantiate(circle, newCircleParent);
                

                if (i < currentLevel)
                {
                    newCircle.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    newCircle.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
                }
                else if (i == currentLevel)
                {
                    newCircle.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    if(!flag)
                        StartCoroutine(currentLevelDot(newCircle.GetComponentInChildren<Animator>()));
                    else
                        newCircle.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                }
                else if (i > currentLevel)
                {
                    newCircle.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    newCircle.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
                }

                if (spawnBar)
                    Instantiate(bar, newCircleParent);
            }
        }

        yield return threeSecs;

        disableAI();

        //print("bar made");

        yield return threeSecs;

        //print("hide bar");
        transform.GetChild(0).GetChild(1).GetComponent<Animator>().Play("Fade_in");
        //print("bar hidden");

        yield return oneSec;

        enableAI();

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
        Debug.Log("waiting to activate animator", anim.gameObject);
        yield return threeSecs;
        Debug.Log("activating animator", anim.gameObject);
        anim.enabled = true;
    }

    void disableAI()
    {
        agents = FindObjectsOfType(typeof(NavMeshAgent)) as NavMeshAgent[];
        //print(agents.Length);
        foreach (NavMeshAgent agent in agents)
        {
            try{
            agent.gameObject.SetActive(false);
            }
            catch
            {
                print("agent not found");
            }
        }
    }

    void enableAI()
    {
        //print(agents.Length);
        foreach (NavMeshAgent agent in agents)
        {
            try
            {
                agent.gameObject.SetActive(true);
            }
            catch
            {
                print("agent not found");
            }
        }
    }
}