using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public House House;
    
    public void PickUp(MaterialType matType)
    {
        var randomOffset = new Vector3(Random.Range(-0.15f, 0.15f),Random.Range(0f, 0.2f),Random.Range(-0.3f, 0.3f));
        Debug.Log(randomOffset);
        var matGO = Instantiate(matPrefab, randomOffset + matsParent.transform.position, Quaternion.identity, matsParent);
        
        materials.Add(new PickedUpBlock{matType = matType, matGO = matGO});
    }

    public void BuildWithMats()
    {
        StartCoroutine(BuildCor());
    }

    IEnumerator BuildCor()
    {
        yield return new WaitForSeconds(0.5f);
        
        foreach (var block in materials)
        {
            StartCoroutine(ThrowBlockCor(block.matGO));
            yield return new WaitForSeconds(0.3f);
        }
        
        materials.Clear();
    }

    public AnimationCurve flyCurve;
    
    IEnumerator ThrowBlockCor(GameObject blockGO)
    {
        var duration = 0.4f;
        var startTime = Time.time;
        var startPos = blockGO.transform.position;
        
        while (Time.time < startTime + duration)
        {
            var t = (Time.time - startTime) / duration;
            blockGO.transform.position = Vector3.Lerp(startPos, House.transform.position,t) + Vector3.up * 5f * flyCurve.Evaluate(t);
            yield return null;
        }
        
        House.ShowNextBlock();
        Destroy(blockGO);
    }
}
