using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBarrel : InteractionObject
{
    [Header("Explosion Barrel")]
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float explosionDelayTime = 0.3f;
    [SerializeField]
    private float explosionRadius = 10.0f;
    [SerializeField]
    private float explosionForce = 1000.0f;

    private bool isExplode = false;
    public override void TakeDamage(int damage)
    {
        currentHP -= damage;
        if(currentHP<=0 && isExplode == false)
        {
            StartCoroutine("ExplodeBarrel");
        }
    }
    private IEnumerator ExplodeBarrel()
    {
        yield return new WaitForSeconds(explosionDelayTime);

        isExplode = true;

        Bounds bounds = GetComponent<Collider>().bounds;
        Instantiate(explosionPrefab, new Vector3(bounds.center.x, bounds.min.y, bounds.center.z), transform.rotation);
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider hit in colliders)
        {
            PlayerController player = hit.GetComponent<PlayerController>();
            if(player != null)
            {
                player.TakeDamage(50);
                continue;
            }
            EnermyFSM2 enemy = hit.GetComponent<EnermyFSM2>();
            if(enemy != null)
            {
                enemy.TakeDamage(200);
                continue;
            }
            InteractionObject interaction = hit.GetComponent<InteractionObject>();
            if(interaction != null)
            {
                interaction.TakeDamage(200);
            }
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
        Destroy(gameObject);
    }

}
