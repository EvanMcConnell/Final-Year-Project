using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
   [SerializeField] Weapon attackStats;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(attack());
        }
    }

    IEnumerator attack(){
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        yield return new WaitForSecondsRealtime(0.25f);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    public Weapon getAttackStats()
    {
        return attackStats;
    }

}
