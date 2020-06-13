using UnityEngine;

public enum UpDownType
{
    None = -1,
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

    private void OnTriggerEnter(Collider other)
    {
        
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            // Debug.Log("OnTriggerStay with Player" + other);               
            player.rampType = Type;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        var player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            // Debug.Log("OnTriggerStay with Player" + other);               
            player.rampType = UpDownType.None;
        }
    }
}
