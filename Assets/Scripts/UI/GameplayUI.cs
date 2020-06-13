using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    public TextMeshProUGUI matCounter;
    public ParticleSystem gainPS;
    
    private void Update()
    {
        matCounter.text = Data.Player.matCount.ToString();
    }

    public void PlaySizeUpAnim()
    {
        StartCoroutine(SizeUpCor());
    }

    IEnumerator SizeUpCor()
    {
        if(gainPS != null)
            gainPS.Play();
        
        var duration = 0.2f;
        var startTime = Time.time;
        
        while (Time.time < startTime + duration)
        {
            matCounter.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, (Time.time - startTime) / duration);
            yield return null;
        }

        matCounter.transform.localScale = Vector3.one;
    }
}
