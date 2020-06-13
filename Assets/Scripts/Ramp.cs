using UnityEngine;

public class Ramp : MonoBehaviour
{  
    private BoxCollider collider;
    PlayerController player;
        
    void Awake()
    {
        collider = GetComponent<BoxCollider>();     
    }

    private void Update()
    {
        if (player == null)
            return;       
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
          //  Debug.Log("OnTriggerStay with Player" + other);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
           // Debug.Log("OnTriggerEnter with Player" + other);

            player = other.gameObject.GetComponent<PlayerController>();
            player.OnRamp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //  Debug.Log("OnTriggerExit with Player" + other);
            player.OnRamp = false;
            player = null;
           
        }
    }    
}
