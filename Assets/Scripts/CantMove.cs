using UnityEngine;

public class CantMove : MonoBehaviour
{  
    public BoxCollider collider;
    PlayerController player;


    public Vector3 ColliderCenter => transform.TransformPoint(collider.center);
    public Vector3 ColliderSize => transform.TransformPoint(collider.bounds.size);

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
            if(player != null)
                player.trs.Speed = 0;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player = other.gameObject.GetComponent<PlayerController>();           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {          
            player = null;
        }
    }
}
