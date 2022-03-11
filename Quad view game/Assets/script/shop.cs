using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class shop : MonoBehaviour
{
    public RectTransform uiGroup;
    public Animator ani;
    Playerscript enterplayer;
    public GameObject[] item;
    
    public int[] itemprice;
    public Transform[] itempos;
    public Text shoptext;
    public string[] text;



    public void Enter(Playerscript player) 
    {
        enterplayer = player;
        uiGroup.anchoredPosition = Vector3.zero;
        


    }
    public void Exit()
    {

        ani.SetTrigger("dohello");
        uiGroup.anchoredPosition = Vector3.down * 1000;
        



    }

    public void buy(int index)
    {
        
       
        if (enterplayer.coins < itemprice[index])
        {
            

            StartCoroutine(shopping());
            return;
        }
        else
        {
            enterplayer.coins -= itemprice[index];
            Vector3 ranvec = Vector3.right * Random.Range(-3, 3) + Vector3.forward * Random.Range(-3, 3);
            Instantiate(item[index], itempos[index].position + ranvec, itempos[index].rotation);

        }


    }

    IEnumerator shopping()
    {

        shoptext.text = text[0];
        yield return new WaitForSeconds(2f);
        shoptext.text = text[1];
    }

}
