using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class PickedUpBlock
{
    public MaterialType matType;
    public GameObject matGO;
}

public class Collector : MonoBehaviour
{
    public List<PickedUpBlock> materials = new List<PickedUpBlock>();

    public Transform matsParent;
    public GameObject matPrefab;

    [Range(0.01f, 10f)]
    public float matSize = 1f;

    [Range(0.01f, 1f)]
    public float removePercent = 0.1f;

    public Animation cameraAnim;
    public PlayerController playerController;
    public GameplayUI gameplayUI;
    public Transform matsUITarget;
    public House House;

    private void Start()
    {
        startAcceleration = playerController.Acceleration;
    }

    float currentMatPos = 0;
    public void PickUp(PickUpTrigger pickedTrigger)
    {
        StartCoroutine(MoveMatToUI(pickedTrigger.transform));
        
        var matType = pickedTrigger.type;
        var randomOffset = new Vector3(Random.Range(-0.15f, 0.15f), 0 ,Random.Range(-0.3f, 0.3f));
        var randomRotation = Quaternion.Euler(0, Random.Range(0,90), 0);
        // Debug.Log(randomOffset);
        var matGO = Instantiate(matPrefab, matsParent.transform.position + Vector3.up * currentMatPos, randomRotation, matsParent);
        
        materials.Add(new PickedUpBlock{matType = matType, matGO = matGO});
        currentMatPos += matSize;
    }

    IEnumerator MoveMatToUI(Transform matTransform)
    {
        var duration = 0.5f;
        var startTime = Time.time;
        var startPos = matTransform.position;
        
        while (Time.time < startTime + duration)
        {
            if(matTransform != null)
                matTransform.position = Vector3.Lerp(startPos, matsUITarget.position, (Time.time - startTime) / duration);
            yield return null;
        }
        
        Data.Player.matCount++;
        gameplayUI.PlaySizeUpAnim();
        Destroy(matTransform.gameObject);
    }

    void StopPlayerMove()
    {
        playerController.Acceleration = -1f;
    }

    public float startAcceleration;
    
    void ResumePlayerMove()
    {
        playerController.Acceleration = startAcceleration;
    }
    
    public void BuildWithMats()
    {
        StartCoroutine(BuildCor());
    }

    IEnumerator BuildCor()
    {
        StopPlayerMove();
        cameraAnim.Play("Camera_LookAtHouse");
        yield return new WaitForSeconds(0.5f);
        
        foreach (var block in materials)
        {
            StartCoroutine(ThrowBlockCor(block.matGO));
            yield return new WaitForSeconds(0.3f);
        }
        
        ResumePlayerMove();
        cameraAnim.Play("Camera_LookAtPlayer");
        
        materials.Clear();
    }

    public AnimationCurve flyCurve;
    
    IEnumerator ThrowBlockCor(GameObject blockGO)
    {
        var duration = 0.4f;
        var startTime = Time.time;
        var startPos = blockGO.transform.position;
        
        Data.Player.matCount--;
        
        while (Time.time < startTime + duration)
        {
            var t = (Time.time - startTime) / duration;
            blockGO.transform.position = Vector3.Lerp(startPos, House.transform.position,t) + Vector3.up * 5f * flyCurve.Evaluate(t);
            yield return null;
        }

        Data.Player.matsInBuilding++;
        House.ShowNextBlock();
        Destroy(blockGO);
    }

    public void RemoveMat()
    {
        var removeCount =(int) (materials.Count * removePercent);
        removeCount = Mathf.Clamp(removeCount, 1, materials.Count);
        

        Debug.Log("RemoveMat before = " + materials.Count);

        for (int i = 0; i < removeCount; i++)
        {
            var lastIndex = materials.Count - 1;          
            Destroy(materials[lastIndex].matGO);
            materials.RemoveAt(lastIndex);
        }       

        Debug.Log("RemoveMat after = " + materials.Count);
    }
}
