using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int wood = 0, metal = 0, gunpowder = 0;
    public TMPro.TextMeshProUGUI woodCounter;
    public TMPro.TextMeshProUGUI metalCounter;
    public TMPro.TextMeshProUGUI gpowderCounter;

    void updateText(TMPro.TextMeshProUGUI t, string text)
    {
        t.text = text;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Resource")
        {
            if (col.GetComponent<ResourceHolder>().getResource())
            {
                ResourceHolder r = col.GetComponent<ResourceHolder>();
                switch (r.getResource().resourceName)
                {
                    case "Wood":
                        wood += r.getQuantity();
                        updateText(woodCounter, wood.ToString());
                        Destroy(col.gameObject);
                    break;

                    case "Metal":
                        metal += r.getQuantity();
                        updateText(metalCounter, metal.ToString());
                        Destroy(col.gameObject);
                        break;
                }
            }
        }
    }
}
