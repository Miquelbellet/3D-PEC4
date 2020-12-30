using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Slider sliderSounds;
    public Toggle particleSys;
    public Toggle generateZombies;
    public Toggle enableCars;
    public TextMeshProUGUI soundsValuesTxt;

    void Start()
    {
        sliderSounds.value = PlayerPrefs.GetFloat("SoundsVolume", 1);

        if (PlayerPrefs.GetInt("ParticleSystem", 1) == 0) particleSys.isOn = false;
        else particleSys.isOn = true;

        if (PlayerPrefs.GetInt("GenerateZombies", 1) == 0) generateZombies.isOn = false;
        else generateZombies.isOn = true;

        if (PlayerPrefs.GetInt("EnableCars", 1) == 0) enableCars.isOn = false;
        else enableCars.isOn = true;
    }

    void Update()
    {
        
    }

    public void ChangeValueSlider(float val)
    {
        PlayerPrefs.SetFloat("SoundsVolume", val);
        soundsValuesTxt.text = val.ToString("F2");
    }

    public void ChangePS(bool val)
    {
        int num;
        if (val) num = 1;
        else num = 0;
        PlayerPrefs.SetInt("ParticleSystem", num);
    }

    public void ChangeGenerateZombies(bool val)
    {
        int num;
        if (val) num = 1;
        else num = 0;
        PlayerPrefs.SetInt("GenerateZombies", num);
    }

    public void ChangeEnableCars(bool val)
    {
        int num;
        if (val) num = 1;
        else num = 0;
        PlayerPrefs.SetInt("EnableCars", num);
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
