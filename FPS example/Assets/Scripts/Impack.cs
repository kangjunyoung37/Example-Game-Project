using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impack : MonoBehaviour
{
    private ParticleSystem particle;
    private MemoryPool memoryPool;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    public void Setup(MemoryPool pool)
    {
        memoryPool = pool;
    }
    private void Update()
    {
        if (particle.isPlaying == false)
        {
            memoryPool.DeactivatePoolItem(gameObject);
        };
    }


}
