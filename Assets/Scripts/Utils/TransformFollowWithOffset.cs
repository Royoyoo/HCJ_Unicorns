using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollowWithOffset : MonoBehaviour
{
    public float smoothing = 5f;
    
    public Transform target;
   // public Transform subTarget;
    public Vector3 posOffset;
    public Vector3 rotOffset;

    void LateUpdate()
    {
        this.transform.position = target.position + posOffset;
        this.transform.rotation = target.rotation * Quaternion.Euler(rotOffset);

       // if(subTarget.rotation.x!=0)
            //this.transform.rotation += Quaternion

        // this.transform.position = Vector3.Lerp(this.transform.position, target.position + posOffset, smoothing * Time.deltaTime);
        // this.transform.rotation = Quaternion.Slerp(this.transform.rotation, target.rotation * Quaternion.Euler(rotOffset), smoothing * Time.deltaTime);
    }
}
