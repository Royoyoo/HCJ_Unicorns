using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

public class BallChain : MonoBehaviour
{
    [SerializeField]
    private BGCurve Route;
    private BGCcMath math;


    [SerializeField, Range(0f, 1f)]
    private float RouteDistancePositon;

    public GameObject Ball;
    
    public float speed = 5f; //скорость туда-сюда
    public float amplitude = 30; //величина размаха

    private LineRenderer lineRender;


    private void Awake()
    {
        lineRender = GetComponent<LineRenderer>();
        lineRender.SetPosition(0, transform.position);

        math = Route.GetComponent<BGCcMath>();
    }  

    private void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(Mathf.Sin(Time.time * speed) * amplitude, 0, 0));
        lineRender.SetPosition(1, Ball.transform.position);
    }

    private void OnDrawGizmos()
    {
        if (Ball == null)
            return;

        Gizmos.color = Color.red;        
        Gizmos.DrawLine(transform.position, Ball.transform.position);     
    }

   /* [ContextMenu("Расположить на маршурте")]
    private void PlaceOnRoute()
    {
        if (Route != null)
        {
            var bgcMath = Route.GetComponent<BGCcMath>();
            var distance = bgcMath.GetDistance() * RouteDistancePositon;
            var position = bgcMath.CalcPositionAndTangentByDistance(distance, out Vector3 tangent);
            var rotation = Quaternion.LookRotation(tangent);
            transform.position = position - Ball.transform.position;// + Vector3.up *  Ball.transform.localScale.x / 2;
            transform.rotation = rotation;
        }
    }*/

    [ContextMenu("Нарисовать веревку")]
    private void ShowLine()
    {
        if (Ball == null)
            return;

        lineRender = GetComponent<LineRenderer>();
        lineRender.SetPosition(0, transform.position);
        lineRender.SetPosition(1, Ball.transform.position);
    }
}
