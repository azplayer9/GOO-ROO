using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelAudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject audioObj = GameObject.FindGameObjectWithTag("Respawn");
        
        if (audioObj)
        {
            if(SceneManager.GetActiveScene().name == "Level-1")
            {
                Destroy(audioObj);
                this.gameObject.tag = "Respawn";
                DontDestroyOnLoad(this.gameObject);
            }
            else 
            {
                Object.Destroy(this.gameObject);
            }
        }
        else 
        {
            this.gameObject.tag = "Respawn";
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
