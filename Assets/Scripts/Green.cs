using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Green : MonoBehaviour
{
    public float value = 0; // EACH BLOB WILL HAVE THEIR OWN VALUE
    //public bool collected = false;
    public bool activated;
    public BoxCollider2D LeftCamBorder;
    public GameObject player;

    void Awake()
    {
        //this.collected = false;
        this.activated = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine("Activate");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetBlobValue(float val){
        this.value = val;
        // some code to resize the blob based on its value(?) -- CAN ALSO RESIZE BLOB IN JUMP CODE BASED ON GOO-ROO'S SIZE
    }

    IEnumerator Activate(){
        yield return new WaitForSeconds(0.3f);
        this.activated = true;
        this.GetComponent<BoxCollider2D>().enabled = true;

        //Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), false);
        Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), LeftCamBorder, true);
        // play glowing anim?
    }

    // Dealing with if Player enters Green Trigger
    void OnTriggerEnter2D (Collider2D col) {
        Debug.Log(col);
        
        if (col.gameObject.tag == "Player" && this.activated){  // player has run into blob
            var Roo = col.GetComponent<Player>();

            Roo.gooMass += this.value;
            //this.collected = true;
            Debug.Log("Roo has " + Roo.gooMass + " goo.");
            
            Object.Destroy(this.gameObject, 0.1f); // destroy this object after Roo eats it
        }    
    }

}
