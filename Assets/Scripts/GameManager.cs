using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public bool isPaused;
    public bool isVictory;
    public bool isDefeat;

    public int LevelNum; // initialize level number in editor

    void Start()
    {
        Time.timeScale = 1; // scene should start unpaused
        isPaused = false; // make sure when restarting scene, game is not paused
        isVictory = false;
        isDefeat = false;
    }

    // Check for Key Presses to Manipulate the Scene
    void Update()
    {
        // Press R; also restart automatically when player 
        if (Input.GetKeyDown(KeyCode.R)) {
            if (isPaused) { // if game is paused, restart level immediately
                RestartLevel();
            }
            else{           // otherwise wait a little bit before restarting (fade out?)
                Invoke("RestartLevel", 0.5f);
            }
        }

        // pause/unpause using Escape key, but can't pause if game is over
        if ( !isVictory && !isDefeat && ( Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) ) ) {
            PauseLevel(isPaused); // call the pause function
            
            // flip the paused bool
            isPaused = !isPaused;
        }

        if (isDefeat) {
            // defeat menu? fade to black/restart level
        }   
    }
    
    // reload the current scene
    public void RestartLevel() {    
        //SceneManager.LoadScene( SceneManager.GetActiveScene().name );
        GoToLevel(-1);
    }

    // pause/unpause game based on current pause state
    public void PauseLevel(bool paused) {
        if (paused) {
            Time.timeScale = 1; // if already paused, unpause
            //Debug.Log("unpause");
        }
        else {
            Time.timeScale = 0; // if not paused, pause
            //Debug.Log("pause");
        }
    }

    // go directly to a level (from level select)
    public void GoToLevel(int levelNum)
    {
        if(levelNum < 1 ) // Level 0 and Level -1 have different soundtracks
        {
            Destroy(GameObject.FindGameObjectWithTag("Respawn")); // destroy the soundtrack object currently playing
        }
        if(levelNum < 9 ){
            var name = "Level"+ levelNum;
            //print(name);
            SceneManager.LoadScene("Level"+ levelNum);
        }
        else{
            Debug.Log("Level not added yet.");
        }
    }

    // go to the next scene in the list
    public void NextLevel() {
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1 );
    }

    // return to main menu scene
    public void ReturnToMenu() {
        Destroy(GameObject.FindGameObjectWithTag("Respawn"));
        SceneManager.LoadScene("0_Title");
    }
}
