using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public dongle lastdongle;
    public GameObject donglePrefab;
    public Transform dongleGroup;
    public GameObject effectPrefab;
    public Transform effectGroup;
    public AudioSource bgmPlayer;
    public AudioSource[] sfxPlayer;
    public AudioClip[] sfxClip;
    public enum Sfx {LevelUp,Next,Attach,Button,Over};
    int sfxCursor;

    public int maxlevel;
    public int score;
    public bool isOver;

    void Awake()
    {
        Application.targetFrameRate = 60; 
    }

    void Start()
    {
        bgmPlayer.Play();
        NextDongle();
    }
    dongle GetDongle()
    {
        GameObject instantEffectObj = Instantiate(effectPrefab, effectGroup);
        ParticleSystem instantEffect= instantEffectObj.GetComponent<ParticleSystem>();

        GameObject instantDongleObj = Instantiate(donglePrefab, dongleGroup);
        dongle instantdongle = instantDongleObj.GetComponent<dongle>();
        instantdongle.effect = instantEffect;
        return instantdongle;

    }
    void NextDongle()
    {
        if (isOver)
        {
            return;
        }
        dongle newDongle = GetDongle();
        lastdongle = newDongle;
        lastdongle.manager = this;
        lastdongle.level = Random.Range(0, maxlevel);
        lastdongle.gameObject.SetActive(true);
        SfxPlay(Sfx.Next);
        StartCoroutine(WaitNext());
    }

    IEnumerator WaitNext()
    {
        while (lastdongle!=null)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2.5f);
        NextDongle();
    }

    public void TouchDown()
    {
        if (lastdongle == null)
            return;
        lastdongle.Drag();
    }
    public void TouchUp()
    {
        if (lastdongle == null)
            return;
        lastdongle.Drop();
        lastdongle = null;
    }

    public void GameOver()
    {
        if (isOver)
        {
            return;
        }
        isOver = true;
        StartCoroutine(GameOverRoutine());


    }
    IEnumerator GameOverRoutine()
    {
        dongle[] dongles = FindObjectsOfType<dongle>();
        for (int index = 0; index < dongles.Length; index++)
        {
            dongles[index].rigid.simulated = false;
        }

        for (int index = 0; index < dongles.Length; index++)
        {
            dongles[index].Hide(Vector3.up * 100);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);
        SfxPlay(Sfx.Over);
    }

    public void SfxPlay(Sfx type)
    {
        switch (type)
        {
            case Sfx.LevelUp:
                sfxPlayer[sfxCursor].clip = sfxClip[Random.Range(0, 3)];
                break;

            case Sfx.Next:
                sfxPlayer[sfxCursor].clip = sfxClip[3];
                break;
            case Sfx.Attach:
                sfxPlayer[sfxCursor].clip = sfxClip[4];
                break;
            case Sfx.Button:
                sfxPlayer[sfxCursor].clip = sfxClip[5];
                break;
            case Sfx.Over:
                sfxPlayer[sfxCursor].clip = sfxClip[6];
                break;


        }
        sfxPlayer[sfxCursor].Play();
        sfxCursor = (sfxCursor + 1) % sfxPlayer.Length;
    }
}
