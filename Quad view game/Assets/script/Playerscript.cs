using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerscript : MonoBehaviour
{

    Rigidbody rig;
    Animator animator;
    MeshRenderer[] meshs;
    Vector3 movevec;
    Vector3 dodgevec;
   
    public float speed;
    public float jumpspeed;

    public GameObject[] weapons;
    public bool[] hasweapons;
    public GameObject[] grenade;
    public int hasgrenade;
    public Camera followCamera;
    public GameObject throwGrenade;
    public GameManager manager;
    public int ammo;
    public int coins;
    public int heart;
    public int score;
    public int maxhasgrenade;
    public int maxammo;
    public int maxcoins;
    public int maxheart;

    bool shiftd;
    bool jumpd;
    bool interd;
    bool num1d;
    bool num2d;
    bool num3d;
    bool attakd;
    bool reloadd;
    bool gradd;

    bool isjumping; 
    bool isdodge;
    bool isswap;
    bool isreload;
    bool isFireReady = true;
    bool istouch;
    bool isdamage;
    bool isshop;
    bool isdead;

    GameObject nearObject;
    public Weapon equipweapons;

    float h;
    float v;
    float AttackDelay;
    int equipweaponIndex = -1;
    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        //PlayerPrefs.SetInt("MaxScore", 112500);
        //Debug.Log(PlayerPrefs.GetInt("MaxScore"));

    }

    void Start()
    {
        
    }

    void Update()
    {
        GetInput();
        playermove();
        turn();
        jump();
        dodge();
        Interect();
        swap();
        Attak();
        Reload();
        throwgrenade();
    }

    void GetInput()
    {
        shiftd = Input.GetButton("walk");
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        jumpd = Input.GetButtonDown("Jump");
        interd = Input.GetButtonDown("Interect");
        attakd = Input.GetButton("Fire1");
        gradd = Input.GetButtonDown("Fire2");
        num1d = Input.GetButtonDown("swap1");
        num2d = Input.GetButtonDown("swap2");
        num3d = Input.GetButtonDown("swap3");
        reloadd = Input.GetButtonDown("reload");
        
    }


    void throwgrenade()
    {
        if (hasgrenade == 0)
            return;
        else if(gradd && !isswap && !isreload &&!isdead)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 10;

                GameObject instantgrade = Instantiate(throwGrenade, transform.position, transform.rotation);
                Rigidbody ridgrenage = instantgrade.GetComponent<Rigidbody>();
                ridgrenage.AddForce(nextVec, ForceMode.Impulse);
                ridgrenage.AddTorque(Vector3.back * 10, ForceMode.Impulse);

                hasgrenade--;

                grenade[hasgrenade].SetActive(false);


            }
        }           
    }
    void playermove()
    {
        movevec = new Vector3(h, 0, v).normalized;
        if (isdodge)
            movevec = dodgevec;
        if (isswap || !isFireReady || isreload || isdead)
            movevec = Vector3.zero;
        if(!istouch)
            transform.position = transform.position + movevec * speed * (shiftd ? 0.5f : 1f)* Time.deltaTime;

        animator.SetBool("iswalk", shiftd);
        animator.SetBool("isrun", movevec != Vector3.zero);
    }

    void turn()
    {
        transform.LookAt(transform.position + movevec);

        Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (attakd && !isdead)
        {
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);


            }
        }
    }

    void jump()
    {
        if (jumpd && !isjumping && movevec == Vector3.zero && !isdodge && !isdead)
        {
            rig.AddForce(Vector3.up * 15, ForceMode.Impulse);
            isjumping = true;
            animator.SetBool("isjump", true);
            animator.SetTrigger("dojump");
            
        }
        
    }
    void dodge()
    {
        if (jumpd && !isjumping && movevec != Vector3.zero && !isdodge && !isswap && !isdead)
        {

            isdodge = true;
            speed *= 2;
            animator.SetTrigger("dododge");
            dodgevec = movevec;

            Invoke("dodgeout", 0.6f);
        }

    }
    void dodgeout()
    {
        speed /= 2;
        isdodge = false;
    }
    void Interect()
    {
        if (interd && nearObject != null && !isjumping && !isdodge && !isswap && !isdead)
        {
            if(nearObject.tag == "weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponindex = item.value;
                hasweapons[weaponindex] = true;

                Destroy(nearObject);
            }
            else if(nearObject.tag == "shop")
            {
                shop shopenter = nearObject.GetComponent<shop>();
                shopenter.Enter(this);
                isshop = true;

            }

        }
    }

    void swap()
    {
        int weaponnum = -1;
        if (num1d) weaponnum = 0;
        if (num2d) weaponnum = 1;
        if (num3d) weaponnum = 2;

        if (num1d && (!hasweapons[0] || equipweaponIndex == 0))
            return;
        if (num2d && (!hasweapons[1] || equipweaponIndex == 1))
            return;
        if (num3d && (!hasweapons[2] || equipweaponIndex == 2))
            return;

        if ((num1d || num2d || num3d) && !isdodge && !isjumping && !isdead)
        {
            if (equipweapons != null)
                equipweapons.gameObject.SetActive(false);

            equipweaponIndex = weaponnum;
            equipweapons = weapons[weaponnum].GetComponent<Weapon>();
            equipweapons.gameObject.SetActive(true);
            animator.SetTrigger("doswap");
            isswap = true;
            Invoke("swapout", 0.4f);

        }

    }
    void swapout()
    {
        isswap = false;
    }

    void Attak()
    {
        if(equipweapons == null)
        {
            return;
        }

        AttackDelay += Time.deltaTime;
        isFireReady = equipweapons.rate < AttackDelay;

        if(attakd && isFireReady && !isdodge && !isswap && !isshop && !isdead)
        {
            equipweapons.Use();
            animator.SetTrigger(equipweapons.type == Weapon.Type.Melee ? "doswing" : "doshot");
            AttackDelay = 0;
        }
        
       
    }
    void Reload()
    {
        if (equipweapons == null)
            return;
        if (equipweapons.type == Weapon.Type.Melee)
            return;
        if (ammo == 0)
            return;
        if(reloadd && !isjumping && !isdodge && !isswap && isFireReady && !isdead)
        {
            animator.SetTrigger("doreload");
            isreload = true;

            Invoke("ReloadOut", 3f);
        }
    }
    void ReloadOut()
    {
        int reAmmo = ammo < equipweapons.maxAmmo ? ammo : equipweapons.maxAmmo;
        equipweapons.curAmmo = reAmmo;
        ammo -= reAmmo;
        isreload = false;

    }
    void FixedUpdate()
    {
        freezposition();
        border();
    }
    void freezposition()
    {
        rig.angularVelocity = Vector3.zero;
    }
    void border()
    {
        istouch = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "item")
        {
            Item item = other.GetComponent<Item>();

            switch (item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if (ammo >= maxammo)
                    {
                        ammo = maxammo;
                    }
                    break;

                case Item.Type.Heart:
                    heart += item.value;
                    if (heart >= maxheart)
                    {
                        heart = maxheart;
                    }
                    break;

                case Item.Type.Coin:
                    coins += item.value;
                    if (coins >= maxcoins)
                    {
                        coins = maxcoins;
                    }
                    break;

                case Item.Type.Grenade:
                    grenade[hasgrenade].SetActive(true);
                    hasgrenade += item.value;
                    if (hasgrenade >= maxhasgrenade)
                    {
                        hasgrenade = maxhasgrenade;
                    }
                    break;


            }
            Destroy(other.gameObject);

        }
        else if (other.tag == "enemy bullet")
        {
            
            if (!isdamage ) {
                bullet enemybullet = other.GetComponent<bullet>();
                heart -= enemybullet.damage;
                bool isbossak = other.name == "boss melee Area";

                StartCoroutine(OnDamage(isbossak));
            }
            if (other.GetComponent<Rigidbody>() != null)
            {
                Destroy(other.gameObject);

            }

        }

    }

    IEnumerator OnDamage(bool isbossak)
    {

        isdamage = true;
        

        foreach (MeshRenderer meshRenderer in meshs)
        {
            meshRenderer.material.color = Color.red;
        }
        if (isbossak)
        {
            rig.AddForce(transform.forward * -20, ForceMode.Impulse);
        }
        if (heart <= 0 && !isdead)
        {

            OnDie();
        }
        yield return new WaitForSeconds(1f);
        isdamage = false;
        foreach (MeshRenderer meshRenderer in meshs)
        {
            meshRenderer.material.color = Color.white;
        }

        if(isbossak)
        {
            rig.velocity = Vector3.zero;

        }



    }
    public void OnDie()
    {
        animator.SetTrigger("dodie");
        isdead = true;
        manager.GameOver();

        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "weapon"|| other.tag == "shop")
        {
            nearObject = other.gameObject;
            Debug.Log(nearObject.name);
        }
        
       

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "weapon")
        {
            nearObject = null;
        }
        else if (other.tag == "shop")
        {
            shop shotout = other.GetComponent<shop>();
            shotout.Exit();
            nearObject = null;
            isshop = false;

        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            isjumping = false;
            animator.SetBool("isjump", false);
        }
    }
}
