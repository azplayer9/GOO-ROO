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
            if( col.gameObject.name == "gooAsset" && this.active)
            {   
                // take damage if roo is invincible
                if (!Roo.invincible){
                    Roo.TakeDamage(30);
                }   

                StartCoroutine("DestroyBlock", 0);
            }
            else if(col.gameObject.name == "blobAsset(Clone)" && this.active)
            {
                Destroy(col.gameObject); // destroy blobs
            }
        }

        // all blocks check for ground collision
        if (col.gameObject.tag == "Ground")
        {
            StartCoroutine("DestroyBlock", 1f);
        }
        
    }

    IEnumerator DestroyBlock( float delay ){
        
        // wait before destroying block
        yield return new WaitForSeconds(delay);
        
        // destroy this object
        this.active = false;
        this.GetComponent<Animation>().Play();
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), Roo.GetComponent<Collider2D>());
        Destroy(this.gameObject, .5f);
    }
}
