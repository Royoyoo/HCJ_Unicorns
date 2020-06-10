using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LookAtCamera : MonoBehaviour
{
    Camera targetCamera = default;

    void Start()
    {
        targetCamera = Camera.main;
        LookAtCam();
    }

    void Update() => LookAtCam();

    void LookAtCam()
    {
        this.transform.rotation = Quaternion.LookRotation((this.transform.position - targetCamera.transform.position).normalized, targetCamera.transform.up);
    }
}
