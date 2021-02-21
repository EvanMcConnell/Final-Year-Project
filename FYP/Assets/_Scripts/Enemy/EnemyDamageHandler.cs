using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHandler : MonoBehaviour
{
    Enemy attackStats;
    EnemyHandler stats;
    [SerializeField] GameObject attackObject;
    BoxCollider2D attackTrigger;

    void Start()
    {
        //attackTrigger = attackObject.GetComponent<BoxCollider2D>();
        stats = GetComponentInParent<EnemyHandler>();
        attackStats = stats.getEnemy();
        //StartCoroutine(attack());
    }

    public Enemy getAttackStats()
    {
        return attackStats;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<AttackHandler>())
        {
            Weapon attackStats = collision.gameObject.GetComponent<AttackHandler>().getAttackStats();
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            stats.takeDamage(attackStats.damage);
            print("Taking damage from: " + collision.gameObject.name);
        }
    }

    IEnumerator attack()
    {
        attackTrigger.enabled = true;
        //GetComponentInChildren<SpriteRenderer>().enabled = true;
        yield return new WaitForEndOfFrame();
        attackTrigger.enabled = false;
        //GetComponentInChildren<SpriteRenderer>().enabled = false;
        yield return new WaitForSecondsRealtime(2f);
        //StartCoroutine(attack());
    }

}
