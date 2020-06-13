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
    private bool isPickedUp;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isPickedUp)
        {
            isPickedUp = true;
            other.GetComponent<Collector>().PickUp(this);
        }
    }
}
