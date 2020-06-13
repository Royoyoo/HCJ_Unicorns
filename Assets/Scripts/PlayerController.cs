using System;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public enum LineRoute
{
    _1 = 1,
    _2 = 2,
    _3 = 3,
    _4 = 4
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class PlayerController : MonoBehaviour
{
    public BGCurve Route;
    BGCcTrs trs;

    public GameObject Model;

    [Range(0.01f, 10f)]
    public float ChangeRouteSpeed = 5;
    [Range(0.01f, 100f)]
    public float MaxSpeed = 10;
    [Range(0.01f, 10f)]
    public float Acceleration = 5;

    [Range(5f, 60f)]
    public int MaxRotate = 30;
    [Range(0.01f, 10f)]
    public float RotateSpeed = 2;

    public Rocking leftLeg;
    public Rocking rightLeg;

    public LineRoute startRoute = LineRoute._1;

    LineRoute desiredRoute;
    Rigidbody body;

    BoxCollider boxCollider;
    Vector3 colliderStartPosition;

    bool fallIntoPit = false;
    bool collideWithBall = false;

    private float timeAfterBreak = 0;
    Transform[] brokenParts;

    private float startRotationY;
    private float startRotationZ;

    public bool OnRamp;

    public float CurrentSpeed => trs.Speed;
    private Quaternion ModelRotation => Model.transform.localRotation;
    private Vector3 ModelPosition => Model.transform.localPosition;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        colliderStartPosition = boxCollider.center;

        trs = Route.GetComponent<BGCcTrs>();
        trs.Speed = 0;
        
        Model.transform.localPosition = Vector3.zero;
        desiredRoute = startRoute;

        startRotationY = Model.transform.localRotation.eulerAngles.y;
        startRotationZ = Model.transform.localRotation.eulerAngles.z;
    }

    private void FixedUpdate()
    {
        Accelerate();
             
        var inputMove = Input.GetAxis("Horizontal");
        var moveZ = inputMove * ChangeRouteSpeed * Time.fixedDeltaTime;
       
        var currentZ = Model.transform.localPosition.z;
        var newPositionZ = currentZ + moveZ;
        newPositionZ = Mathf.Clamp(newPositionZ, DefineRouteX(LineRoute._1), DefineRouteX(LineRoute._4));
        Model.transform.localPosition = new Vector3(0f, 0f, newPositionZ);
        boxCollider.center = colliderStartPosition + Model.transform.localPosition;
        
        ApplyRotation(inputMove);

        // test
        if (Input.GetKeyDown(KeyCode.R))
        {
            StopRouting();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartRouting();
        }

        leftLeg.speed = CurrentSpeed;
        rightLeg.speed = CurrentSpeed;
        // UpdateBreakParts();
    }

    private void ApplyRotation(float inputMove)
    {
        var koefRotaitonZ = 0.5f;

        var desireRotateY = startRotationY + inputMove * MaxRotate;
        var desireRotateZ = startRotationZ + inputMove * MaxRotate * koefRotaitonZ;

        var currentRotationY = ModelRotation.eulerAngles.y;
        var rotationY = currentRotationY;
        if (currentRotationY != desireRotateY)
        {
            rotationY = Mathf.MoveTowardsAngle(ModelRotation.eulerAngles.y, desireRotateY, RotateSpeed);
        }
        var currentRotationZ = ModelRotation.eulerAngles.z;
        var rotationZ = currentRotationZ;
        if (currentRotationZ != desireRotateZ)
        {
            rotationZ = Mathf.MoveTowardsAngle(ModelRotation.eulerAngles.z, desireRotateZ, RotateSpeed * koefRotaitonZ);
        }

        Model.transform.localRotation = Quaternion.Euler(ModelRotation.eulerAngles.x, rotationY, rotationZ);
    }

    private void Accelerate()
    {
        if (trs.Speed < MaxSpeed)
        {
            trs.Speed += Acceleration * Time.fixedDeltaTime;
        }

        if (Acceleration < 0)
        {
            trs.Speed = Mathf.Max(0f, trs.Speed + Acceleration * Time.fixedDeltaTime);
        }
    }

    private void StopRouting()
    {
        trs.enabled = false;

    }

    private void StartRouting()
    {
        trs.enabled = true;
        trs.Speed = 0;
    }   

    #region яма

    public void FallIntoPit()
    {
        // видимо из-за переключения твердого тела, срабатывает несколько раз этот триггер
        if (fallIntoPit == true)
            return;

        Debug.Log("FallIntoPit");

        fallIntoPit = true;

        StopRouting();

        body.isKinematic = false;
        body.useGravity = true;
        var randomForce = new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value) * 10;
        body.AddForce(randomForce, ForceMode.Force);
        var randomTorque = new Vector3(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value) * 10;
        body.AddTorque(randomTorque, ForceMode.Force);
        //  StartCoroutine(FallIntoPitCor()); 

        StartCoroutine(WaitTimeout(1f, () => ResoreFromPit()));
    }

    public void ResoreFromPit()
    {
        fallIntoPit = false;

        // todo
        var addDistance = 4f;

        Debug.Log("ResoreFromPit");

        trs.Distance += addDistance;

        body.velocity = Vector3.zero;
        body.isKinematic = true;
        body.useGravity = false;

        StartRouting();
        // StartCoroutine(WaitTimeout(1f, () => StartRouting())); 

    }

    #endregion яма

    #region шар на цепи

    public void CollideWithBall(Vector3 forceDirection, Vector3 ballPosition)
    {
        // видимо из-за переключения твердого тела, срабатывает несколько раз этот триггер
        if (collideWithBall == true)
            return;

        Debug.Log("CollideWithBall");

        collideWithBall = true;

        StopRouting();

        Break(ballPosition);

        //body.isKinematic = false;
        //body.useGravity = false;
        // body.AddForce(forceDirection * 1f, ForceMode.Impulse);

        StartCoroutine(WaitTimeout(2f, () => ResoreAfterCollideWithBall()));
    }


    public void ResoreAfterCollideWithBall()
    {
        Model.SetActive(true);
        collideWithBall = false;

        // todo
        var addDistance = 3f;

        Debug.Log("ResoreAfterCollideWithBall");

        trs.Distance += addDistance;

        body.velocity = Vector3.zero;
        body.isKinematic = true;
        body.useGravity = false;

        StartRouting();
        // StartCoroutine(WaitTimeout(1f, () => StartRouting())); 

    }


  
    private void Break(Vector3 ballPosition)
    {
        var brokenModel = Instantiate(Model, this.transform);
        brokenModel.name = Model.name + "_Broken";
        brokenParts = brokenModel.GetComponentsInChildren<Transform>();
        foreach (var item in brokenParts)
        {
            // print("For loop: " + item.name);
            var collider = item.gameObject.AddComponent<BoxCollider>();
            //collider.size *= 0.2f;
            var rb = item.gameObject.AddComponent<Rigidbody>();

            rb.AddForce((Random.insideUnitSphere + Vector3.up) * 5, ForceMode.Impulse);
            //rb.AddExplosionForce(10, ballPosition, 100);

        }
        Destroy(brokenModel, 1f);
        Model.SetActive(false);
    }   

    #endregion шар на цепи

    public IEnumerator WaitTimeout(float timeout, System.Action action)
    {
        var startTime = Time.time;

        while (Time.time < startTime + timeout)
        {
            yield return null;
        }
        action.Invoke();
    }

    public static float DefineRouteX(LineRoute route)
    {
        switch (route)
        {
            case LineRoute._1:
                return -2.25f;  
                
            case LineRoute._2:
                return -0.75f;

            case LineRoute._3:
                return 0.75f;

            case LineRoute._4:
                return 2.25f;

            default:
                return 0;               
        }
    }
}

