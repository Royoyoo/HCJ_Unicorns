using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ChangeType
{
    Up,
    Down
}

public class SpeedChange : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10f)]
    private float ChangeValue = 0.5f;

    [SerializeField, Range(0.1f, 10f)]
    private float Time = 1f;

    // [SerializeField]
    //private ChangeType Type = ChangeType.Up;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            //var value = Type == ChangeType.Up ? ChangeValue : -ChangeValue;
            player.ChangeMaxSpeed(ChangeValue, Time);
        }
    }
}
