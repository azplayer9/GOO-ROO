﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{

    public GameObject TitleMenu;
    public GameObject SettingsMenu;
    public GameObject CreditsMenu;
    public Texture2D cursorTexture;

    public bool skipTutorial = false;
    public int savedLevel = -1; // save last level completed in player prefs; called in StartGame()

    // Start is called before the first frame update
    void Start() 
    {
        TitleMenu.SetActive(true);
        SettingsMenu.SetActive(false);
    }
    
    // start game (click on play)
    public void StartGame() 
    {
        Destroy(GameObject.FindGameObjectWithTag("Respawn"));
        if(PlayerPrefs.GetInt("skipTutorial") > 0) // make skipTutorial changeable in settings?
        { 
            SceneManager.LoadScene( "Level1" ); // load first level scene
            // change to load last level? - if last level == -1, go to tutorial instead
        }
        else 
        {
            SceneManager.LoadScene( "1_Tutorial" ); // load tutorial scene
        }
    }

    // go to level select scene
    public void GoToLevelSelect()
    {
        SceneManager.LoadScene( "Level0" ); // load tutorial scene
        Destroy(GameObject.FindGameObjectWithTag("Respawn"));
    }
    
    // open settings menu
    public void OpenSettings() 
    {
        TitleMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }    

    public void OpenCredits()
    {
        TitleMenu.SetActive(false);
        SettingsMenu.SetActive(false);
        CreditsMenu.SetActive(true);    
    }
    
    // return to title screen
    public void ReturnToTitle() 
    {
        TitleMenu.SetActive(true);

        CreditsMenu.SetActive(false);    
        SettingsMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
