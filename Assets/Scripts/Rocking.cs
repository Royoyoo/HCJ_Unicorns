
using UnityEngine;

// Раскачивание как маятник
public class Rocking : MonoBehaviour
{    
    public float speed = 5f; //скорость туда-сюда
    public float amplitude = 30; //величина размаха

    public bool minusSin = true; // sin или -sin
     
    private void Update()
    {
        var x = minusSin ? -Mathf.Sin(Time.time * speed) : Mathf.Sin(Time.time * speed);
        transform.localRotation = Quaternion.Euler(new Vector3(x * amplitude, 0, 0));      
    }  
}
