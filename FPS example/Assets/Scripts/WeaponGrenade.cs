using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrenade : WeaponBase
{
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipFire;//���� ����
    [Header("Grenade")]
    [SerializeField]
    private GameObject grenacePrefab;//����ź ������
    [SerializeField]
    private Transform grenadeSpawnPoint;//����ź ���� ��ġ

    private void OnEnable()//����ź ���Ⱑ Ȱ��ȭ���� ��
    {
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);//źâ���� ����
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);//ź �� ���� ����
    }
    private void Awake()
    {
        base.Setup();
        weaponSetting.currentMagazine = weaponSetting.maxMagazine;//ó�� źâ ���� �ִ�� ����
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;//ó�� ź ���� �ִ�� ����

    }
    public override void StartWeaponAction(int type = 0)
    {
        if(type==0&& isAttack == false && weaponSetting.currentAmmo > 0)// Ÿ���� 0 �̰� �������� �ƴϰ� ���� źâ�� 0���� Ŭ �� ����
        {
            StartCoroutine("OnAttack");
        }
    }
    private IEnumerator OnAttack()
    {
        isAttack = true; // �����ϴϱ� Ʈ���
        animator.Play("Fire", -1, 0);//���� �ִϸ��̼� ���
        PlaySound(audioClipFire);
        yield return new WaitForEndOfFrame();//1������ ����
        while (true)
        {
            if(animator.CurrentAnimationIs("Movement"))//���� �ִϸ��̼��� Movement�϶� ���� �ִϸ��̼��� �������ϱ�
            {
                isAttack = false;//�������� false��
                yield break;
            }
            yield return null;
        }

    }
    public void SpawnGrenadeProjectile()
    {
        GameObject grenadeClone = Instantiate(grenacePrefab, grenadeSpawnPoint.position, Random.rotation);// ��������Ʈ�� ����ź�� ����
        grenadeClone.GetComponent<WeaponGrenadeProjectile>().Setup(weaponSetting.damage, transform.parent.forward);//���������� �� ������ ����
        weaponSetting.currentAmmo--;
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);//����ź�� �����ϱ� ź ���� �ٽ� ����
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
