using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HouseUI : MonoBehaviour
{
    public SpriteRenderer progressBar;

    public float minBarWidth, maxBarWidth;
    
    void Update()
    {
        progressBar.size = new Vector2(minBarWidth + (float)Data.Player.matsInBuilding / Data.Config.buildingMatsRequired * (maxBarWidth - minBarWidth), progressBar.size.y);
    }
}
