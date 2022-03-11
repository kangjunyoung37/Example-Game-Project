using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rid;
    void Start()
    {
        rid = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetButtonDown("Jump"))
         {
             rid.AddForce(Vector3.up * 5, ForceMode.Impulse);
         }
        float h = Input.GetAxis("Horizontal") * 0.05f;
        float v = Input.GetAxis("Vertical") * 0.05f;
         Vector3 speed = new Vector3(h,0,v);
         rid.AddForce(speed,ForceMode.Impulse);
  
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.name == "Cube")
        {
            rid.AddForce(Vector3.up*0.5f, ForceMode.Impulse);
        }
    }
}
