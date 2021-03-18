using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHolder : MonoBehaviour
{
    [SerializeField] Resource[] possibleResources;
    [SerializeField] int[] minMaxQuantity =  new int[2];
    int quantity;
    Resource resource;

    private void Start()
    {
        if (possibleResources.Length == 0) { 
            print("Resource Spawner - No resources specified.");
            Destroy(gameObject);
        }
        else
        {
            resource = possibleResources[Mathf.FloorToInt(Random.Range(0, possibleResources.Length))];

            if (minMaxQuantity.Length > 1 && minMaxQuantity[0] < minMaxQuantity[1])
            {
                quantity = Mathf.FloorToInt(Random.Range(minMaxQuantity[0], minMaxQuantity[1]));
            }
            else
            {
                print("ResourceSpawner - Illegal min max quantity values, returning quantity of 1");

                quantity = 1;
            }

            setResourceSprite(resource.resourceSprite);

            setResourceText(quantity.ToString());
        }
    }

    void setResourceSprite(Sprite newSprite)
    {
        SpriteRenderer renderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        renderer.sprite = newSprite;
        renderer.sortingOrder = Mathf.RoundToInt(transform.position.z)*-100+1;
    }


    void setResourceText(string newText) => gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = newText;


    public int getQuantity() => quantity;

    public Resource getResource() => resource;
}
