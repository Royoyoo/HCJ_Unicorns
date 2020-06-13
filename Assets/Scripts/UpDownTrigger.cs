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

    private BoxCollider collider;

    void Awake()
    {
        collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //  Debug.Log("OnTriggerStay with Player" + other);
        }
    }
}
