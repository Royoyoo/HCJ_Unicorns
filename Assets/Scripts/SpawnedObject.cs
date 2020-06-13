using UnityEngine;

public class SpawnedObject : MonoBehaviour
{
    [SerializeField]
    private GameObject Model;

    public void SetRandomRoute()
    {
        var randomRoute = Random.Range((int)LineRoute._1, (int)LineRoute._4 + 1 );
        //Debug.Log(randomRoute);
        Model.transform.localPosition += Vector3.right * randomRoute * PlayerController.DefineRouteX((LineRoute)randomRoute);
    }
}
