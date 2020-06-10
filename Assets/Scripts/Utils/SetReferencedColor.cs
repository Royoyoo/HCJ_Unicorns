using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SetReferencedColor : MonoBehaviour
{
#if UNITY_EDITOR

    public ColorReference colorRef;

    public Image image;
    public SpriteRenderer spriteRenderer;
    public TextMeshProUGUI tmpUGUI;
    public TextMeshPro tmp;

    void OnEnable()
    {
        if (!ColorReference.allScripts.Contains(this))
        {
            ColorReference.allScripts.Add(this);
        }

        image = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        tmpUGUI = GetComponent<TextMeshProUGUI>();
        tmp = GetComponent<TextMeshPro>();
        UpdateColor();
    }

    [ContextMenu("Forec Update this Color")]
    public void UpdateColor()
    {
        if (colorRef == null)
        {
            //Debug.LogWarning("ColorRef not set", this.gameObject);
            return;
        }

        if (image != null)
            image.color = colorRef.color;
        
        if (spriteRenderer != null)
            spriteRenderer.color = colorRef.color;
        
        if (tmpUGUI != null)
            tmpUGUI.color = colorRef.color;

        if (tmp != null)
            tmp.color = colorRef.color;
    }

    void OnDestroy()
    {
        ColorReference.allScripts.Remove(this);
    }

    void OnValidate()
    {
        UpdateColor();
    }
#endif
}
