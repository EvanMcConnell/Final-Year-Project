using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHandler : MonoBehaviour
{
    void Start() => GetComponent<SpriteRenderer>().sortingOrder = Mathf.FloorToInt(transform.position.z*-100);

}
