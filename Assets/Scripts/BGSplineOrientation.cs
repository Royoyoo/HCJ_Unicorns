using System;
using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BGSplineOrientation : MonoBehaviour
{
    public BGCurve route;
    public BGCcMath math;
    
    void Update()
    {
        var curveDistance = 0f;
        var curvePos = math.CalcPositionByClosestPoint(this.transform.position, out curveDistance);
        var curveTangent = math.CalcTangentByDistance(curveDistance);
        
        Debug.Log($"{curveDistance}, {curvePos}, {curveTangent}");

        transform.rotation = Quaternion.LookRotation(curveTangent);
    }
}
