using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    public bool isPaused;
    public bool isVictory;
    public bool isDefeat;

    public int LevelNumber; // initialize level number in editor
    public static int LastLevel = 8;    // cut-off point before transitioning to boss stage

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
                RestartLevel(0);
            }
            else{           // otherwise wait a little bit before restarting (fade out?)
                //Invoke("RestartLevel", 0.5f);
                RestartLevel(0.5f);
            }
        }

        // pause/unpause using Escape key, but can't pause if game is over
        if ( !isVictory && !isDefeat && ( Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) ) ) {
            PauseLevel(isPaused); // call the pause function
            
            // flip the paused bool
            //isPaused = !isPaused;
        }

        if (isDefeat) {
            // defeat menu? fade to black/restart level
        }   
    }
    
    // reload the current scene
    public void RestartLevel(float delay) 
    {    
        // set a delay before changing scenes (animation? etc.)
        Invoke("RestartTimer", delay);
        
        // insert code for fade to black

        //GoToLevel(LevelNumber);
        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }

    IEnumerator RestartTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    // pause/unpause game based on current pause state
    public void PauseLevel(bool paused) {
        if (paused) {
            Time.timeScale = 1; // if already paused, unpause
            //Debug.Log("unpause");
            this.isPaused = false;
        }
        else {
            Time.timeScale = 0; // if not paused, pause
            //Debug.Log("pause");
            this.isPaused = true;
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
        // may need to change this to instead use LevelNumber
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex + 1 );

        if(this.LevelNumber == LastLevel || this.LevelNumber <= 0) { // make sure to destroy audio before entering boss stage
            Destroy(GameObject.FindGameObjectWithTag("Respawn"));
        }
    }

    // return to main menu scene
    public void ReturnToMenu() {
        Destroy(GameObject.FindGameObjectWithTag("Respawn"));
        SceneManager.LoadScene("0_Title");
    }
}
