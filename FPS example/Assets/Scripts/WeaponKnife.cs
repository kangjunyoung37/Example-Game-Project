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

        onMagazineEvent.Invoke(weaponSetting.currentMagazine); // ���Ⱑ Ȱ��ȭ�� �� ������ źâ ������ �����Ѵ�.
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo); // ���Ⱑ Ȱ��ȭ�� �� ������ ź �� ������ �����Ѵ�.
    }

    private void Awake()
    {
        base.Setup();// ����� ������Ʈ�� �÷��̾� �ִϸ����� ������Ʈ�� �ʱ�ȭ ������
        weaponSetting.currentMagazine = weaponSetting.maxMagazine;
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;

    }
    public override void StartWeaponAction(int type = 0)
    {
        if (isAttack == true) return; // �������̸� �׳� ����
        if (weaponSetting.inAutomaticeAttack == true)//���Ӱ����� �� ���� ��� ������ �ݺ��ϴ� �Լ��� ����
        {
            StartCoroutine("OnAttackLoop", type);
        }
        else//�װ� �ƴ϶�� �׳� ���� �Լ� ����
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
        animator.SetFloat("attackType", type); // ���� ��� ����
        animator.Play("Fire", -1, 0);

        yield return new WaitForEndOfFrame();//1������ ����

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
