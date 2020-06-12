using UnityEngine;

public class SpawnedObject : MonoBehaviour
{
    [SerializeField]
    private GameObject Model;

    public void SetRandomRoute()
    {
        var randomRoute = Random.Range(-1, 2);
        //Debug.Log(randomRoute);
        Model.transform.localPosition += Vector3.right * randomRoute * Data.Config.RouteDistance;
    }
}
