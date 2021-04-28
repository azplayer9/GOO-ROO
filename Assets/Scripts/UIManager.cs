using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public Player Goo;
    public Image gooBar;
    public TextMeshProUGUI gooText;
    public Image powerBar;

    public GameManager gameState;
    public GameObject PausedMenu;
    public TextMeshProUGUI PausedText;
    public GameObject SettingsMenu;
    public TextMeshProUGUI victoryText;
    
    
    void Start()
    {
        //victoryText = GameObject.FindWithTag("Victory").GetComponent<TextMeshProUGUI>();
        //victoryText.gameObject.SetActive(false);
    }

    void Update()
    {
        if(!gameState.isPaused) {
            gooBar.fillAmount = Goo.gooMass / 100;      // fill mass UI
            gooText.text = Goo.gooMass + "%";
            gooText.gameObject.SetActive(true);
            powerBar.fillAmount = Goo.jumpPower / 100;  // fill power bar UI
            
            // reset UI display so that unpause->pause always defaults to paused buttons
            ShowPauseButtons();
            PausedMenu.SetActive(false);
        }
        else if (gameState.isVictory){
            victoryText.gameObject.SetActive(true);
        }
        else {     
            PausedMenu.SetActive(true);
        }
    }

    public void ShowPauseButtons() {
        PausedText.gameObject.SetActive(true);
        SettingsMenu.SetActive(false);
    }

    public void ShowSettings() {
        PausedText.gameObject.SetActive(false);
        SettingsMenu.SetActive(true);
    }

}
