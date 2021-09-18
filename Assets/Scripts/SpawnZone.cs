using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    
    //public float spawnTime = 4.0f;
    public bool spawning = true;
    public int difficulty = 1;
    private int objsSpawned = 0;

    public BossScript EvilRoo;
    public Player Roo;
    public GameObject block;
    public GameObject blob;

    // Start is called before the first frame update
    void Start()
    {
        Roo = Object.FindObjectOfType<Player>();
        EvilRoo = Object.FindObjectOfType<BossScript>();
        spawning = true;
        difficulty = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (difficulty < 4) // cap the difficulty at 4
        {
            difficulty = objsSpawned / 20 + 1;
        }
        // make sure both Roo and Evil Roo are alive
        if(EvilRoo && EvilRoo.active && Roo && Roo.gooMass > 0)
        {
            if(spawning)
            {
                //Debug.Log("Start Coroutine");
                StartCoroutine("SpawnRandom");
                spawning = false;
            }
        }
        else // boss has been defeated
        {
            //Debug.Log("Boss has been defeated");
            // stop spawning
            StopCoroutine("SpawnRandom");
            spawning = false;
        }
    }

    IEnumerator SpawnRandom(){
        // delay before next spawn

        // approximate area to spawn object
        EvilRoo.anim.Play("Stomp");

        float numSpawn = Random.Range(1*difficulty, 3*difficulty);

        for(int i = 0; i < numSpawn; i++)
        {
            Debug.Log("Spawn Object");

            if ( Random.Range(0, 3) < 1 )
            {
                GameObject spawn = Instantiate(blob);

                int val = Random.Range(2, 4) * 10;
                spawn.GetComponent<Green>().SetBlobValue(Random.Range(2, 5) * 10);
                spawn.GetComponent<Green>().activated = true;

                spawn.transform.position = new Vector3( Random.Range(-10, 12), 
                                                        Random.Range(20, 25),
                                                        0);

                spawn.transform.localScale = new Vector3(val/3, val/3, 1);

                objsSpawned++; // increment objs spawned an extra time to speed up difficulty
            }
            else
            {
                GameObject spawn = Instantiate(block);

                spawn.transform.position = new Vector3( Random.Range(-10, 12), 
                                                        Random.Range(20, 25), 
                                                        0);
            }

            objsSpawned++;
        }

        // wait for delay before respawning a new object
        yield return new WaitForSeconds(Random.Range(4f, 8f) - difficulty);
        spawning = true;
    }
}
