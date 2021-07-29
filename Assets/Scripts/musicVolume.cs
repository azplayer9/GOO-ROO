using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicVolume : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("bgmVol", 50)/100;
    }
}
