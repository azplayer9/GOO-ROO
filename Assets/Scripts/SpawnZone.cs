using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    public float spawnTime = 4.0f;
    public bool spawning = true;
    public BossScript EvilRoo;
    // Start is called before the first frame update
    void Start()
    {
        EvilRoo = Object.FindObjectOfType<BossScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(EvilRoo != null){
            if(!spawning)
            {

            }
        }
        else 
        {

        }
    }
}
