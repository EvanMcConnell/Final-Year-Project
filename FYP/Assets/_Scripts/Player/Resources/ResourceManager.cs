using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int wood = 0, metal = 0, gunpowder = 0;

    [SerializeField] HUDManager hud;

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
                        hud.updateCounter("woodCounter", wood);
                        Destroy(col.gameObject);
                    break;

                    case "Metal":
                        metal += r.getQuantity();
                        hud.updateCounter("metalCounter", metal);
                        Destroy(col.gameObject);
                        break;
                }
            }
        }
    }
}
