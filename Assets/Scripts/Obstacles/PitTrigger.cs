using UnityEngine;
using UnityEngine.Events;

public class PitTrigger : MonoBehaviour
{
    public UnityEvent CollideWithPlayer;

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter" + other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Debug.Log("OnTriggerEnter with Player" + other);
            CollideWithPlayer?.Invoke();
        }
    }

}
