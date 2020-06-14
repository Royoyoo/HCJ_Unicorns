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
    public Transform WorkerPoint;

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

    private float startRotationX;
    private float startRotationY;
    private float startRotationZ;

    public Ramp Ramp;
    public bool OnRamp;
    public UpDownType rampType;   
    public bool CanChangeRoute;

    public float CurrentSpeed => trs.Speed;
    private Quaternion ModelLocalRotation => Model.transform.localRotation;
    private Vector3 ModelLocalPosition { get => Model.transform.localPosition; set => Model.transform.localPosition = value; }

    float threshold = 0.1f;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        colliderStartPosition = boxCollider.center;

        trs = Route.GetComponent<BGCcTrs>();
        trs.Speed = 0;
        
        //Model.transform.localPosition = Vector3.zero;
        desiredRoute = startRoute;

        startRotationX = ModelLocalRotation.eulerAngles.x;
        startRotationY = ModelLocalRotation.eulerAngles.y;
        startRotationZ = ModelLocalRotation.eulerAngles.z;

        rampType = UpDownType.None;
        CanChangeRoute = true;
        Ramp = null;
    }

    private void FixedUpdate()
    {
        Accelerate();
             
        var inputMove = Input.GetAxis("Horizontal");
        // скидываем с рампы, если середина вышла за край
        var force = ForceFromRamp();
        //print(force);
        if (!Mathf.Approximately(force, 0f))
        {
           // print(force);
            inputMove = force * 0.3f;
        }

        var moveZ = inputMove * ChangeRouteSpeed * Time.fixedDeltaTime;      

        var currentZ = Model.transform.localPosition.z;
        var newPositionZ = currentZ + moveZ;
        newPositionZ = Mathf.Clamp(newPositionZ, DefineRouteX(LineRoute._1), DefineRouteX(LineRoute._4));
        var newPosition = new Vector3(ModelLocalPosition.x, ModelLocalPosition.y, newPositionZ);

        var oldPosition = Model.transform.localPosition;
        if (CanApplyNewPosition(newPosition, oldPosition))
        {
            Model.transform.localPosition = newPosition;           
        }

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

        CheckRamp();

        leftLeg.speed = CurrentSpeed;
        rightLeg.speed = CurrentSpeed;
        // UpdateBreakParts();
    }

    private bool CanApplyNewPosition(Vector3 newPosition, Vector3 oldPosition)
    {       
        if (!CanChangeRoute && Ramp != null)
        {
            // еще проверка высоты, когда уже на рампе
            if (ModelLocalPosition.y < 1)
            {
                var colliderCenter = Ramp.ColliderCenter;

                var currentDistance = Vector3.Distance(transform.TransformPoint(ModelLocalPosition), colliderCenter);
                var newDistance = Vector3.Distance(transform.TransformPoint(newPosition), colliderCenter);

                //Debug.DrawLine(transform.TransformPoint(ModelLocalPosition), colliderCenter, Color.blue, 3f);
                //Debug.DrawLine(transform.TransformPoint(newPosition), colliderCenter, Color.red, 3f);           

                // Отдаляемся?
                var movingAway = newDistance > currentDistance;
                // print(currentDistance + "  :  " + newDistance +  " : "  +movingAway);
                if (movingAway)
                    return true;

                return false;
            }  
        }
        return true;
    }

    private float ForceFromRamp()
    {
        if (Ramp != null && ModelLocalPosition.y > 1f)
        {          
            //var rect = new Rect(new Vector2(Ramp.ColliderCenter.x, , Ramp.ColliderSize);
            var contains = Ramp.collider.bounds.Contains(WorkerPoint.position);
           // print(contains);
            if(!contains)
            {    
                var rightPoint = WorkerPoint.position + Vector3.forward;
                var leftPoint = WorkerPoint.position + Vector3.back;

                var colliderCenter =  Ramp.ColliderCenter;
                var rightPointDistance = Vector3.Distance(rightPoint, colliderCenter);
                var leftPointDistance = Vector3.Distance(leftPoint, colliderCenter);
                // если права точка ближе, к рампе, значит рампа справа и нужно двигать влево
                if (rightPointDistance < leftPointDistance)
                    return -1;
                else
                    return 1;
            }
            return 0;           
        }
        return 0;
    }  
    

    private void CheckRamp()
    {
        if (OnRamp == false && Model.transform.position.y > threshold)
        {
          //  print("CheckRamp");
            var downForce = Vector3.down * Physics.gravity.y * 0.5f * Time.deltaTime;
            Model.transform.position -= downForce;
            //print(downForce);
            if (ModelLocalPosition.y < 0f)
            {
                ModelLocalPosition = new Vector3(Model.transform.localPosition.x, 0, Model.transform.localPosition.z);
            }
        }
        
        if(rampType != UpDownType.None)
        {
            var direction = rampType == UpDownType.Up ? Vector3.up : Vector3.down;
            var value = direction * CurrentSpeed * Time.deltaTime;
            ModelLocalPosition += value;
        }        
    }


    private void ApplyRotation(float inputMove)
    {
        var koefRotaitonZ = 0.5f;

        var desireRotateX = startRotationX;
        var desireRotateY = startRotationY + inputMove * MaxRotate;
        var desireRotateZ = startRotationZ + inputMove * MaxRotate * koefRotaitonZ;

        var currentRotationY = ModelLocalRotation.eulerAngles.y;
        var rotationY = currentRotationY;
        if (currentRotationY != desireRotateY)
        {
            rotationY = Mathf.MoveTowardsAngle(ModelLocalRotation.eulerAngles.y, desireRotateY, RotateSpeed);
        }
        var currentRotationZ = ModelLocalRotation.eulerAngles.z;
        var rotationZ = currentRotationZ;
        if (currentRotationZ != desireRotateZ)
        {
            rotationZ = Mathf.MoveTowardsAngle(ModelLocalRotation.eulerAngles.z, desireRotateZ, RotateSpeed * koefRotaitonZ);
        }

        var rotationX = startRotationX;
        var currentRotationX = ModelLocalRotation.eulerAngles.x;
        if (rampType != UpDownType.None)
        {           
            desireRotateX = rampType == UpDownType.Up ? 45 : -45;
        }
        if (currentRotationX != desireRotateX)
        {
            rotationX = Mathf.MoveTowardsAngle(currentRotationX, desireRotateX, RotateSpeed);
        }

        Model.transform.localRotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
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
        var randomForce = new Vector3(Random.value, Random.value, Random.value) * 10;
        body.AddForce(randomForce, ForceMode.Force);
        var randomTorque = new Vector3(Random.value, Random.value, Random.value) * 10;
        body.AddTorque(randomTorque, ForceMode.Force);
        //  StartCoroutine(FallIntoPitCor()); 

        StartCoroutine(WaitTimeout(1f, () => ResoreFromPit()));
    }

    public void ResoreFromPit()
    {
        fallIntoPit = false;

        // todo
        var addDistance = 5f;

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
        var addDistance = 5f;

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

    public IEnumerator WaitTimeout(float timeout, Action action)
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

    private void OnDrawGizmos()
    {
        if (WorkerPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(WorkerPoint.position, 0.3f);

        var rightPoint = WorkerPoint.position + Vector3.forward;
        var leftPoint = WorkerPoint.position + Vector3.back;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(rightPoint, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(leftPoint, 0.1f);
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


