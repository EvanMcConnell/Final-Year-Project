using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Enemy : ScriptableObject
{
    public new string name;
    public int maxHealth;
    public int damage;
    public Sprite deadSprite;
}
