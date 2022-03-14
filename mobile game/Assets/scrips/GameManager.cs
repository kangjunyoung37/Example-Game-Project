using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    [Header("------------[ Core ]")]
    public int maxlevel;
    public int score;
    public bool isOver;
    [Header("------------[ Object Pooling ]")]
    public GameObject donglePrefab;
    public Transform dongleGroup;
    public List<dongle> donglepool;
    public GameObject effectPrefab;
    public Transform effectGroup;
    public List<ParticleSystem> effectpool;
    [Range(1,30)]
    public int poolSize;
    public int poolCursor;
    public dongle lastdongle;
    [Header("------------[ Audio ]")]
    public AudioSource bgmPlayer;
    public AudioSource[] sfxPlayer;
    public AudioClip[] sfxClip;
    public enum Sfx {LevelUp,Next,Attach,Button,Over};
    int sfxCursor;
    [Header("------------[ UI ]")]
    public GameObject endgroup;
    public GameObject startgroup;
    public Text scoreText;
    public Text maxScoreText;
    public Text subScoreText;

    [Header("------------[ ETC ]")]
    public GameObject line;
    public GameObject bottom;


    void Awake()
    {
        Application.targetFrameRate = 60;
        donglepool = new List<dongle>();
        effectpool = new List<ParticleSystem>();
        for (int index = 0; index < poolSize; index++)
        {
            MakeDongle();
        }
        if (!PlayerPrefs.HasKey("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", 0);
        }
        maxScoreText.text = PlayerPrefs.GetInt("MaxScore").ToString();
    }

    public void GameStart()
    {
        line.SetActive(true);
        bottom.SetActive(true);
        scoreText.gameObject.SetActive(true);
        maxScoreText.gameObject.SetActive(true);
        startgroup.SetActive(false);
       
        bgmPlayer.Play();
        SfxPlay(Sfx.Button);
        Invoke("NextDongle", 1.5f);

    }

    dongle MakeDongle()
    {
        GameObject instantEffectObj = Instantiate(effectPrefab, effectGroup);
        instantEffectObj.name = "Effect" + effectpool.Count;
        ParticleSystem instantEffect = instantEffectObj.GetComponent<ParticleSystem>();
        effectpool.Add(instantEffect);

        GameObject instantDongleObj = Instantiate(donglePrefab, dongleGroup);
        instantDongleObj.name = "dongle" + donglepool.Count;
        dongle instantdongle = instantDongleObj.GetComponent<dongle>();
        instantdongle.manager = this;
        instantdongle.effect = instantEffect;
        donglepool.Add(instantdongle);
        return instantdongle;
    }
    dongle GetDongle()
    {

        for(int index=0; index < donglepool.Count; index++)
        {
            poolCursor = (poolCursor + 1) % donglepool.Count;
            if (!donglepool[poolCursor].gameObject.activeSelf)
            {
                return donglepool[poolCursor];
            }
        }
        return MakeDongle();
    }
    void NextDongle()
    {
        if (isOver)
        {
            return;
        }

        lastdongle = GetDongle();
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

        int maxScore = Mathf.Max(score, PlayerPrefs.GetInt("MaxScore"));
        PlayerPrefs.SetInt("MaxScore", maxScore);

        endgroup.SetActive(true);
        subScoreText.text = "Á¡¼ö : "+scoreText.text;
        bgmPlayer.Stop();
        SfxPlay(Sfx.Over);
    }

    public void Reset()
    {
        SfxPlay(Sfx.Button);
        StartCoroutine(ResetCoroutine());

    }
    IEnumerator ResetCoroutine()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Main");
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
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        } 
    }
    void LateUpdate()
    {
        scoreText.text = score.ToString();
    }
}
