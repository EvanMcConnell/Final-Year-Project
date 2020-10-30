using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScreenTransitionManager : MonoBehaviour
{
    private enum side { left, right, up, down};
    [SerializeField] private side exitSide;
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject player;
    public Vector3 camNewPosition, playerNewPosition;
    // Start is called before the first frame update
    void Start()
    {
        switch (exitSide)
        {
            case side.left:
                camNewPosition = new Vector3(cam.transform.position.x + (20 * -1), cam.transform.position.y, -10);
                playerNewPosition = new Vector3(player.transform.position.x + (2.5f * -1), player.transform.position.y, 0);
                break;

            case side.right:
                camNewPosition = new Vector3(cam.transform.position.x + (20 * 1), cam.transform.position.y, -10);
                break;

            case side.down:
                camNewPosition = new Vector3(cam.transform.position.x, cam.transform.position.y + (20 * -1), -10);
                break;

            case side.up:
                camNewPosition = new Vector3(cam.transform.position.x, cam.transform.position.y + (20 * 1), -10);
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Transition")
        {
            cam.transform.position = col.gameObject.transform.parent.transform.position + new Vector3(0, 0.5f, -10);
        }
    }
}
