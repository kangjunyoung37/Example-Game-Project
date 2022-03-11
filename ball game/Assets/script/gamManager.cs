using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gamManager : MonoBehaviour
{
    public int finishitem;
    public int stage;
    public Text playeritemText;
    public Text mapitemText;
    void Awake()
    {
        mapitemText.text = "/ " + finishitem;

    }

    public void GetItem(int count)
    {
        playeritemText.text = count.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(stage);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
