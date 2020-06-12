using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOs/Consts", fileName = "Consts")]
public class Config : ScriptableObject
{
    // расстояние между линиями маршрута
    [Range(1, 10)]
    public int RouteDistance = 1;
}
