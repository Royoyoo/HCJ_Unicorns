using UnityEngine;

public class PitTrigger : MonoBehaviour
{   
    public void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            // Debug.Log("OnTriggerStay with Player" + other);               
            player.FallIntoPit();
        }
    }
}
