using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camerascript : MonoBehaviour
{
    public Vector3 offset;
    public Transform playerposition;
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerposition.position + offset;
    }
}
