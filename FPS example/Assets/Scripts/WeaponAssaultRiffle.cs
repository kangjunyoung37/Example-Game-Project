using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }
public class WeaponAssaultRiffle : MonoBehaviour
{

    [HideInInspector]
    public AmmoEvent onAmmoEvnet = new AmmoEvent();

    [Header("Audio Clips")]
    [SerializeField]
    public AudioClip audioClipTakeOutWeapon;
    [SerializeField]
    public AudioClip audioClipFire;

    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting weaponSetting;

    [Header("Spawn Points")]
    [SerializeField]
    private Transform casingSpawnPoint;

    [Header("Fire Effect")]
    [SerializeField]
    private GameObject muzzleFlashEffect;

    private float lastAttackTime = 0;
    
    private AudioSource audioSource;
    private PlayerAnimatorController animator;
    private CasingMemoryPool casingMemoryPool;

    public WeaponName WeaponName => weaponSetting.weaponName;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInParent<PlayerAnimatorController>();
        casingMemoryPool = GetComponent<CasingMemoryPool>();

        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }
    private void OnEnable()
    {
        PlaySound(audioClipTakeOutWeapon);
        muzzleFlashEffect.SetActive(false);
        onAmmoEvnet.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
    }

    // Update is called once per frame
    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();

    }
    public void StartWeaponAction(int type = 0)
    {
        if(type == 0)
        {
            if(weaponSetting.inAutomaticeAttack == true)
            {
                StartCoroutine("OnAttackLoop");
            }
            else
            {
                OnAttack();
            }
        }
    }
    public void StopWeaponAction(int type = 0)
    {
        if(type == 0)
        {
            StopCoroutine("OnAttackLoop");
        }
    }
    private IEnumerator OnAttackLoop()
    {
        while (true)
        {
            OnAttack();
            yield return null;
        }
  
    }
    public void OnAttack()
    {
        if(Time.time - lastAttackTime > weaponSetting.attackRate)
        {
            if(animator.MoveSpeed > 0.5f)
            {
                return;
            }
            lastAttackTime = Time.time;
            if (weaponSetting.currentAmmo <= 0)
            {
                return;
            }
            weaponSetting.currentAmmo--;
            onAmmoEvnet.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);
            animator.Play("Fire", -1, 0);
            StartCoroutine("OnMuzzleFlashEffect");
            PlaySound(audioClipFire);
            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right);
        }
    }
    IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);
        yield return new WaitForSeconds(weaponSetting.attackRate*0.3f);
        muzzleFlashEffect.SetActive(false);
    }
}
