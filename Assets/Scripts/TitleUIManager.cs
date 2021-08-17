using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{

    public GameObject TitleMenu;
    public GameObject SettingsMenu;
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
        // load last level - if last level == -1, go to tutorial instead
        if(skipTutorial) // make skipTutorial changeable in settings?
        { 
            SceneManager.LoadScene( "Level1" ); // load first level scene
        }
        else 
        {
            SceneManager.LoadScene( "1_Tutorial" ); // load tutorial scene
        }
    }
    
    // open settings menu
    public void OpenSettings() 
    {
        TitleMenu.SetActive(false);
        SettingsMenu.SetActive(true);
    }    
    
    // return to title screen
    public void ReturnToTitle() 
    {
        TitleMenu.SetActive(true);
        SettingsMenu.SetActive(false);
    }

}
