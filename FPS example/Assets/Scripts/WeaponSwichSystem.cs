using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwichSystem : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private PlayerHUD playerHUD;

    [SerializeField]
    private WeaponBase[] weapons;

    private WeaponBase currentWeapon;
    private WeaponBase previousWeapon;

    private void Awake()
    {
        playerHUD.SetupAllWeapons(weapons);

        for (int i = 0; i < weapons.Length; ++i)
        {
            if(weapons[i].gameObject != null)
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
        SwitchingWeapon(WeaponType.Main);
        
    }
    private void Update()
    {
        UpdateSwich();
    }
    private void UpdateSwich()
    {
        if (!Input.anyKeyDown) return;
        //1���� 4 ������ ����Ű�� ������ ���� ��ü
        int inputIndex = 0;
        if (int.TryParse(Input.inputString,out inputIndex)&&(inputIndex>0 && inputIndex < 5))
        {
            SwitchingWeapon((WeaponType)(inputIndex - 1));
        }
    }
    private void SwitchingWeapon(WeaponType weaponType)
    {
        //��ü ������ ���Ⱑ ������ ����
        if (weapons[(int)weaponType] == null)
        {
            return;
        }
        //���� ������� ���Ⱑ ������ ���� ���� ������ ����
        if (currentWeapon != null)
        {
            previousWeapon = currentWeapon;
        }
        //���ⱳü
        currentWeapon = weapons[(int)weaponType];
        //���� ������� ���ⱳü�ҷ��� �ϸ� ����
        if(currentWeapon == previousWeapon)
        {
            return;
        }
        playerHUD.SwitchingWeapon(currentWeapon);
        playerController.SwitchingWeapon(currentWeapon);

        if(previousWeapon != null)
        {
            previousWeapon.gameObject.SetActive(false);
        }
        currentWeapon.gameObject.SetActive(true);

    }

}
