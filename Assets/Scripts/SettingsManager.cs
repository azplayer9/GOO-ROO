using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SettingsManager : MonoBehaviour
{
    
    // local variables to save to playerprefs
    public static float bgm_volume = 50f;
    public static float sfx_volume = 50f;
    public static float angle_sensitivity = 3.0f;
    public int skip_tutorial = 0;
    
    // gameObjects referring to settings
    public Slider bgm;
    public Slider sfx;
    public Slider angleSens;
    public Toggle skipTutorial;

    // text referring to settings
    public TextMeshProUGUI bgm_val;
    public TextMeshProUGUI sfx_val;
    public TextMeshProUGUI angleSens_val;

    // Start is called before the first frame update
    void Awake()
    {
        // Initialize PlayerPrefs
        bgm_volume = PlayerPrefs.GetFloat("bgmVol", 50);
        sfx_volume = PlayerPrefs.GetFloat("sfxVol", 50);
        angle_sensitivity = PlayerPrefs.GetFloat("angleSens", 3.0f);
        skip_tutorial = PlayerPrefs.GetInt("skipTutorial", 0);

        // Change Sliders
        bgm.value = bgm_volume;
        sfx.value = sfx_volume;
        angleSens.value = angle_sensitivity;
        
        if(skipTutorial){
            if(skip_tutorial > 0)
            {
                skipTutorial.isOn = true;
            }
            else   
            {
                skipTutorial.isOn = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // save as PlayerPrefs based on slider vals
        PlayerPrefs.SetFloat("bgmVol", bgm.value);
        PlayerPrefs.SetFloat("sfxVol", sfx.value);
        PlayerPrefs.SetFloat("angleSens", angleSens.value);
        PlayerPrefs.SetInt("skipTutorial", skip_tutorial);
        PlayerPrefs.Save();
        
        // Change Text Field based on slider vals
        bgm_val.text = "" + bgm.value;
        sfx_val.text = "" + sfx.value;
        angleSens_val.text = "" + angleSens.value;

        if(skipTutorial){
            if(skipTutorial.isOn)
            {
                skip_tutorial = 1;
            }
            else
            {
                skip_tutorial = 0;
            }
        }
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
