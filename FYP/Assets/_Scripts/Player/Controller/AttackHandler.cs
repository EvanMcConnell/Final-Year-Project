using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [SerializeField] Weapon attackStats;
    [SerializeField] LayerMask hitLayer;
    WaitForEndOfFrame nextFrame = new WaitForEndOfFrame();
    Animator anim;
    Collider[] hits;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(attack());
        }
    }

    IEnumerator attack(){
        //GetComponent<BoxCollider2D>().enabled = true;
        //GetComponentInChildren<SpriteRenderer>().enabled = true;
        //yield return new WaitForEndOfFrame();

        /*	initial attempt at replacing attack  trigger
			should be replaced with OverlapArea  Evan 12-12-20 */
        //Collider2D[] hits = Physics2D.OverlapAreaAll(transform.position + (Vector3.up / 2), transform.position + Vector3.right + (Vector3.down / 2), hitLayer);
        //Collider2D[] hits = Physics.OverlapAreaAll(
        //    transform.position + (Vector3.up / 2),
        //    transform.position + Vector3.right + (Vector3.down / 2), 
        //    hitLayer
        //    );

        anim.SetTrigger("Attack");
        print("attacking");

        hits = Physics.OverlapBox(
            transform.position + new Vector3(0.25f, 0, 0),
            new Vector3(0.5f, 1, 10), new Quaternion(), hitLayer);


        foreach (Collider x in hits)
        {
            print("attacked: " + x.gameObject.name);
            x.transform.gameObject.GetComponentInParent<EnemyHandler>().takeDamage(attackStats.damage);
        }

        yield return nextFrame;

        anim.ResetTrigger("Attack");

        //yield return new WaitForSecondsRealtime(0.25f);
        //GetComponent<BoxCollider2D>().enabled = false;
        //GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    public Weapon getAttackStats()
    {
        return attackStats;
    }

    public void OnDrawGizmos()
    {
        //Gizmos.DrawLine(
        //    transform.position + (Vector3.up / 2),
        //    transform.position + (Vector3.down / 2)
        //    );

        //Gizmos.DrawWireCube(
        //    transform.position + new Vector3(0.25f, 0, 0),
        //    new Vector3(0.5f, 1, 10)
        //    );
    }

}
