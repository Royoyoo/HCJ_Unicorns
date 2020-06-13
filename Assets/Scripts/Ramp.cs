using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ramp : MonoBehaviour
{
    //[SerializeField]
    //private float pDownSpeed = 


    [SerializeField] private Transform upStart;
    [SerializeField] private Transform upEnd;

    [SerializeField] private Transform downStart;
    [SerializeField] private Transform downEnd;

    private BoxCollider collider;
    PlayerController player;

    Rect upRect;
    Rect downRect;

    void Awake()
    {
        collider = GetComponent<BoxCollider>();

        var upPos = new Vector2((upStart.position.x + upEnd.position.x) / 2, (upStart.position.z + upEnd.position.z) / 2);
        var upSize = new Vector2(Mathf.Abs(downStart.position.x - downEnd.position.x), Mathf.Abs(downStart.position.z - downEnd.position.z));
        upRect = new Rect(upPos, upSize);

        var downPos = new Vector2((downStart.position.x + downEnd.position.x) / 2, (downStart.position.z + downEnd.position.z) / 2);
        var downSize = new Vector2(Mathf.Abs(downStart.position.x - downEnd.position.x), Mathf.Abs(downStart.position.z - downEnd.position.z));
        downRect = new Rect(downPos, downSize);
    }

    private void Update()
    {
        if (player == null)
            return;
               
        var playerPoint = new Vector2(player.transform.position.x, player.transform.position.z);
        if (upRect.Contains(playerPoint))
        {
           // print(upRect);
            player.Model.transform.localPosition += Vector3.up * player.CurrentSpeed * Time.deltaTime;          
        }

        if (downRect.Contains(playerPoint))
        {
           // print(downRect);
            player.Model.transform.localPosition += Vector3.down * player.CurrentSpeed * Time.deltaTime;
        }
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

    private void OnDrawGizmos()
    {
        if (upStart == null || upEnd == null || downStart == null || downEnd == null)
            return;          

        var upCentre = (upStart.position + upEnd.position) / 2;
        var upSize = new Vector3(upStart.position.x - upEnd.position.x, upStart.position.y- upEnd.position.y, upStart.position.z - upEnd.position.z);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube( upCentre, upSize);

        var downCentre = (downStart.position + downEnd.position) / 2;
        var downSize = new Vector3(downStart.position.x - downEnd.position.x, downStart.position.y - downEnd.position.y, downStart.position.z - downEnd.position.z);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(downCentre, downSize);      
    }
}
