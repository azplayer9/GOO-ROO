using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SettingsManager : MonoBehaviour
{
    
    public static float bgm_volume = 5.0f;
    public static float sfx_volume = 5.0f;
    public static float angle_sensitivity = 3.0f;
    
    public Slider bgm;
    public Slider sfx;
    public Slider angleSens;

    public TextMeshProUGUI bgm_val;
    public TextMeshProUGUI sfx_val;
    public TextMeshProUGUI angleSens_val;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize PlayerPrefs
        bgm_volume = PlayerPrefs.GetInt("bgmVol", 50);
        sfx_volume = PlayerPrefs.GetInt("sfxVol", 50);
        angle_sensitivity = PlayerPrefs.GetFloat("angleSens", 3.0f);
        
        // Change Sliders
        bgm.value = bgm_volume;
        sfx.value = sfx_volume;
        angleSens.value = angle_sensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        // save as PlayerPrefs based on slider vals
        PlayerPrefs.SetFloat("bgmVol", bgm.value);
        PlayerPrefs.SetFloat("sfxVol", sfx.value);
        PlayerPrefs.SetFloat("angleSens", angleSens.value);
        PlayerPrefs.Save();
        
        // Change Text Field based on slider vals
        bgm_val.text = "" + bgm.value;
        sfx_val.text = "" + sfx.value;
        angleSens_val.text = "" + angleSens.value;
    }

    // reset settings to initial values
    public void ResetSettingsToDefault() {
        // Change PlayerPrefs
        //PlayerPrefs.SetFloat("bgmVol", 50);
        //PlayerPrefs.SetFloat("sfxVol", 50);
        //PlayerPrefs.SetFloat("angleSens", 3f);
        //PlayerPrefs.Save();
        
        // Change Sliders -- PlayerPrefs will be saved automatically in Update()
        bgm.value = 50;
        sfx.value = 50;
        angleSens.value = 3;
    }
}
