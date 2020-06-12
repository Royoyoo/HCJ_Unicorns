using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class House : MonoBehaviour
{
    public List<GameObject> blocksGO = new List<GameObject>();

    public void Start()
    {
        foreach (var blockGO in blocksGO)
        {
            blockGO.SetActive(false);
        }
    }

    public void ShowNextBlock()
    {
        foreach (var block in blocksGO)
        {
            if (!block.activeSelf)
            {
                block.SetActive(true);
                block.GetComponent<Animation>().Play();
                break;
            }
        }
    }
}
