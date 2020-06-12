using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private BGCurve Route;

    [SerializeField, Range(1, 100f)]
    private int SpawnCount = 10;

    [SerializeField]
    private SpawnedObject[] Prefabs;

    private BGCcMath math;
    private float totalDistance;
    private float spawnDistance;

    private void Start()
    {
        math = Route.GetComponent<BGCcMath>();

        //Длина маршрута
        totalDistance = math.GetDistance();
        Debug.Log("totalDistance = " + totalDistance);
        // добавляем +1 , чтобы не было в краях дистанции
        // Пример: дистанция 10, нужно 4 предметов, но не нужно в 0 и в 10;
        // получится как раз в координатах 2 4 6 8
        spawnDistance = totalDistance / (SpawnCount + 1);
        Debug.Log("spawnDistance = " + spawnDistance);
        Spawn();
    }

    // Update is called once per frame
    public void Spawn()
    {
        for (int i = 0; i < SpawnCount; i++)
        {
            // чтобы не заспавнить в 0 дистанции
            var distance = spawnDistance * (i + 1);
            Debug.Log(i + " " + distance);
            // позиция и тангенс на этой дистанции
            // TODO: мб не нужны повороты

            // var positionAtOneMeter = math.CalcByDistance(BGCurveBaseMath.Field.Position, distance);
            var position = math.CalcPositionAndTangentByDistance(distance, out Vector3 tangent);
            var rotation = Quaternion.LookRotation(tangent);
            
            var randomPrefab = Prefabs[Random.Range(0, Prefabs.Length)];
            var spawnedObject = Instantiate(randomPrefab, position, rotation, this.transform);
        }
    }
}