#region Движение по дорожкам

/*
 private void Update()
    {
        Accelerate();        
      
        // направо 
        if (Input.GetKey(KeyCode.D))
        { 
            desiredRoute = desiredRoute + 1;
            if (desiredRoute > LineRoute._4)
                desiredRoute = LineRoute._4;
        }

        // налево 
        if (Input.GetKey(KeyCode.A))
        {           
            desiredRoute = desiredRoute - 1;

            if (desiredRoute < LineRoute._1)
                desiredRoute = LineRoute._1;
        }

        var currentX = Model.transform.localPosition.x;
        var newPositionX = currentX + moveX;        
        newPositionX = Mathf.Clamp(newPositionX, DefineRouteX(LineRoute._1), DefineRouteX(LineRoute._4));        
        Model.transform.localPosition = new Vector3(newPositionX, 0, 0);
        
        Move(desiredRoute);

        leftLeg.speed = CurrentSpeed;
        rightLeg.speed = CurrentSpeed;       
    }   


     private void Move(LineRoute desiredRoute)
    {
        var currentX = Model.transform.localPosition.x;

        var desiredX = DefineRouteX(desiredRoute);// (int)desiredRoute * Data.Config.RouteDistance;

        var threshold = 0.1f;

        var moveX = 0f;

        if (currentX < desiredX)
            moveX = ChangeRouteSpeed * Time.deltaTime;
        if (currentX > desiredX)
            moveX = -ChangeRouteSpeed * Time.deltaTime;

        var newPositionX = ClampRouteX(desiredRoute, threshold, Model.transform.localPosition.x + moveX);

        //Model.transform.localPosition = new Vector3(newPositionX, Model.transform.localPosition.y, Model.transform.localPosition.z);
        Model.transform.localPosition = new Vector3(newPositionX, 0, 0);
        boxCollider.center = colliderStartPosition + Model.transform.localPosition;
    }

    private float ClampRouteX(LineRoute desiredRoute, float threshold, float newPositionX)
    {
        var desiredX = DefineRouteX(desiredRoute);
        if (newPositionX > desiredX - threshold && newPositionX < desiredX + threshold)
        {
            newPositionX = DefineRouteX(desiredRoute);
        }

        return newPositionX;
    }

 */

#endregion


