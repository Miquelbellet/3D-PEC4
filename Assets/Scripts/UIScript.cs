using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    public float initLife = 100;
    public GameObject lifeBar;
    public GameObject bulletsParent;
    public TextMeshProUGUI ammoTxt;
    public TextMeshProUGUI timerTxt;

    private float timerSec;
    private float timerMin;
    private float timerHours;

    void Start()
    {
        SetLifeBar(initLife);
    }

    void Update()
    {
        timerSec += Time.deltaTime;
        if(timerSec >= 60)
        {
            timerSec = 0;
            timerMin++;
        }
        if (timerMin > 60)
        {
            timerMin = 0;
            timerHours++;
        }
        timerTxt.text = timerHours.ToString("00")+":"+timerMin.ToString("00")+":"+timerSec.ToString("F1");

    }

    public void SetLifeBar(float life)
    {
        var completLifeWidth = lifeBar.transform.GetChild(0).GetComponent<RectTransform>().rect.width;
        var currentLifeWidth = completLifeWidth * life / initLife;
        if (currentLifeWidth <= 0) currentLifeWidth = 0;
        lifeBar.transform.GetChild(0).GetChild(0).transform.GetComponent<RectTransform>().offsetMin = new Vector2(currentLifeWidth, lifeBar.transform.GetChild(0).GetChild(0).transform.GetComponent<RectTransform>().offsetMin.y);
    }

    public void SetAmmo(int currentAmmo, int totalAmmo)
    {
        ammoTxt.text = totalAmmo.ToString();
        for (int i = 0; i < bulletsParent.transform.childCount; i++)
        {
            if (i > currentAmmo - 1) bulletsParent.transform.GetChild(i).gameObject.SetActive(false);
            else bulletsParent.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void ReloadAmmo(int maxAmmo, int totalAmmo)
    {
        ammoTxt.text = totalAmmo.ToString();
        for (int i = 0; i < bulletsParent.transform.childCount; i++)
        {
            if (i > maxAmmo - 1) bulletsParent.transform.GetChild(i).gameObject.SetActive(false);
            else bulletsParent.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
