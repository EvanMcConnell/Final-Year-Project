using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] static GameObject[] roomPrefabs;
    public bool left, right, up, down, done;
    private static Dictionary<string, int> roomTypes = new Dictionary<string, int>()
    {
        {"R", 0},
        {"U", 1},
        {"L", 2},
        {"D", 3},
        {"RU", 4},
        {"LU", 5},
        {"LD", 6},
        {"RD", 7},
        {"RUD", 8},
        {"RLU", 9},
        {"LUD", 10},
        {"RLD", 11},
        {"UD", 12},
        {"RL", 13},
        {"RLUD", 14}
    };
    private string room = "";
    private static RoomPrefabHolder prefabs;

    private void Awake()
    {
        if(prefabs == null)
            prefabs = 
                GameObject.Find("Room Prefab Holder")
                .GetComponent<RoomPrefabHolder>();
    }

    // Start is called before the first frame update
    //IEnumerator Start()
    //{
    //    yield return new WaitForEndOfFrame();
    //    spawnRoom();
    //}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnRoom()
    {
        if (right) room += "R";
        if (left) room += "L";
        if (up) room += "U";
        if (down) room += "D";

        roomTypes.TryGetValue(room, out int roomChoice);

        SpriteRenderer mapSprite = GetComponentInChildren<SpriteRenderer>();
        mapSprite.sprite = prefabs.mapCells[roomChoice];
        if(gameObject.name != "Base")
            mapSprite.enabled = false;

        Instantiate(prefabs.roomShells[roomChoice], this.transform);
    }
}
