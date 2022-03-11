using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enermy : MonoBehaviour
{
    public int maxheart;
    public int curheart;
    public int score;

    public bool ischase;
    public bool isattack;
    public bool isdead;
    public BoxCollider meleeArea;
    public GameManager manager;
    public enum Type{A,B,C,D}
    public Type type;

    public Transform Target;
    public GameObject missile;
    public Rigidbody rid;
    public BoxCollider box;
    public NavMeshAgent Nav;
    public Animator ani;
    public MeshRenderer[] mat;
    public GameObject[] coins;


    // Start is called before the first frame update

    void Awake()
    {
        rid = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
        mat = GetComponentsInChildren<MeshRenderer>();
        Nav = GetComponent<NavMeshAgent>();
        ani = GetComponentInChildren<Animator>();
        if (type != Type.D)
        {
            Invoke("enemychase", 2f);
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curheart -= weapon.damage;
            Vector3 distance = transform.position - other.transform.position;
            Debug.Log(curheart);
            StartCoroutine(OnDamage(distance, false));
        

        }
        else if (other.tag == "Bullet")
        {
            bullet bullet = other.GetComponent<bullet>();
            curheart -= bullet.damage;
            Vector3 distance = transform.position - other.transform.position;
            Debug.Log(curheart);
            Destroy(other.gameObject);
            StartCoroutine(OnDamage(distance, false));

        }

        
    }

    IEnumerator OnDamage(Vector3 distance , bool isgrenade)
    {

        foreach (MeshRenderer mesh in mat)
            mesh.material.color = Color.red;
        

        yield return new WaitForSeconds(0.1f);
        if (curheart > 0)
        {
            foreach (MeshRenderer mesh in mat)
                mesh.material.color = Color.white;
            

        }
       
        else
        {

            foreach (MeshRenderer mesh in mat)
                mesh.material.color = Color.gray;
            
            gameObject.layer = 12;
            ani.SetTrigger("dodie");
            ischase = false;
            isdead = true;
            Nav.enabled = false;
            Playerscript player = Target.GetComponent<Playerscript>();
            player.score += score;
            int index = Random.Range(0, 3);
            Instantiate(coins[index], transform.position, Quaternion.identity);

            switch (type)
            {
                case Type.A:
                    manager.enemyAcount--;
                    break;
                case Type.B:
                    manager.enemyBcount--;
                    break;
                case Type.C:
                    manager.enemyCcount--;
                    break;
                case Type.D:
                    manager.enemyDcount--;
                    break;
            }



            if (isgrenade)
            {
                distance = distance.normalized;
                distance += Vector3.up * 3;
                rid.freezeRotation = false;

                rid.AddForce(distance * 5, ForceMode.Impulse);
                rid.AddTorque(distance * 15, ForceMode.Impulse);

            }
            else
            {
                distance = distance.normalized;
                distance += Vector3.up;
                rid.AddForce(distance * 5, ForceMode.Impulse);
                
            }


                Destroy(gameObject, 4);

          

        }
    


    }    
    public void HitByGrenade(Vector3 explosionpos)
        {
            curheart = -100;
        Vector3 hitvec = transform.position - explosionpos;
        StartCoroutine(OnDamage(hitvec, true));

        }

    
     
   
    void targeting()
    {
        if(!isdead && type != Type.D) {

            float radius = 0;
            float range = 0;

            switch (type)
            {
                case Type.A:

                    radius = 1.5f;
                    range = 3.0f;

                    break;

                case Type.B:
                    radius = 1f;
                    range = 12f;
                    break;

                case Type.C:
                    radius = 0.5f;
                    range = 20f;
                    break;



            }
            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, radius, transform.forward, range, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isattack)
            {
                StartCoroutine(Attack());

            }

        }

    }

    IEnumerator Attack()
    {     
        
        isattack = true;
        ischase = false;
        ani.SetBool("isattack", true);        
       
       
        switch (type)
        {
            case Type.A:
                
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;

            case Type.B:

                yield return new WaitForSeconds(0.2f);                
                rid.AddForce(transform.forward * 20, ForceMode.Impulse);
                
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                
                rid.velocity = Vector3.zero;
                meleeArea.enabled = false;
                yield return new WaitForSeconds(2f);

                break;

            case Type.C:
                
                yield return new WaitForSeconds(0.2f);
                GameObject Missile = Instantiate(missile, transform.position, transform.rotation);
                Rigidbody ridMis = Missile.GetComponent<Rigidbody>();
                ridMis.AddForce(transform.forward * 10 , ForceMode.Impulse);

                yield return new WaitForSeconds(2f);

                
                break;



        }


        ischase = true;
        isattack = false;
        ani.SetBool("isattack", false);



    }

    void enemychase()
    {
        ischase = true;
        ani.SetBool("iswalk", true);
    }

    void Update()
    {
        if (Nav.enabled && type != Type.D)
        {
            Nav.SetDestination(Target.position);
            Nav.isStopped = !ischase;

        }
       
    }
    void FixedUpdate()
    {

        Freezenemy(); 
        targeting();

    }
    void Freezenemy()
    {
        if (ischase) {
        rid.velocity = Vector3.zero;
        rid.angularVelocity = Vector3.zero;
        }
    }
}
