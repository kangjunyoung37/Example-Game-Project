using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnifeCollider : MonoBehaviour
{

    [SerializeField]
    private ImpactMemoryPool impactMemoryPool;
    [SerializeField]
    private Transform knifeTransform;

    private new Collider collider;
    private int damage;
    private void Awake()
    {
        collider = GetComponent<Collider>();
        collider.enabled = false;
    }

    public void StartCollider(int damage)
    {
        this.damage = damage;
        collider.enabled = true;
        StartCoroutine("DisablebyTime", 0.1f); // ������ ������ 0.1�Ŀ� �ݶ��̴� ��Ȱ��ȭ
    }
    public IEnumerator DisablebyTime(float timte)// ��Ȱ��ȭ�ϴ� �ڵ�
    {
        yield return new WaitForSeconds(timte);
        collider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)//Į�� ��ü�� �ε����� �� ����Ʈ ���� �� ������ ����
    {
        impactMemoryPool.SpawnImpact(other, knifeTransform);
        if (other.CompareTag("ImpactEnemy"))
        {
            other.GetComponent<EnermyFSM2>().TakeDamage(damage);

        }
        if(other.CompareTag("InteractionObject"))
        {
            other.GetComponent<InteractionObject>().TakeDamage(damage);

        }
    }

}
