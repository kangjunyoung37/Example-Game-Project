using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyMemoryPool : MonoBehaviour
{
    [SerializeField]
    private GameObject enermySpawnPointPrefab;
    [SerializeField]
    private GameObject enermyPrefab;
    [SerializeField]
    private float enermySpawnTime = 1;
    [SerializeField]
    private float enermySpawnLatency = 1;

    private MemoryPool spawnPointMemoryPool;
    private MemoryPool enermyMemoryPool;

    private int numberOfEnermiesSpawnedAtOnce = 1;
    private Vector2Int mapSize = new Vector2Int(100, 100);

    private void Awake()
    {
        spawnPointMemoryPool = new MemoryPool(enermySpawnPointPrefab);
        enermyMemoryPool = new MemoryPool(enermyPrefab);

        StartCoroutine("SpawnTile");
    }

    private IEnumerator SpawnTile()
    {
        int currentNumber = 0;
        int MaxiumNumber = 50;

        while (true)
        {
            for ( int i = 0; i < numberOfEnermiesSpawnedAtOnce; ++i)
            {
                GameObject item = spawnPointMemoryPool.ActivatepoolItem();
                item.transform.position = new Vector3(Random.Range(-mapSize.x * 0.48f, mapSize.x * 0.48f), 1, Random.Range(-mapSize.y * 0.48f, mapSize.y * 0.48f));

                StartCoroutine("SpawnEnermy", item);
            }
            currentNumber++;
            if (currentNumber >= MaxiumNumber)
            {
                currentNumber = 0;
                numberOfEnermiesSpawnedAtOnce++;
            }
            yield return new WaitForSeconds(enermySpawnTime);
        }
    }

    private IEnumerator SpawnEnermy(GameObject point)
    {
        yield return new WaitForSeconds(enermySpawnLatency);
        
        GameObject item = enermyMemoryPool.ActivatepoolItem();
        item.transform.position = point.transform.position;

        spawnPointMemoryPool.DeactivatePoolItem(point);
    }

}
