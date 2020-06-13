using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum MaterialType
{
    Wall, Window, Roof, Base
}

public class PickUpTrigger : MonoBehaviour
{
    public MaterialType type;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<Collector>().PickUp(this);
        }
    }
}
