using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private Player Roo;
    public bool harmful = false;
    private bool active = true;

    // Start is called before the first frame update
    void Start()
    {
        Roo = GameObject.FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // only run this code on danger blocks
        if(this.harmful)
        {
            if(col.gameObject.name == "gooAsset" && this.active)
            {   
                Roo.gooMass -= 20;
                Roo.rooBody.transform.localScale = Roo.initialSize * (Roo.gooMass)/50 + Roo.initialSize; 
                // Roo.anim.play("DamageBlink"); // animation to indicate damage

                // destroy this object
                this.GetComponent<Animation>().Play();
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), Roo.GetComponent<Collider2D>());
                this.active = false;
                Destroy(this.gameObject, .5f);
            }
            else if(col.gameObject.name == "blobAsset(Clone)" && this.active)
            {
                Destroy(col.gameObject); // destroy blobs
            }
        }

        // all blocks check for ground collision
        if (col.gameObject.tag == "Ground")
        {
            // destroy this object
            this.GetComponent<Animation>().Play();
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), Roo.GetComponent<Collider2D>());
            this.active = false;
            Destroy(this.gameObject, .5f);
        }
    }
}
