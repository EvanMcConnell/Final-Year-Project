using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] Enemy stats;
    int health;
    [SerializeField] GameObject hbScalePoint;
    Vector3 hbStartScale;
    Rigidbody rb;
    EnemyStates ai;
    BoxCollider bColl;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        ai = GetComponentInParent<EnemyStates>();
        health = stats.maxHealth;
        hbStartScale = hbScalePoint.transform.localScale;
        bColl = GetComponentInChildren<BoxCollider>();
    }

    public Enemy getEnemy()
    {
        return stats;
    }

    public IEnumerator takeDamage(int damage, Vector3 knockback, float stunDuration)
    {
        health -= damage;

        Vector3 newHBScale = new Vector3(hbStartScale.x * (float)((float)health / (float)stats.maxHealth), hbStartScale.y, hbStartScale.z);
        float bigbrain = (50f/100f);
        print($"{hbStartScale} {newHBScale} {health} {stats.maxHealth} {bigbrain}");
        hbScalePoint.transform.localScale = newHBScale;

        if (health < 1)
        {
            GetComponentInParent<EnemyStates>().die();

        }
        else
        {

            rb.AddForce(knockback, ForceMode.Impulse);
            ai.currentState = EnemyStates.state.idle;
            bColl.isTrigger = false;

            yield return new WaitForSecondsRealtime(stunDuration);

            rb.velocity = new Vector3(0, 0, 0);
            ai.currentState = EnemyStates.state.chase;
            bColl.isTrigger = true;
        }
    }
}
