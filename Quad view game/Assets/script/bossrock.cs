using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossrock : bullet
{

    Rigidbody rigid;
    float angularpower = 2;
    float scaleValue = 0.1f;
    bool isShoot;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(GainPowerTimer());
        StartCoroutine(GainPower());


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator GainPowerTimer()
    {
        yield return new WaitForSeconds(1.5f);
        isShoot = true;
    }

    IEnumerator GainPower()
    {

        while (!isShoot)
        {
            angularpower += 0.02f;
            scaleValue += 0.0015f;
            transform.localScale = Vector3.one * scaleValue;
            rigid.AddTorque(transform.right * angularpower, ForceMode.Acceleration);
            yield return null;

        }
       
    }

}
