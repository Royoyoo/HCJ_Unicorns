using UnityEngine;

public class BallChainTrigger : MonoBehaviour
{
  //  public UnityEvent CollideWithPlayer;


    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter " + other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            var player = other.gameObject.GetComponent<PlayerController>();
            var forceDirection = player.transform.position - this.transform.position;            
           // player.CollideWithBall(forceDirection);
            //   CollideWithPlayer?.Invoke();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
           // Debug.Log("OnCollisionEnter " + collision);
            var player = collision.gameObject.GetComponent<PlayerController>();
            var forceDirection = player.transform.position - this.transform.position;
            player.CollideWithBall(forceDirection, transform.position);
          //  CollideWithPlayer?.Invoke();
        }

    }
}
