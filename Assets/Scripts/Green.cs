using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Green : MonoBehaviour
{
    public float value = 0; // EACH BLOB WILL HAVE THEIR OWN VALUE
    //public bool collected = false;
    public bool activated;
    //public BoxCollider2D LeftCamBorder;
    public GameObject player;

    void Awake()
    {
        //this.collected = false;
        player = GameObject.FindObjectOfType<Player>().gameObject;
        //LeftCamBorder = GameObject.Find("ceilingLeft").GetComponent<BoxCollider2D>();
        this.activated = false;
        
        // this.GetComponent<BoxCollider2D>().enabled = false;
        Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), player.GetComponent<BoxCollider2D>(), true);
        //Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), LeftCamBorder, true);
        StartCoroutine("Activate");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // set sfx vol level
        this.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("sfxVol", 50)/400;
    }

    public void SetBlobValue(float val){
        this.value = val;
        // some code to resize the blob based on its value(?) -- CAN ALSO RESIZE BLOB IN JUMP CODE BASED ON GOO-ROO'S SIZE
    }

    IEnumerator Activate(){
        yield return new WaitForSeconds(0.1f);
        this.activated = true;
        this.GetComponent<ParticleSystem>().Play();
        //Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), player.GetComponent<BoxCollider2D>(), false);
        // play glowing anim?
    }

    // Dealing with if Player enters Green Trigger
    void OnTriggerEnter2D (Collider2D col) {
        
        // check if player is not jumping && goo is activated
        if (col.gameObject.tag == "Player" && col.GetComponent<Player>().jumpPower == 0 && this.activated){
            this.GetComponent<Rigidbody2D>().isKinematic = true;
            Object.Destroy(this.GetComponent<BoxCollider2D>());
            var Roo = col.GetComponent<Player>();
            Roo.eating = true;
            Roo.anim.Play("Eat");
            //Debug.Log("Play Eat animation");
            //this.collected = true;
            //Debug.Log("Roo has " + Roo.gooMass + " goo.");
            StartCoroutine("EatGreen", Roo);
        }    
    }

    // void OnCollisionExit2D(Collision2D col) {
    //     if(col.gameObject.tag == "Player" && !activated){
    //         StartCoroutine("Activate");
    //     }
    // }

    IEnumerator EatGreen (Player Roo){
        this.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.25f);
        Object.Destroy(this.gameObject); // destroy this object after Roo eats it
        Roo.gooMass += this.value;
        Roo.rooBody.transform.localScale = Roo.initialSize * (Roo.gooMass)/50 + Roo.initialSize; 
        Roo.eating = false;
    }

}
