using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Playerscript player;
    public bosspatern boss;
    public GameObject gamecamera;
    public GameObject startcamera;
    public GameObject MenuPanel;
    public GameObject GamePanel;
    public GameObject GameOverPanel;

    public GameObject weaponshop;
    public GameObject itemshop;
    public GameObject startzone;
    public GameObject[] Enemy;
    public GameObject bossEnemy;
    public Transform[] enemypos;
    public int enemyAcount;
    public int enemyBcount;
    public int enemyCcount;
    public int enemyDcount;
    public bool isbattle;
    public Text Maxscore;
    public Text userscore;
    public Text userhealth;
    public Text userammo;
    public Text usercoins;
    public Text stagetext;
    public int stage;
    public float playtime;
    public Text playertime;
    public Text enemyA;
    public Text enemyB;
    public Text enemyC;
    public Text BestText;
    public Text curscoreText;
    public Image Hammer;
    public Image HandGun;
    public Image SubmachineGun;
    public Image Grenade;
    public RectTransform bossHealthgroup;
    public RectTransform bossHealthbar;
    public List<int> enermylist; 
    void Awake()
    {
        enermylist = new List<int>();
        Maxscore.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore")); 

    }

    public void GameStart()
    {
        MenuPanel.SetActive(false);
        GamePanel.SetActive(true);
        player.gameObject.SetActive(true);

        startcamera.SetActive(false);
        gamecamera.SetActive(true);

    }

    void Update()
    {
        if (isbattle)
        {
            playtime += Time.deltaTime;
            
        }

    }
    void LateUpdate()
    {

        userscore.text = string.Format("{0:n0}", player.score);
        userhealth.text = player.heart + " / " + player.maxheart;
        if (player.equipweapons == null || player.equipweapons.type == Weapon.Type.Melee)
        {
            userammo.text = "-" + " / " + player.ammo;
        }
        else
        {
            userammo.text = player.equipweapons.curAmmo + " / " + player.ammo;
        }

        usercoins.text = string.Format("{0:n0}", player.coins);


        Hammer.color = new Color(1, 1, 1, ( player.hasweapons[0]? 1:0));
        HandGun.color = new Color(1, 1, 1, (player.hasweapons[1] ? 1 : 0));
        SubmachineGun.color = new Color(1, 1, 1, (player.hasweapons[2] ? 1 : 0));
        Grenade.color = new Color(1, 1, 1, (player.hasgrenade > 0 ? 1 : 0));
        stagetext.text = "STAGE" + stage;
        enemyA.text = enemyAcount.ToString();
        enemyB.text = enemyBcount.ToString();
        enemyC.text = enemyCcount.ToString();

        int hour = (int)playtime / 3600;
        int minute = (int)(playtime - 3600*hour) / 60;
        int seconds = (int)playtime % 60;

        playertime.text = string.Format("{0:00}", hour) +":"+ string.Format("{0:00}", minute) + ":" + string.Format("{0:00}", seconds);
        if (boss != null)
        {
            bossHealthbar.localScale = new Vector3((float)boss.curheart / (float)boss.maxheart, 1, 1);
            bossHealthgroup.anchoredPosition = Vector3.down * 30;

        }
        else
        {
            bossHealthgroup.anchoredPosition = Vector3.up * 100;
        }

    }

    public void StageStart()
    {
        StartCoroutine(Startbattle());
    }

    IEnumerator Endbattle()
    {

        weaponshop.SetActive(true);
        itemshop.SetActive(true);
        startzone.SetActive(true);
        foreach (Transform pos in enemypos)
        {
            pos.gameObject.SetActive(false);
        }
        stage++;
        player.transform.position = Vector3.up * 0.72f;
        isbattle = false;
        yield return null;

        
    }

    IEnumerator Startbattle()
    {
        weaponshop.SetActive(false);
        itemshop.SetActive(false);
        startzone.SetActive(false);
        foreach (Transform pos in enemypos)
        {
            pos.gameObject.SetActive(true);
        }
        isbattle = true;
        StartCoroutine(Enemyon());
        yield return new WaitForSeconds(1f);

    }
    IEnumerator Enemyon()
    {
        
        if(stage % 5 == 0)
        {
            enemyDcount++;
            GameObject instantbossEnemy = Instantiate(bossEnemy, enemypos[0].position, enemypos[0].rotation);
            enermy Enermy = instantbossEnemy.GetComponent<enermy>();
            Enermy.Target = player.transform;
            boss = instantbossEnemy.GetComponent<bosspatern>();
            Enermy.manager = this;


        }
        else
        {
            for (int index = 0; index < stage; index++)
            {

                int rannum = Random.Range(0, 3);
                enermylist.Add(rannum);
                switch (rannum)
                {
                    case 0:
                        enemyAcount++;
                        break;

                    case 1:
                        enemyBcount++;
                        break;

                    case 2:
                        enemyCcount++;
                        break;

                }

            }

            while (enermylist.Count > 0)
            {
                int ranpo = Random.Range(0, 4);

                GameObject instantEnemy = Instantiate(Enemy[enermylist[0]], enemypos[ranpo].position, enemypos[ranpo].rotation);
                enermy Enermy = instantEnemy.GetComponent<enermy>();
                Enermy.Target = player.transform;
                Enermy.manager = this;
                enermylist.RemoveAt(0);
                yield return new WaitForSeconds(1f);
            }
        }
        while (enemyAcount + enemyBcount + enemyCcount + enemyDcount > 0)
        {
            yield return null;

        }
        StartCoroutine(Endbattle());
    }

    public void GameOver()
    {
        GamePanel.SetActive(false);
        GameOverPanel.SetActive(true);
        curscoreText.text = userscore.text;
        
        if(player.score> PlayerPrefs.GetInt("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", player.score);
            BestText.gameObject.SetActive(true);
        }

    }

    public void restart()
    {
        SceneManager.LoadScene(0);

    }




}
