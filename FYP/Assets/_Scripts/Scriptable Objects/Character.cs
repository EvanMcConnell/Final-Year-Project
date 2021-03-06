﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Character : ScriptableObject
{
    public new string name;

    /*
     * - Portraits -
     * 0 - Neutral
     * 1 - Frustrated
     * 2 - Happy
     */
    public Sprite[] portraits;
}
