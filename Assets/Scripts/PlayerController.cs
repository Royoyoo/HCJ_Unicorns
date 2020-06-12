using BansheeGz.BGSpline.Curve;
using UnityEngine;

public enum Route
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

    int desiredRoute;

    private void Awake()
    {
        desiredRoute =(int) Model.transform.localPosition.x;
    }

    private void Update()
    {   
        // направо - внутрь круга
        if (Input.GetKeyDown(KeyCode.D))
        {
            desiredRoute = desiredRoute + 1;

            //if (Model.transform.localPosition.x < 1)
            //    Model.transform.localPosition += Vector3.right;
        }

        // налево - наружу круга
        if (Input.GetKeyDown(KeyCode.A))
        {
            desiredRoute = desiredRoute - 1;

            //if (Model.transform.localPosition.x > -1)
            //    Model.transform.localPosition += Vector3.left;
        }
        Move(desiredRoute);
    }

    private void Move(int desiredRoute)
    {
        var currentRoute = Model.transform.localPosition.x;

        desiredRoute = Mathf.Clamp(desiredRoute, -1, 1);

        if (Mathf.Approximately(currentRoute, desiredRoute))
            return;

        var moveX = 0f;

        if (desiredRoute > currentRoute)
            moveX = 1 * ChangeRouteSpeed * Time.deltaTime;
        if (desiredRoute < currentRoute)
            moveX = -1 * ChangeRouteSpeed * Time.deltaTime;


        var newPositionX = Mathf.Clamp(Model.transform.localPosition.x + moveX, -1, 1);
        // округляем около центрального маршрута
        if (desiredRoute == 0 && newPositionX > -0.1f && newPositionX < 0.1f)
        {
            newPositionX = 0f;
        }

        Model.transform.localPosition = new Vector3(newPositionX, Model.transform.localPosition.y, Model.transform.localPosition.z);
    }
}
