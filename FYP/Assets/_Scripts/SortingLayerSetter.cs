using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerSetter : MonoBehaviour
{
    void Start() => GetComponent<SpriteRenderer>().sortingOrder = Mathf.FloorToInt(transform.position.z*-100);

}
