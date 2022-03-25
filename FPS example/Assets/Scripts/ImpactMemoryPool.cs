using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ImpactType {Normal = 0,Obstacle,}
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
        if (hit.transform.CompareTag("ImpactObstacle"))
        {
            OnSpawnImpact(ImpactType.Obstacle, hit.point, Quaternion.LookRotation(hit.normal));

        }
    }
    public void OnSpawnImpact(ImpactType type , Vector3 position,Quaternion rotation)
    {
        GameObject item = memoryPool[(int)type].ActivatepoolItem();
        item.transform.position = position;
        item.transform.rotation = rotation;
        item.GetComponent<Impack>().Setup(memoryPool[(int)type]);
    }

}

