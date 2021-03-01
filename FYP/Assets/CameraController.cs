using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool followPlayer;
    [SerializeField] Transform player;
    Vector3 playerOffset = new Vector3(0, 0, -10);
    // Start is called before the first frame update
    void Start()
    {
        print(playerOffset);   
    }

    // Update is called once per frame
    void Update()
    {
        if (followPlayer)
            transform.localPosition = player.localPosition + playerOffset;
    }
}
