﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    Player Goo;
    public Image gooBar;
    public TextMeshProUGUI gooText;
    public Image powerBar;

    public GameManager gameState;
    public GameObject PausedMenu;
    public TextMeshProUGUI PausedText;
    public GameObject SettingsMenu;
    public TextMeshProUGUI victoryText;

    private bool victorious = false;
    //public TextMeshProUGUI defeatText;
    
    
    void Start()
    {
        //victoryText = GameObject.FindWithTag("Victory").GetComponent<TextMeshProUGUI>();
        //victoryText.gameObject.SetActive(false);
        this.Goo = FindObjectOfType<Player>();
    }

    void Update()
    {
        // code to run if game is not paused
        if(!gameState.isPaused) 
        {
            gooBar.fillAmount = Goo.gooMass / 100;      // fill mass UI
            gooText.text = Goo.gooMass + "%";
            gooText.gameObject.SetActive(true);
            powerBar.fillAmount = Goo.jumpPower / Player.jumpPowerMax;  // fill power bar UI
            
            // reset UI display so that unpause->pause always defaults to paused buttons
            ShowPauseButtons();
            PausedMenu.SetActive(false);
        }
        // if game is paused, show the pause menu
        else 
        {     
            PausedMenu.SetActive(true);
        }

        // check to play victory text
        if (gameState.isVictory && !victorious)
        {
            StartCoroutine("HandleVictory");
        }
        else if(gameState.isDefeat)
        {
            //defeatText.gameObject.SetActive(true);
            gameState.RestartLevel(0.5f);
        }
        
    }

    public IEnumerator HandleVictory()
    {
        victorious = true;
        victoryText.gameObject.SetActive(true);

        Goo.anim.Play("Eat");
        
        yield return new WaitForSeconds(.85f);

        Goo.anim.Play("Grow");
    }

    public void ShowPauseButtons() 
    {
        PausedText.gameObject.SetActive(true);
        SettingsMenu.SetActive(false);
    }

    public void ShowSettings() 
    {
        PausedText.gameObject.SetActive(false);
        SettingsMenu.SetActive(true);
    }

}
