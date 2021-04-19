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
    public TextMeshProUGUI PausedText;
    public TextMeshProUGUI victoryText;
    
    
    void Start()
    {
        //victoryText = GameObject.FindWithTag("Victory").GetComponent<TextMeshProUGUI>();
        //victoryText.gameObject.SetActive(false);
    }

    void Update()
    {
        gooBar.fillAmount = Goo.gooMass / 100;      // fill mass UI
        gooText.text = Goo.gooMass + "%";
        gooText.gameObject.SetActive(true);
        powerBar.fillAmount = Goo.jumpPower / 100;   // fill ower bar UI

        PausedText.gameObject.SetActive(gameState.isPaused);

        if (gameState.isVictory)
            victoryText.gameObject.SetActive(true);
    }
}
