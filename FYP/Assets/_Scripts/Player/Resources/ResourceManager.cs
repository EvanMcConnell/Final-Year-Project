using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [SerializeField] int wood = 0, metal = 0, gunpowder = 0;
    [SerializeField] bool validate = false;

    public int getWood() => wood;
    public int getMetal() => metal;
    public int getPowder() => gunpowder;

    public int getQuantity(Resource resource)
    {
        switch (resource.name)
        {
            case ("Wood"):
                return wood;

            case ("Metal"):
                return metal;

            case ("powder"):
                return gunpowder;

            default:
                print("Resource get quantity - resource name [" + resource.name + "] not found.");
                return -100;
        }
    }

    private void OnValidate()
    {
        if(validate)
            HUDManager.Instance.updateResourceCounters();
    }

    public void reduceResource(Resource resource, int change)
    {
        switch (resource.name)
        {
            case ("Wood"):
                wood -= change;
                break;

            case ("Metal"):
                metal -= change;
                break;

            case ("powder"):
                gunpowder -= change;
                break;

            default:
                print("Reduce resource - resource name ["+resource.name+"] not found.");
                break;
        }

        HUDManager.Instance.updateResourceCounters();
    }

    public void gainResource(Resource resource, int change)
    {
        switch (resource.name)
        {
            case ("Wood"):
                wood += change;
                break;

            case ("Metal"):
                metal += change;
                break;

            case ("powder"):
                gunpowder += change;
                break;

            default:
                print("Gain resource - resource name [" + resource.name + "] not found.");
                break;
        }

        HUDManager.Instance.updateResourceCounters();
    }

    public bool checkQuantity(Resource resource, int checkValue)
    {
        switch(resource.name){
            case ("Wood"):
                return wood >= checkValue;

            case ("Metal"):
                return metal >= checkValue;

            case ("powder"):
                return gunpowder >= checkValue;

            default:
                return false;
        }
    }


    private void Awake()
    {
        if (!Instance)
            Instance = this;

        else
            Destroy(this);
    }

    void setInstance(ResourceManager rm)
    {
        Instance = rm;
        DontDestroyOnLoad(rm.gameObject);
    }


    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Resource")
        {
            if (col.GetComponent<ResourceHolder>().getResource())
            {
                ResourceHolder r = col.GetComponent<ResourceHolder>();
                switch (r.getResource().resourceName)
                {
                    case "Wood":
                        gainResource(r.getResource(), r.getQuantity());
                        Destroy(col.gameObject);
                    break;

                    case "Metal":
                        gainResource(r.getResource(), r.getQuantity());
                        Destroy(col.gameObject);
                        break;
                }
            }
        }
    }

    private void updateHUD(string counterName, int value)
    {
        try
        {
            HUDManager.Instance.updateCounter(counterName, value);
        }
        catch
        {
            print(gameObject.name + " no HUDManager Instance found.");
        }
    }
}
