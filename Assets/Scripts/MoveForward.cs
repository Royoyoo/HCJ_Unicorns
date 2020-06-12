using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoveForward : MonoBehaviour
{
    public float Speed;

    void Update()
    {
        this.transform.position += Vector3.forward * Speed * Time.deltaTime;
    }
}