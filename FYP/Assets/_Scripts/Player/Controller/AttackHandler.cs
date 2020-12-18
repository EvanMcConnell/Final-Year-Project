using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
   [SerializeField] Weapon attackStats;
    [SerializeField] LayerMask hitLayer;
    
    void Update()
    {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(attack());
        }
    }

    IEnumerator attack(){
        //GetComponent<BoxCollider2D>().enabled = true;
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        //yield return new WaitForEndOfFrame();

        /*	initial attempt at replacing attack  trigger
			should be replaced with OverlapArea  Evan 12-12-20 */
        Collider2D[] hits = Physics2D.OverlapAreaAll(transform.position + (Vector3.up / 2), transform.position + Vector3.right + (Vector3.down / 2), hitLayer);

        foreach(Collider2D x in hits)
        {
            x.transform.gameObject.GetComponentInParent<EnemyHandler>().takeDamage(attackStats.damage);
        }

        yield return new WaitForEndOfFrame();

        //yield return new WaitForSecondsRealtime(0.25f);
        //GetComponent<BoxCollider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    public Weapon getAttackStats()
    {
        return attackStats;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + (Vector3.up / 2), transform.position + (Vector3.down / 2));
    }

}
