using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{

    float rotatespeed = 40;

    void Update()
    {
        transform.Rotate(Vector3.up * rotatespeed * Time.deltaTime , Space.World);
    } 
}
