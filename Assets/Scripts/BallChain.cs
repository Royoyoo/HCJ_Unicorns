using UnityEngine;

public class BallChain : MonoBehaviour
{   
    public GameObject Ball;
    
    public float speed = 5f; //скорость туда-сюда
    public float amp = 30; //величина размаха

    private LineRenderer lineRender;

    [ContextMenu("Нарисовать веревку")]
    private void ShowLine()
    {
        if (Ball == null)
            return;

        lineRender = GetComponent<LineRenderer>();
        lineRender.SetPosition(0, transform.position);
        lineRender.SetPosition(1, Ball.transform.position);
    }

    private void Awake()
    {
        lineRender = GetComponent<LineRenderer>();
        lineRender.SetPosition(0, transform.position);
    }


    private void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(Mathf.Sin(Time.time * speed) * amp, 0, 0));
        lineRender.SetPosition(1, Ball.transform.position);
    }

    private void OnDrawGizmos()
    {
        if (Ball == null)
            return;

        Gizmos.color = Color.red;        
        Gizmos.DrawLine(transform.position, Ball.transform.position) ;
    }

  
}
