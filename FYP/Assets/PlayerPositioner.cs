using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositioner : MonoBehaviour
{
    [SerializeField] private Vector3 position;
    [SerializeField] private bool fadeIn = true;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Player position set.", PlayerManager.Instance.gameObject);
        PlayerManager.Instance.setPlayerPostion(position, fadeIn);
    }
}
