using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAssaultRiffle : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Audio Clips")]
    [SerializeField]
    public AudioClip audioClipTakeOutWeapon;
    public AudioSource audioSource;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
    }
    private void OnEnable()
    {
        PlaySound(audioClipTakeOutWeapon);
    }

    // Update is called once per frame
    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();

    }
}
