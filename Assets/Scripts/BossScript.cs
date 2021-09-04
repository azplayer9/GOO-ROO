using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    private Player Roo;
    private float spawnTime;
    private Animator anim;
    public bool active = true;
    // Start is called before the first frame update
    void Start()
    {
        Roo = GameObject.FindObjectOfType<Player>();
        anim = this.GetComponent<Animator>();
        active = true;
        InvokeRepeating("Stomp", 2.0f, 4f); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.name == "gooAsset")
        {
            if(Roo.gooMass < 300)
            {
                Roo.gooMass -= 30;
                Roo.rooBody.transform.localScale = Roo.initialSize * (Roo.gooMass)/50 + Roo.initialSize; 
                // Roo.anim.play("DamageBlink"); // animation to indicate damage
                col.rigidbody.AddForce(-5f * (this.transform.position - col.gameObject.transform.position), 
                                        ForceMode2D.Impulse);
            }
            else // BOSS IS DEAD 
            {
                this.active = false;
                Roo.anim.Play("Eat");
                
                // play death animation?
                Object.Destroy(this.gameObject, 0.2f);
            }
        }
    }
    
    void Stomp(){
        //this.anim.Play("Stomp");
        Debug.Log("STOMP GROUND!");
    }
}
