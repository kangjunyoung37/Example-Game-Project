using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAutoDestroyerByTime : MonoBehaviour
{
    private ParticleSystem particle;
    // Start is called before the first frame update


    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        if(particle.isPaused == false)
        {
            Destroy(gameObject);
        }
    }
}
