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
        bgm_volume = PlayerPrefs.GetInt("bgmVol", 50);
        sfx_volume = PlayerPrefs.GetInt("sfxVol", 50);
        angle_sensitivity = PlayerPrefs.GetFloat("angleSens", 3.0f);
        
        // Change Text Field
        bgm_val.text = "" + bgm_volume;
        sfx_val.text = "" + sfx_volume;
        angleSens_val.text = "" + angle_sensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        // Update Values
        bgm_volume = bgm.value;
        sfx_volume = sfx.value;
        angle_sensitivity = angleSens.value;

        // save as PlayerPrefs
        PlayerPrefs.SetFloat("bgmVol", bgm_volume);
        PlayerPrefs.SetFloat("sfxVol", sfx_volume);
        PlayerPrefs.SetFloat("angleSens", angle_sensitivity);
        PlayerPrefs.Save();
        
        // Change Text Field
        bgm_val.text = "" + bgm_volume;
        sfx_val.text = "" + sfx_volume;
        angleSens_val.text = "" + angle_sensitivity;
    }
}
