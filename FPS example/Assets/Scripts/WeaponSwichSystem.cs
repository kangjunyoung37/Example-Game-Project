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
        //1에서 4 사이의 숫자키를 누르면 무기 교체
        int inputIndex = 0;
        if (int.TryParse(Input.inputString,out inputIndex)&&(inputIndex>0 && inputIndex < 5))
        {
            SwitchingWeapon((WeaponType)(inputIndex - 1));
        }
    }
    private void SwitchingWeapon(WeaponType weaponType)
    {
        //교체 가능한 무기가 없으면 종료
        if (weapons[(int)weaponType] == null)
        {
            return;
        }
        //현재 사용중인 무기가 있으면 이전 무기 정보에 저장
        if (currentWeapon != null)
        {
            previousWeapon = currentWeapon;
        }
        //무기교체
        currentWeapon = weapons[(int)weaponType];
        //현재 사용중인 무기교체할려고 하면 리턴
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
