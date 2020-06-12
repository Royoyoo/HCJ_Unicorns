using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BallChainTrigger : MonoBehaviour
{
    public UnityEvent CollideWithPlayer;


    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter" + other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CollideWithPlayer?.Invoke();
        }
    }

    //public void OnCollisionStay(Collision collision)
    //{
    //    Debug.Log("OnTriggerEnter" + collision);

    //}
}
