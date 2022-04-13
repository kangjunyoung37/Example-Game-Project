using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnife : WeaponBase
{
    [SerializeField]
    private WeaponKnifeCollider weaponKnifeCollider;
    private void OnEnable()
    {
        isAttack = false;

        onMagazineEvent.Invoke(weaponSetting.currentMagazine); // 무기가 활성화할 때 무기의 탄창 정보를 갱신한다.
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo); // 무기가 활성화할 때 무기의 탄 수 정보를 갱신한다.
    }

    private void Awake()
    {
        base.Setup();// 오디오 컴포넌트와 플레이어 애니메이터 컴포넌트를 초기화 시켜줌
        weaponSetting.currentMagazine = weaponSetting.maxMagazine;
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;

    }
    public override void StartWeaponAction(int type = 0)
    {
        if (isAttack == true) return; // 공격중이면 그냥 리턴
        if (weaponSetting.inAutomaticeAttack == true)//연속공격이 켜 있을 경우 공격을 반복하는 함수를 실행
        {
            StartCoroutine("OnAttackLoop", type);
        }
        else//그게 아니라면 그냥 공격 함수 실행
        {
            StartCoroutine("OnAttack", type);
        }
    }
    public override void StopWeaponAction(int type = 0)
    {
        isAttack = false;
        StopCoroutine("OnAttackLoop");
    }
    public override void StartReload()
    {

    }
    private IEnumerator OnAttackLoop(int type)
    {
        while (true)
        {
            yield return StartCoroutine("OnAttack", type);
        }
    }
    private IEnumerator OnAttack(int type)
    {
        isAttack = true;
        animator.SetFloat("attackType", type); // 공격 모션 선태
        animator.Play("Fire", -1, 0);

        yield return new WaitForEndOfFrame();//1프레임 쉬고

        while (true)
        { 
            if (animator.CurrentAnimationIs("Movement"))
            {
                isAttack = false;
                yield break;
            }
            yield return null;
        }
    }
    public void StartWeaponKnifeCollider()
    {
        weaponKnifeCollider.StartCollider(weaponSetting.damage);
    }
}
