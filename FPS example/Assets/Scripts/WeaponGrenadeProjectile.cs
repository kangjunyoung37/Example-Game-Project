using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrenadeProjectile : MonoBehaviour
{
    [Header("Explosion Barrel")]
    [SerializeField]
    private GameObject explosionPrefab;//����������
    [SerializeField]
    private float explosionRadius = 10.0f;//���� ����
    [SerializeField]
    private float explosionForce = 500.0f;// ���� ��
    [SerializeField]
    private float throwForce = 1000.0f; //������ ��

    private int explosionDamamge;
    private new Rigidbody rigidbody;

    public void Setup(int damage, Vector3 rotation)
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(rotation * throwForce);//����ź ������

        explosionDamamge = damage;
    }

    private void OnCollisionEnter(Collision collision)//�浹 ���� ��
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);//�ε��� ���� ���� �������� �����ϰڴ�.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius); //���� ��ü ���� �ȿ� �ִ� ��� ������Ʈ ������ �޾ƿ� 
        foreach ( Collider hit in colliders)
        {
            PlayerController player = hit.GetComponent<PlayerController>();
            if(player != null) // ���� ������ �÷��̾ �������
            {
                player.TakeDamage((int)(explosionDamamge * 0.2f));// ���� �������� 0.2�� ���� ��ŭ �÷��̾ �������� ����
                continue;
            }
            EnermyFSM2 enemy = hit.GetComponent<EnermyFSM2>();
            if(enemy != null)//���� ���
            {
                enemy.TakeDamage(explosionDamamge);//���� ��������ŭ �������� ����
                continue;
            }
            InteractionObject interaction = hit.GetComponent<InteractionObject>();
            if(interaction != null)//�繰�� ���
            {
                interaction.TakeDamage(explosionDamamge);//���� ��������ŭ �������� ����
            }
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            if(rigidbody != null)
            {
                rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);//rigidbody �Ӽ��� ���� ��� ��ü�� ���� ���� �޾Ƽ� ����

            }
        }
        Destroy(gameObject);
    }


}
