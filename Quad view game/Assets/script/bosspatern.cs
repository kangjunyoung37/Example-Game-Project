using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class bosspatern : enermy
{
    public GameObject bossmissile;
    public Transform missilePosA;
    public Transform missilePosB;        
    Vector3 Tauntvec;
    Vector3 lookatvec;
    public bool islook;



    void Awake()
    {
        rid = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
        Nav = GetComponent<NavMeshAgent>();
        ani = GetComponentInChildren<Animator>();
        mat = GetComponentsInChildren<MeshRenderer>();
        Nav.isStopped = true;

        StartCoroutine(Think());
    }

    void Update()
    {


        if (isdead)
        {
            StopAllCoroutines();
            return;
        }
        if (islook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookatvec = new Vector3(h, 0, v) * 5;
            transform.LookAt(Target.position + lookatvec);

        }
        else
            Nav.SetDestination(Tauntvec);
                

}

    IEnumerator Think()

    {
        int patern = Random.Range(0, 5);

        yield return new WaitForSeconds(3f);

        switch (patern)
        {
            case 0:
            case 1:
                StartCoroutine(Shoot());
                break;
            case 2:
            case 3:
                StartCoroutine(RockShoot());
                break;
            case 4:
                StartCoroutine(taunt());

                break;

        }
        
        

        

    }

    IEnumerator Shoot()
    {

        ani.SetTrigger("isshot");
        islook = false;

        GameObject bossMissileA = Instantiate(bossmissile, missilePosA.position, missilePosA.rotation);
        bossmissile gamebossmisle = bossMissileA.GetComponent<bossmissile>();
        gamebossmisle.target = Target;
        yield return new WaitForSeconds(0.2f);
       
        GameObject bossMissileB = Instantiate(bossmissile, missilePosB.position, missilePosA.rotation);
        bossmissile gamebossmisleB = bossMissileB.GetComponent<bossmissile>();
        gamebossmisleB.target = Target;
        yield return new WaitForSeconds(0.3f);

        islook = true;
        yield return new WaitForSeconds(2.0f);

        StartCoroutine(Think());

    }

    IEnumerator RockShoot()
    {
        islook = false;
        ani.SetTrigger("isbigshot");
        GameObject rock = Instantiate(missile, transform.position, transform.rotation);
        yield return new WaitForSeconds(3.0f);
        islook = true;
        StartCoroutine(Think());
    }

    IEnumerator taunt()
    {
        Tauntvec = Target.position + lookatvec;
        ani.SetTrigger("istaunt");
        islook = false;
        Nav.isStopped = false;
        box.enabled = false;
        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;
        
        yield return new WaitForSeconds(1f);
        Nav.isStopped = true;
        islook = true ;
        box.enabled = true;
        StartCoroutine(Think());
    }
}
 