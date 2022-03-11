using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEnter : MonoBehaviour
{

    public GameManager manager;




    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            manager.StageStart();
        }
    }




}
