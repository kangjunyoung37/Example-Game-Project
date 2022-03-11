using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Type { Ammo, Coin, Grenade, Heart, Weapon};
    public Type type;
    public int value;
    SphereCollider sphere;
    Rigidbody rid;


    void Awake()
    {
        rid = GetComponent<Rigidbody>();
        sphere = GetComponent<SphereCollider>();

    }
    void Update()
    {
        transform.Rotate(Vector3.up * 20 *Time.deltaTime);

    }
    
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "floor")
        {
            rid.isKinematic = true;
            sphere.enabled = false;
        }
    }
    
}
