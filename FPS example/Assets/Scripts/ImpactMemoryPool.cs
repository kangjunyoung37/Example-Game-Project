using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ImpactType {Normal = 0,Obstacle,Enenmy,InteractionObject,}
public class ImpactMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] impactPrefab;
    private MemoryPool[] memoryPool;

    private void Awake()
    {
        memoryPool = new MemoryPool[impactPrefab.Length];
        for(int i =0; i< impactPrefab.Length; ++i)
        {
            memoryPool[i] = new MemoryPool(impactPrefab[i]);
        }
    }
    public void SpawnImpack(RaycastHit hit)
    {
        if (hit.transform.CompareTag("ImpactNormal"))
        {
            OnSpawnImpact(ImpactType.Normal, hit.point, Quaternion.LookRotation(hit.normal));

        }
        else if (hit.transform.CompareTag("ImpactObstacle"))
        {
            OnSpawnImpact(ImpactType.Obstacle, hit.point, Quaternion.LookRotation(hit.normal));

        }
        else if (hit.transform.CompareTag("ImpactEnemy"))
        {
            OnSpawnImpact(ImpactType.Enenmy, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if (hit.transform.CompareTag("InteractionObject"))
        {
            Color color = hit.transform.GetComponentInChildren<MeshRenderer>().material.color;
            OnSpawnImpact(ImpactType.InteractionObject, hit.point, Quaternion.LookRotation(hit.normal),color);
        }
    }
    public void OnSpawnImpact(ImpactType type , Vector3 position,Quaternion rotation, Color color = new Color())
    {
        GameObject item = memoryPool[(int)type].ActivatepoolItem();
        item.transform.position = position;
        item.transform.rotation = rotation;
        item.GetComponent<Impack>().Setup(memoryPool[(int)type]);
        if (type == ImpactType.InteractionObject)
        {
            ParticleSystem.MainModule main = item.GetComponent<ParticleSystem>().main;
            main.startColor = color;
        }
    }

}

