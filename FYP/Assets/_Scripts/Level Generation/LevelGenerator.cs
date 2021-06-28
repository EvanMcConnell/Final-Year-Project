using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour
{
    //static Vector3 spawnerCheckSize = new Vector3(24, 24, 1);
    public string nextLevelName;
    [SerializeField] int iterations = 1;
    List<GameObject> spawners;
    RoomSpawner currentSpawner;
    [SerializeField] LayerMask roomSpawnerLayer;
    [SerializeField] GameObject roomSpawnerPrefab;
    [SerializeField] int roomOffset = 24;
    float maxX = 0, minX = 0, maxY = 0, minY = 0;

    private void Start()
    {
        //ensure possibleDirections list is empty

        //ensure loop doesn't run too long.
        iterations = Mathf.Abs(iterations);
        if (iterations > 10)
            iterations = 10;

        StartCoroutine(generate());
    }

    IEnumerator generate()
    {
        //print("generation started");
        int completedIterations = 0;
        int roomsSpawned = 0;

        while (completedIterations < iterations)
        {
            spawners = GameObject.FindGameObjectsWithTag("RoomSpawner").ToList();

            //removeCheckedSpawners();
            foreach (GameObject x in spawners.ToList())
            {
                if (x.GetComponent<RoomSpawner>().done)
                    spawners.Remove(x);
            }

            foreach (GameObject x in spawners)
            {
                currentSpawner = x.GetComponent<RoomSpawner>();

                List<int> possibleDirections = new List<int>();

                //add a value to list for each side not marked as an exit
                if (!currentSpawner.up) possibleDirections.Add(1);
                if (!currentSpawner.right) possibleDirections.Add(2);
                if (!currentSpawner.down) possibleDirections.Add(3);
                if (!currentSpawner.left) possibleDirections.Add(4);

                //if all sides of spawner marked as exit, do nothing
                if (possibleDirections.Count == 0)
                {
                    //print("spawner surrounded");
                    continue;
                }
                else
                {

                    int newSpawnerDirection = possibleDirections.ToArray()[Mathf.FloorToInt(Random.Range(0, possibleDirections.Count))];

                    Vector3 newSpawnerPosition;

                    switch (newSpawnerDirection)
                    {
                        case 1:
                            newSpawnerPosition = new Vector3(0, roomOffset, 0) + x.transform.localPosition;
                            //print(x.name + " new spawner above " + x.transform.localPosition);
                            break;

                        case 2:
                            newSpawnerPosition = new Vector3(roomOffset, 0, 0) + x.transform.localPosition;
                            //print(x.name + " new spawner right " + x.transform.localPosition);
                            break;

                        case 3:
                            newSpawnerPosition = new Vector3(0, -roomOffset, 0) + x.transform.localPosition;
                            //print(x.name + " new spawner below " + x.transform.localPosition);
                            break;

                        case 4:
                            newSpawnerPosition = new Vector3(-roomOffset, 0, 0) + x.transform.localPosition;
                            //print(x.name + " new spawner left " + x.transform.localPosition);
                            break;

                        default:
                            newSpawnerPosition = x.transform.localPosition;
                            break;
                    }

                    if (newSpawnerPosition != x.transform.localPosition)
                    {
                        //print("spawn location valid");
                        roomsSpawned++;
                        GameObject newSpawner = Instantiate(roomSpawnerPrefab, this.transform);
                        newSpawner.transform.localPosition = newSpawnerPosition;
                        newSpawner.name = roomsSpawned.ToString();

                        if (newSpawner.transform.position.x > maxX) maxX = newSpawner.transform.position.x;
                        if (newSpawner.transform.position.x < minX) minX = newSpawner.transform.position.x;
                        if (newSpawner.transform.position.z > maxY) maxY = newSpawner.transform.position.z;
                        if (newSpawner.transform.position.z < minY) minY = newSpawner.transform.position.z;

                        RoomSpawner newSpawnerScript = newSpawner.GetComponent<RoomSpawner>();

                        foreach (GameObject y in GameObject.FindGameObjectsWithTag("RoomSpawner"))
                        {
                            
                            if (Physics.Raycast(newSpawner.transform.position + newSpawner.transform.right, newSpawner.transform.right, out RaycastHit rightHit, roomOffset, roomSpawnerLayer))
                            {
                                rightHit.transform.gameObject.GetComponent<RoomSpawner>().left = true;
                                newSpawnerScript.right = true;
                                //print(rightHit.transform.name + " left due to " + newSpawner.name);
                            }
                            if (Physics.Raycast(newSpawner.transform.position - newSpawner.transform.right, -newSpawner.transform.right, out RaycastHit leftHit, roomOffset, roomSpawnerLayer))
                            {
                                leftHit.transform.GetComponent<RoomSpawner>().right = true;
                                newSpawnerScript.left = true;
                                //print(leftHit.transform.name + " right due to " + newSpawner.name);
                            }
                            if (Physics.Raycast(newSpawner.transform.position + newSpawner.transform.up, newSpawner.transform.up, out RaycastHit upHit, roomOffset, roomSpawnerLayer))
                            {
                                upHit.transform.GetComponent<RoomSpawner>().down = true;
                                newSpawnerScript.up = true;
                                //print(upHit.transform.name + " down due to " + newSpawner.name);
                            }
                            if (Physics.Raycast(newSpawner.transform.position - newSpawner.transform.up, -newSpawner.transform.up, out RaycastHit downHit, roomOffset, roomSpawnerLayer))
                            {
                                downHit.transform.GetComponent<RoomSpawner>().up = true;
                                newSpawnerScript.down = true;
                                //print(downHit.transform.name + " up due to " + newSpawner.name);
                            }

                        }
                        //print(x.name + " created " + roomsSpawned);
                    }

                }

                    currentSpawner.done = true;
            }

            completedIterations++;

            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }

        foreach (GameObject x in GameObject.FindGameObjectsWithTag("RoomSpawner"))
            x.GetComponent<RoomSpawner>().spawnRoom();

        GameObject.Find("Map Camera").GetComponent<CameraController>().setMapCameraSize(maxX, minX, maxY, minY);

        GameObject exit = GameObject.Find(roomsSpawned.ToString()).GetComponent<RoomSpawner>().spawnExit();

        exit.name = GameObject.Find("Level Manager").GetComponent<LevelManager>().nextLevelName;


        yield return new WaitForSecondsRealtime(2);

        
        GameObject.Find("Base").transform.GetComponentInChildren<NavMeshSurface>().BuildNavMesh();
    }

    void removeCheckedSpawners()
    {
        foreach (GameObject x in spawners)
        {
            if (x.GetComponent<RoomSpawner>().done)
                spawners.Remove(x);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, 26);
    //}
}
