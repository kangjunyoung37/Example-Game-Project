using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrenade : WeaponBase
{
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipFire;//공격 사운드
    [Header("Grenade")]
    [SerializeField]
    private GameObject grenacePrefab;//수류탄 프리펩
    [SerializeField]
    private Transform grenadeSpawnPoint;//수류탄 생성 위치

    private void OnEnable()//수류탄 무기가 활성화했을 때
    {
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);//탄창정보 갱신
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);//탄 수 정보 갱신
    }
    private void Awake()
    {
        base.Setup();
        weaponSetting.currentMagazine = weaponSetting.maxMagazine;//처음 탄창 수는 최대로 설정
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;//처음 탄 수를 최대로 설정

    }
    public override void StartWeaponAction(int type = 0)
    {
        if(type==0&& isAttack == false && weaponSetting.currentAmmo > 0)// 타입이 0 이고 공격중이 아니고 현재 탄창이 0보다 클 때 실행
        {
            StartCoroutine("OnAttack");
        }
    }
    private IEnumerator OnAttack()
    {
        isAttack = true; // 공격하니까 트루로
        animator.Play("Fire", -1, 0);//공격 애니메이션 재생
        PlaySound(audioClipFire);
        yield return new WaitForEndOfFrame();//1프레임 쉬고
        while (true)
        {
            if(animator.CurrentAnimationIs("Movement"))//현재 애니메이션이 Movement일때 공격 애니메이션이 끝났으니까
            {
                isAttack = false;//공격중을 false로
                yield break;
            }
            yield return null;
        }

    }
    public void SpawnGrenadeProjectile()
    {
        GameObject grenadeClone = Instantiate(grenacePrefab, grenadeSpawnPoint.position, Random.rotation);// 스폰포인트에 수류탄을 생성
        grenadeClone.GetComponent<WeaponGrenadeProjectile>().Setup(weaponSetting.damage, transform.parent.forward);//데미지설정 및 던지는 방향
        weaponSetting.currentAmmo--;
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);//수류탄을 썻으니까 탄 수를 다시 갱신
    }
    public override void IncreaseMagazine(int ammo)
    {
        weaponSetting.currentAmmo = weaponSetting.currentAmmo + ammo > weaponSetting.maxAmmo ? weaponSetting.maxAmmo : weaponSetting.currentAmmo + ammo;
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }
    public override void StopWeaponAction(int type = 0)
    {

    }
    public override void StartReload()
    {

    }
}
