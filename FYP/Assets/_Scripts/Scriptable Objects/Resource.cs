﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Resource : ScriptableObject
{
    public string resourceName;
    public Sprite resourceSprite;
    public int value;
}
