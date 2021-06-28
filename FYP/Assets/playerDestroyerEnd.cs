using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerDestroyerEnd : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Destroy(GameObject.Find("Player Manager").gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
