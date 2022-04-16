using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrenadeProjectile : MonoBehaviour
{
    [Header("Explosion Barrel")]
    [SerializeField]
    private GameObject explosionPrefab;//폭발프리펩
    [SerializeField]
    private float explosionRadius = 10.0f;//폭발 범위
    [SerializeField]
    private float explosionForce = 500.0f;// 폭팔 힘
    [SerializeField]
    private float throwForce = 1000.0f; //던지는 힘

    private int explosionDamamge;
    private new Rigidbody rigidbody;

    public void Setup(int damage, Vector3 rotation)
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(rotation * throwForce);//수류탄 던지기

        explosionDamamge = damage;
    }

    private void OnCollisionEnter(Collision collision)//충돌 했을 때
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);//부딪힌 곳에 폭발 프리펩을 실행하겠다.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius); //폭발 구체 범위 안에 있는 모든 오브젝트 정보르 받아옴 
        foreach ( Collider hit in colliders)
        {
            PlayerController player = hit.GetComponent<PlayerController>();
            if(player != null) // 폭발 범위에 플레이어가 있을경우
            {
                player.TakeDamage((int)(explosionDamamge * 0.2f));// 폭발 데미지의 0.2를 곱한 만큼 플레이어가 데미지를 받음
                continue;
            }
            EnermyFSM2 enemy = hit.GetComponent<EnermyFSM2>();
            if(enemy != null)//적일 경우
            {
                enemy.TakeDamage(explosionDamamge);//폭발 데미지만큼 데미지를 받음
                continue;
            }
            InteractionObject interaction = hit.GetComponent<InteractionObject>();
            if(interaction != null)//사물일 경우
            {
                interaction.TakeDamage(explosionDamamge);//폭발 데미지만큼 데미지를 받음
            }
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            if(rigidbody != null)
            {
                rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);//rigidbody 속성을 가진 모든 물체는 폭발 힘을 받아서 날라감

            }
        }
        Destroy(gameObject);
    }


}
