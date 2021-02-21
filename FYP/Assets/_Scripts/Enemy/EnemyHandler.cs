using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] Enemy stats;
    int health;
    [SerializeField] GameObject hbScalePoint;
    Vector3 hbStartScale;

    private void Start()
    {
        health = stats.maxHealth;
        hbStartScale = hbScalePoint.transform.localScale;
    }

    public Enemy getEnemy()
    {
        return stats;
    }

    public void takeDamage(int damage)
    {
        health -= damage;

        Vector3 newHBScale = new Vector3(hbStartScale.x * (float)((float)health / (float)stats.maxHealth), hbStartScale.y, hbStartScale.z);
        float bigbrain = (50f/100f);
        print($"{hbStartScale} {newHBScale} {health} {stats.maxHealth} {bigbrain}");
        hbScalePoint.transform.localScale = newHBScale;

        if (health < 1)
            GetComponentInParent<EnemyStates>().die();
    }
}
