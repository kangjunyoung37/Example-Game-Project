using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ani : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    public Rigidbody rigidbody;
    private float h;
    private float v;

    private float moveX;
    private float moveZ;
    private float speedH = 50f;
    private float speedV = 80f;
   


    void Start()

    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.Play("JUMP00");

        }
       
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        moveX = h * speedH * Time.deltaTime;
        moveZ = v * speedV * Time.deltaTime;
         
        if(moveZ <= 0)
        {
            moveX = 0;
        }

        rigidbody.velocity = new Vector3(moveX, 0, moveZ);

        animator.SetFloat("h", h);
        animator.SetFloat("v", v);


   

}  
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Cube")
        {
            Debug.Log("충돌감지");
            animator.Play("DAMAGED01");
            this.transform.Translate(Vector3.back * speedH * Time.deltaTime);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Cube")
        {
            Debug.Log("충돌계속감지");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Cube")
        {
            Debug.Log("충돌종료");
        }
    }

}
