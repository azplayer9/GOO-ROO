using System.Collections;
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
    public GameObject victoryObj;
    public GameObject defeatObj;

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
         // first check for victory/defeat
        if (gameState.isVictory && !victorious)
        {
            StartCoroutine("HandleVictory");
        }
        else if(gameState.isDefeat)     // HANDLES POST MORTEM UI/GAME STUFF
        {
            // probably want to tweak this code
            //gameState.RestartLevel(0.5f);
            defeatObj.SetActive(true);
        }
        // code to run if game is not paused
        else if(!gameState.isPaused) 
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

       
        
    }

    public IEnumerator HandleVictory()
    {
        victorious = true;
        victoryObj.SetActive(true);
        
        Goo.anim.Play("Eat");        
        yield return new WaitForSeconds(.4f);

        if(Goo.gooMass < 50)
            Goo.rooBody.transform.localScale = Goo.initialSize * 2; // growing size
        yield return new WaitForSeconds(.45f);
        
        if (Goo.gooMass < 100)
            Goo.rooBody.transform.localScale = (Goo.initialSize * 3); // max out size for victory

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
