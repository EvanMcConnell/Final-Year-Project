using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.FloorToInt(transform.position.y*-100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
