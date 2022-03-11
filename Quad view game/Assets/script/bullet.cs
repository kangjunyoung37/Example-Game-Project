using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public int damage;
    public bool ismelee;
    public bool isrock;
    private void OnCollisionEnter(Collision collision)
    {
      if(!isrock&& collision.gameObject.tag == "floor")
        {
            Destroy(gameObject, 3);

        }


      
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "wall" && !ismelee)
        {
            Debug.Log("detect");
            Destroy(gameObject);


        }
    }


}
