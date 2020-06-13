using UnityEngine;

public enum UpDownType
{
    Up = 0,
    Down = 1
}

[RequireComponent(typeof(BoxCollider))]
public class UpDownTrigger : MonoBehaviour
{   
    [SerializeField]
    private UpDownType Type = UpDownType.Up;

    //private BoxCollider collider;
    

    //void Awake()
    //{
    //    collider = GetComponent<BoxCollider>();
    //}   

    private void OnTriggerStay(Collider other)
    {
        
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
           // Debug.Log("OnTriggerStay with Player" + other);
            var direction = Type == UpDownType.Up ? Vector3.up : Vector3.down;
            var value = direction *  player.CurrentSpeed * Time.deltaTime;
            player.Ramp(value); 
        }
    }
}
