using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameramove : MonoBehaviour
{
    Transform ballposition;
    Vector3 cameraposition;
    void Awake()
    {
        ballposition = GameObject.FindGameObjectWithTag("Player").transform;
        cameraposition = transform.position - ballposition.position;


    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ballposition.position + cameraposition;


    }
}
