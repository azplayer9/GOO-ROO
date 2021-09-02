using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killZone : MonoBehaviour
{
    GameManager gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameState = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player" && !gameState.isVictory){
            // handle player death in Player Class
            col.gameObject.GetComponent<Player>().Die(); 
        }
        else {
            Object.Destroy(col.gameObject);
        }
    }
}
