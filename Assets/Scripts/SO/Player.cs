using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "SOs/Player", fileName = "Player")]
public class Player : ScriptableObject
{
    public int matCount;
    public int matsInBuilding;
}