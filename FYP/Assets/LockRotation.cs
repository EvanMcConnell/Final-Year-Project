using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    Vector3 targetRotation;
    // Start is called before the first frame update
    void Start() => targetRotation = transform.localEulerAngles;

    // Update is called once per frame
    void Update() => transform.localEulerAngles = targetRotation;
}
