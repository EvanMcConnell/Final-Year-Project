using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [SerializeField] TMPro.TextMeshProUGUI lifeCounter;
    [SerializeField] TMPro.TextMeshProUGUI gunpowderCounter;
    [SerializeField] TMPro.TextMeshProUGUI metalCounter;
    [SerializeField] TMPro.TextMeshProUGUI woodCounter;


    public void updateCounter(string counterName, int value)
    {
        switch (counterName)
        {
            case "lifeCounter":
                lifeCounter.text = value.ToString();
                break;

            case "gunpowderCounter":
                gunpowderCounter.text = value.ToString();
                break;

            case "metalCounter":
                metalCounter.text = value.ToString();
                break;

            case "woodCounter":
                woodCounter.text = value.ToString();
                break;

            default:
                print(" HUD manager: counterName String error");
                break;
        }
    }

    public void updateResourceCounters()
    {
        gunpowderCounter.text = ResourceManager.Instance.getPowder().ToString();
        metalCounter.text = ResourceManager.Instance.getMetal().ToString();
        woodCounter.text = ResourceManager.Instance.getWood().ToString();
    }

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            print("HUDManager Instance found - destroying instance on "+gameObject.name);
            Destroy(this);
        }
    }

    private void Start()
    {
        HUDManager.Instance.updateResourceCounters();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
