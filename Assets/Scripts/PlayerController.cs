using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using UnityEngine;

public enum LineRoute
{
    Left = -1,
    Center = 0,
    Right = 1,
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
    [Range(0.01f, 10f)]
    public float MaxSpeed = 5;
    [Range(0.01f, 10f)]
    public float Acceleration = 5;

    LineRoute desiredRoute;
    Rigidbody body;

    BoxCollider boxCollider;
    Vector3 colliderStartPosition;

    bool fallIntoPit = false;
    bool collideWithBall = false;

    private float timeAfterBreak = 0;
    Transform[] brokenParts;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        colliderStartPosition = boxCollider.center;

        trs = Route.GetComponent<BGCcTrs>();
        trs.Speed = 0;


        Model.transform.localPosition = Vector3.zero;
        desiredRoute = LineRoute.Center;
    }

   
    private void Update()
    {
        Accelerate();

        // направо - внутрь круга
        if (Input.GetKeyDown(KeyCode.D))
        {
            desiredRoute = desiredRoute + 1;
            if (desiredRoute > LineRoute.Right)
                desiredRoute = LineRoute.Right;
        }

        // налево - наружу круга
        if (Input.GetKeyDown(KeyCode.A))
        {
            desiredRoute = desiredRoute - 1;

            if (desiredRoute < LineRoute.Left)
                desiredRoute = LineRoute.Left;
        }

        // test
        if (Input.GetKeyDown(KeyCode.R))
        {
            StopRouting();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartRouting();
        }

        Move(desiredRoute);

       // UpdateBreakParts();
    }

    bool addedColliders;
    private void UpdateBreakParts()
    {
        if (collideWithBall)
        {
            timeAfterBreak += Time.deltaTime;
            if (timeAfterBreak > 0.1f && brokenParts != null && !addedColliders)
            {
                foreach (var item in brokenParts)
                {
                    item.gameObject.AddComponent<BoxCollider>();
                }
                print("addedColliders = " + addedColliders);
                addedColliders = true;
            }
        }
        else
        {
            timeAfterBreak = 0;
            addedColliders = false;
        }
    }

    private void Accelerate()
    {
        if (trs.Speed < MaxSpeed)
        {
            trs.Speed += Acceleration * Time.deltaTime;
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

    private void Move(LineRoute desiredRoute)
    {
        var currentX = Model.transform.localPosition.x;


        var desiredX = (int)desiredRoute * Data.Config.RouteDistance;

        var threshold = 0.1f * Data.Config.RouteDistance;

        var moveX = 0f;

        if (currentX < desiredX)
            moveX = ChangeRouteSpeed * Time.deltaTime;
        if (currentX > desiredX)
            moveX = -ChangeRouteSpeed * Time.deltaTime;

        var newPositionX = Mathf.Clamp(Model.transform.localPosition.x + moveX, -Data.Config.RouteDistance, Data.Config.RouteDistance);
        // округляем около центрального маршрута

        if (desiredRoute == 0 && newPositionX > -threshold && newPositionX < threshold)
        {
            newPositionX = 0f;
        }

        Model.transform.localPosition = new Vector3(newPositionX, Model.transform.localPosition.y, Model.transform.localPosition.z);
        boxCollider.center = colliderStartPosition + Model.transform.localPosition;
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


}
