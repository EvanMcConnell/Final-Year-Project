using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : Item
{
    public int damage;
    public Resource[] recipe;
    public int[] quantities;
    public RuntimeAnimatorController animatorController;
}