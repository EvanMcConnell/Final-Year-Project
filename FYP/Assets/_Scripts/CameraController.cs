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
        //print(playerOffset);   
    }

    // Update is called once per frame
    void Update()
    {
        if (followPlayer)
            transform.localPosition = player.localPosition + playerOffset;
    }

    public void setMapCameraSize(float maxX, float minX, float maxY, float minY)
    {
        //transform.position = new Vector3((maxX + minX)/2, (maxY + minY)/2, 1);

        //float relativeX = ((maxX - minX) / 16);
        //float relativeY = ((maxY - minY) / 9);

        //if((maxX-minX) > (maxY - minY))
        //    GetComponent<Camera>().orthographicSize = relativeX * 4.5f;
        //else
        //    GetComponent<Camera>().orthographicSize = relativeY * 4.5f;

        //print("Max X: " + maxX + " Min X: " + minX);
        //print("Max Y: " + maxY + " Min Y: " + minY);
        //print("X Length: " + (maxX - minX) + " Y Length: " + (maxY - minY));
        //print("Relative X: " + relativeX + " Relative Y: " + relativeY);
    }
}
