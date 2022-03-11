using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    
    public enum Type {Melee,Range};
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer traileffect;
    public Transform bulletPoz;
    public GameObject bullet;
    public Transform bulletcasePoz;
    public GameObject bulletcase;
    public int maxAmmo;
    public int curAmmo;



    public void Use()
    {
        if(type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");

        }
        else if(type == Type.Range && curAmmo > 0)
        {
            curAmmo--;
            StopCoroutine("Shot");
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        traileffect.enabled = true;

        yield return new WaitForSeconds(0.7f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        traileffect.enabled = false;

    }
    IEnumerator Shot()
    {
        GameObject intantBullet = Instantiate(bullet, bulletPoz.position, bulletPoz.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPoz.forward * 50;
        
        yield return null;

        GameObject intantBulletCase = Instantiate(bulletcase, bulletcasePoz.position, bulletcasePoz.rotation);
        Rigidbody bulletcaseRigid = intantBulletCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletcasePoz.forward * Random.Range(-3  , -2) + Vector3.up * Random.Range(-3, -2);
        bulletcaseRigid.AddForce(caseVec, ForceMode.Impulse);
        bulletcaseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);


    }
}
