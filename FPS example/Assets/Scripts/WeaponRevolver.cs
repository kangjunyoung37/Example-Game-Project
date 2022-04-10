using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRevolver : WeaponBase
{
    [Header("Fire Effects")]
    [SerializeField]
    private GameObject muzzleFlashEffect;

    [Header("spawn Points")]
    [SerializeField]
    private Transform bulletSpawnPoint;
    public override void StartWeaponAction(int type = 0)
    {
       
    }
    public override void StopWeaponAction(int type = 0)
    {
   
    }
    public override void StartReload()
    {
     
    }
}
