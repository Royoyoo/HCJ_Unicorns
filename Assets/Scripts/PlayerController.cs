using BansheeGz.BGSpline.Curve;
using UnityEngine;

public enum LineRoute
{
    Left = -1,
    Center = 0,
    Right = 1,
}

public class PlayerController : MonoBehaviour
{
    public BGCurve Route;
    public GameObject Model;

    [Range(0.01f , 10f)]
    public float ChangeRouteSpeed = 5;

    LineRoute desiredRoute;   

    private void Awake()
    {
        Model.transform.localPosition = Vector3.zero;
        desiredRoute = LineRoute.Center;     
    }

    private void Update()
    {   
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
        Move(desiredRoute);
    }

    private void Move(LineRoute desiredRoute)
    {
        var currentX = Model.transform.localPosition.x;

        var desiredX = (int)desiredRoute * Data.Config.RouteDistance;
              
        var threshold = 0.1f * Data.Config.RouteDistance;
                     
        var moveX = 0f;

        if (currentX < desiredX  )
            moveX = ChangeRouteSpeed * Time.deltaTime;
        if (currentX > desiredX )
            moveX = -ChangeRouteSpeed * Time.deltaTime;

        var newPositionX = Mathf.Clamp(Model.transform.localPosition.x + moveX, -Data.Config.RouteDistance, Data.Config.RouteDistance);
        // округляем около центрального маршрута

        if (desiredRoute == 0 && newPositionX > -threshold && newPositionX < threshold)
        {
            newPositionX = 0f;
        }

        Model.transform.localPosition = new Vector3(newPositionX, Model.transform.localPosition.y, Model.transform.localPosition.z);
    }
}
