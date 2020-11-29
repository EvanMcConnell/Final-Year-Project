using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] Enemy stats;
    int health;

    private void Start()
    {
        health = stats.maxHealth;
    }

    public Enemy getEnemy()
    {
        return stats;
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        if (health < 1) { Die();  }
    }

    void Die()
    {
        GameObject corpse = new GameObject(stats.name + " corpse", typeof(SpriteRenderer));
        corpse.GetComponent<SpriteRenderer>().sprite = stats.deadSprite;
        corpse.GetComponent<SpriteRenderer>().sortingLayerName = "Character";
        corpse.GetComponent<SpriteRenderer>().sortingOrder = Mathf.FloorToInt(transform.position.y*-100);
        print(stats.name + " died");
        Destroy(gameObject);
    }
}
