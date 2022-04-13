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
        StartCoroutine("DisablebyTime", 0.1f); // 공격이 끝난후 0.1후에 콜라이더 비활성화
    }
    public IEnumerator DisablebyTime(float timte)// 비활성화하는 코드
    {
        yield return new WaitForSeconds(timte);
        collider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)//칼이 물체에 부딪혔을 때 임팩트 생성 및 데미지 적용
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
