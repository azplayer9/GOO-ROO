using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Goal : MonoBehaviour
{
    private float trans_dir = 1.0f;
    public float y_init = 1.0f
    public static bool pickedUp = false;
    //public TextMeshProUGUI victoryText;
    public GameManager gameState;

    // Start is called before the first frame update
    void Start()
    {
        y_init = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Goal.pickedUp) {
        var position = this.transform.position;
        
        if(position.y <= y_init) {
            trans_dir = 1.0f;
        }else if(position.y >= y_init + .5f){
            trans_dir = -1.0f;
        }
        
        position.y += 0.01f * trans_dir;
        this.transform.position = position;
        } else {
            // play anim (?)
            transform.localScale *= 0.95f;
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        //Debug.Log("trigger enter");
        if(col.gameObject.tag == "Player"){
            
            // destroy the bean
            Object.Destroy(this.gameObject, 1.5f);
            
            if(!Goal.pickedUp){
                //this.score_text.text = "Pieces Collected: " + ++score;
                Goal.pickedUp = true;
                gameState.isVictory = true;
            }   
        }
    }
}
