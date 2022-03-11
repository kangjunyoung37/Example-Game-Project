using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class myball : MonoBehaviour
{

    Rigidbody rid;
    public float jumpspeed;
    public int itemcount;
    bool jumping = false;
    AudioSource aud;
    public gamManager manager;
    int stage;
    void Awake()
    {
        
        rid = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();
        stage = manager.stage;
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && !jumping)
        {
            rid.AddForce(new Vector3(0, jumpspeed, 0), ForceMode.Impulse);
            jumping = true;
            
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "floor")
        {
            jumping = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "item")

        {   
            aud.Play();
            other.gameObject.SetActive(false);
            itemcount++;
            manager.GetItem(itemcount);
            
        }
        else if(other.gameObject.tag == "Finish")
        {
            if(itemcount == manager.finishitem)
            {
                Debug.Log("완료");
                SceneManager.LoadScene(stage+1);
            }
            else
            {
                Debug.Log("다시 하세요");
                SceneManager.LoadScene(stage);
               
            }


        }
    }


    void FixedUpdate()
    {
        float h;
        float v;

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        rid.AddForce(new Vector3(h,0,v), ForceMode.Impulse);

       
        


    }
}
