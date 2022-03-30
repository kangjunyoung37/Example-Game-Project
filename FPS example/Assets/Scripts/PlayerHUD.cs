using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{

    [Header("Components")]
    [SerializeField]
    private WeaponAssaultRiffle weapon;
    [SerializeField]
    private Status status;

    [Header("Weapon Base")]
    [SerializeField]
    private TextMeshProUGUI textWeaponName;
    [SerializeField]
    private Image imageWeaponIcon;
    [SerializeField]
    private Sprite[] spritesWeaponIcons;

    [Header("Ammo")]
    [SerializeField]
    private TextMeshProUGUI textAmmo;

    [Header("Magazine")]
    [SerializeField]
    private GameObject magaineUIPrefab;
    [SerializeField]
    private Transform magazineParent;
    [Header("HP & BloodScreen UI")]
    [SerializeField]
    private TextMeshProUGUI textHP;
    [SerializeField]
    private Image imageBloodeScreen;
    [SerializeField]
    private AnimationCurve curveBloodScreen;
 


    private List<GameObject> magazineList;



    private void Awake()
    {
        SetupWeapon();
        SetupMagazine();
        weapon.onAmmoEvnet.AddListener(UpdateAmmoHUD);
        weapon.onMagazineEvent.AddListener(UpdateMagazineHUD);
        status.onHPEvent.AddListener(UpdateHPHUD);
    }
    private void SetupWeapon()
    {
        textWeaponName.text = weapon.WeaponName.ToString();
        imageWeaponIcon.sprite = spritesWeaponIcons[(int)weapon.WeaponName];
    }

    private void UpdateAmmoHUD(int currentAmmo, int maxAmmo)
    {
        textAmmo.text = $"<size=40>{currentAmmo}/</size>{maxAmmo}";
    }
    private void SetupMagazine()
    {
        magazineList = new List<GameObject>();
        for (int i = 0; i<weapon.MaxMagazine; ++i)
        {
            GameObject clone = Instantiate(magaineUIPrefab);
            clone.transform.SetParent(magazineParent);
            clone.SetActive(false);

            magazineList.Add(clone);
        }
        for(int i = 0; i < weapon.CurrentMagazine; ++i)
        {
            magazineList[i].SetActive(true);
        }
    }
    private void UpdateMagazineHUD(int currentMagazine)
    {
        for(int i = 0; i<magazineList.Count; ++i)
        {
            magazineList[i].SetActive(false);
        }
        for (int i = 0; i < currentMagazine; ++i)
        {
            magazineList[i].SetActive(true);
        }

    }
    private void UpdateHPHUD(int previous, int current)
    {
        textHP.text = "HP" + current;
        if(previous - current > 0)
        {
            StopCoroutine("OnBloodScreen");
            StartCoroutine("OnBloodScreen");
        }
    }
    private IEnumerator OnBloodScreen()
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime;

            Color color = imageBloodeScreen.color;
            color.a = Mathf.Lerp(1, 0, curveBloodScreen.Evaluate(percent));
            imageBloodeScreen.color = color;

            yield return null;
        }
    }
}
