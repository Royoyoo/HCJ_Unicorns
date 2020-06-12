using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoveForward : MonoBehaviour
{
    public float Speed;
    public Rigidbody thisRB;
    
    void FixedUpdate()
    {
        thisRB.position += Vector3.forward * Speed * Time.fixedDeltaTime;
    }
}