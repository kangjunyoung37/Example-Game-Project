using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public dongle lastdongle;
    public GameObject donglePrefab;
    public Transform dongleGroup;
    void Awake()
    {
        Application.targetFrameRate = 60; 
    }

    void Start()
    {
        NextDongle();
    }
    dongle GetDongle()
    {
        GameObject instant = Instantiate(donglePrefab, dongleGroup);
        dongle instantdongle = instant.GetComponent<dongle>();
        return instantdongle;

    }
    void NextDongle()
    {
        dongle newDongle = GetDongle();
        lastdongle = newDongle;
        StartCoroutine(WaitNext());
        lastdongle.level = Random.Range(0, 8);
        lastdongle.gameObject.SetActive(true);
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
}
