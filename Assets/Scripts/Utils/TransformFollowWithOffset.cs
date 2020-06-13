using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollowWithOffset : MonoBehaviour
{
    public float smoothing = 5f;
    
    public Transform target;
    public Vector3 posOffset;
    public Vector3 rotOffset;

    void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, target.position + posOffset, smoothing * Time.deltaTime);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, target.rotation * Quaternion.Euler(rotOffset), smoothing * Time.deltaTime);
    }
}
