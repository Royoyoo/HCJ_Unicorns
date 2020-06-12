using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformFollowWithOffset : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    void Update()
    {
        this.transform.position = target.position + offset;
    }
}
