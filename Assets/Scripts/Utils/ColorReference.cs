using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "SOs/Utils/ColorReference", fileName = "ColorReference")]
public class ColorReference : ScriptableObject
{
#if UNITY_EDITOR

    public Color color = Color.white;

    public static List<SetReferencedColor> allScripts = new List<SetReferencedColor>();

    void OnValidate()
    {
        foreach (var script in allScripts)
        {
            script.UpdateColor();
        }
    }
#endif
}
