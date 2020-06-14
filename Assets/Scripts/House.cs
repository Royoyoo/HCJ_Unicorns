using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class Block
{
    public float matsRate;
    public GameObject go;
}

public class House : MonoBehaviour
{
    public List<Block> blocks = new List<Block>();

    public void Start()
    {
        foreach (var block in blocks)
        {
            block.go.SetActive(false);
        }
    }

    public void CheckForNextBlock()
    {
        var completionRate = (float) Data.Player.matsInBuilding / Data.Config.buildingMatsRequired;
        
        foreach (var block in blocks)
        {
            if (!block.go.activeSelf && completionRate > block.matsRate)
            {
                block.go.SetActive(true);
                block.go.GetComponent<Animation>().Play();

                AudioManager.PlaySound(completionRate < 1 ? SoundType.HouseStageCompleted : SoundType.HouseCompleted);

                break;
            }
        }
    }
}
