﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{

    public GameObject TitleMenu;
    public GameObject SettingsMenu;
    public bool skipTutorial = false;

    // Start is called before the first frame update
    void Start() {
        TitleMenu.SetActive(true);
        SettingsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update() {}
    
    // start game
    public void StartGame() {
        if(skipTutorial){ // maybe make this a playerprefs so it saves over time
            SceneManager.LoadScene( "Level1" ); // load first level scene
        }
        else {
            SceneManager.LoadScene( "1_Tutorial" ); // load tutorial scene
        }
    }
    
    // open settings menu
    public void OpenSettings() {
        TitleMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }    
    
    // return to title screen
    public void ReturnToTitle() {
        TitleMenu.SetActive(true);
        SettingsMenu.SetActive(false);
    }

}