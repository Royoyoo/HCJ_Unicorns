using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] Vector3 rotateSpeed = default;

    //Vector3 targetSpeed;

    //[SerializeField] Vector3 maxSpeed = new Vector3(0f, 0f, 720f);
    //[SerializeField] Vector3 minSpeed = new Vector3(0f, 0f, 180f);

    void Update()
    {
        //rotateSpeed = Vector3.Lerp(rotateSpeed, targetSpeed, 1.5f * Time.deltaTime);
        transform.Rotate(rotateSpeed * Time.deltaTime);
    }

    //public void SetMax()
    //{
    //    targetSpeed = maxSpeed;
    //}

    //public void SetMin()
    //{
    //    targetSpeed = minSpeed;
    //}
}
