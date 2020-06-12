using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinishTrigger : MonoBehaviour
{
    public MaterialType type;

    public Animation cameraAnim;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.GetComponent<Collector>().BuildWithMats();
            cameraAnim.Play();
        }
    }
}
