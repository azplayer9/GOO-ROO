using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
    
    public static float jumpPowerMin = 10f;
    public static float jumpPowerMax = 60f;

    public float speed = 5.0f;
    public float gooMass = 100;
    public float gravScale = 1.0f; //  gravity scale ** CHANGE THIS NAMNE **
    public float combinedForce = 0;
    private float jumpAngle = 0; // angle at which Goo will jump
    public float jumpPower = 0; // force with which Goo will jump
        
    public bool grounded = false; // whether or not Goo is on the ground
    public bool jumping = false; // whether or not Goo is jumping
    private bool walk = true;   // whether or not Goo can walk
    public bool jumpCancel = false;
    public bool eating = false;
    public bool invincible = false;

    public bool stopped; // used to control Goo's momentum upon completing a level

    private GameManager gameState;
    public Vector3 initialSize;
    
    public GameObject rooBody;
    private Rigidbody2D rig; 
    public Animator anim;
    public GameObject indicator;
    public GameObject blobObj;
    //public AudioSource audio;

    public AudioClip bounceSFX;
    public AudioClip chargeSFX;
    public AudioClip hurtSFX;
    //public AudioClip chargeLoopSFX;
    
    //private new Transform camera;
    
    void Start() 
    {
        gameState = FindObjectOfType<GameManager>();
        rooBody = GameObject.Find("gooAsset");
        rig = rooBody.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        
        //Debug.Log(initialSize);
        //camera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        //indicator = GameObject.FindWithTag("Arrow");
        //indicator.SetActive(false);

        // initialize variables upon scene start/restart
        gooMass = 100;
        combinedForce = 0;
        jumpAngle = 0;
        jumpPower = 0;
        
        grounded = false;
        jumping = false;
        walk = true;
        jumpCancel = false;
        eating = false;
        invincible = false;

        rooBody.transform.localScale = initialSize * (this.gooMass)/50 + initialSize; 
    }

    void FixedUpdate() 
    {
        // set sfx volume based on settings
        this.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("sfxVol", 50)/500;

        if(this.gooMass <= 0){  // MAKE SURE HEALTH ALWAYS > 0
            this.gooMass = 0; // set mass to 0 to make sure UI looks ok
            Die();
        }

        if(!gameState.isVictory && !gameState.isDefeat)
        {
            var axis = Input.GetAxisRaw("Horizontal"); // get input direction
            //Debug.Log(axis);
            var dist =  speed * axis * Time.fixedDeltaTime;

            // constantly update size based on gooMass -- scales from initial size to 3x initial size
            // rooBody.transform.localScale = initialSize * (this.gooMass)/50 + initialSize; 

            // Code for if Goo is *NOT* jumping
            if (!jumping)
            {
                // on rightclick, charge the jump IF have enough goo.
                if (this.gooMass > 10 && (Input.GetMouseButton(1) || Input.GetMouseButtonDown(1))) 
                {
                    if(!this.jumpCancel && this.gooMass >= 10f && !eating)
                    {
                        
                        this.indicator.SetActive(true); // make indicator show up
                        this.rig.gravityScale = gravScale/10; // reduce gravity if charging mid-air

                        if (this.walk)
                        {
                            this.walk = false; // disable normal movement
                            anim.Play("JumpCharge");
                            this.GetComponent<AudioSource>().PlayOneShot(chargeSFX);
                        }
                        
                        // shifting jumpAngle/indicator using input axis

                            if (this.jumpAngle - axis * 10 < 80 && this.jumpAngle - axis * 10 > -80)
                            {
                                this.jumpAngle -= axis * PlayerPrefs.GetFloat("angleSens", 3f); // default angle sensitivity is 3.0f
                            }


                        this.indicator.transform.rotation = Quaternion.Euler(0, 0, (this.jumpAngle)); // * jumpDir);
                        
                        // CHARGE JUMP
                        if( 9 + Mathf.Floor(this.jumpPower / 5) < this.gooMass &&       // goo needs to make sure he doesnt use up all his resources to jump
                            this.jumpPower < jumpPowerMax )                             // can charge the jump to a limit (~2-4 seconds?)
                        {
                            //Debug.Log("CHARGING:" + this.jumpPower);
                            this.jumpPower += 1;
                        }
                        else 
                        {
                            anim.Play("JumpBlink");
                            if(!GetComponent<AudioSource>().isPlaying)
                                GetComponent<AudioSource>().Play();   
                        }
                    }

                    // CANCEL JUMP
                    if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)) // cancel jump on left click
                    {
                        this.walk = true; // re-enable normal movement
                        this.rig.gravityScale = this.gravScale;// reset rigidbody mass
                        this.indicator.SetActive(false); // get rid of indicator
                        anim.Play("Idle");
                        GetComponent<AudioSource>().Stop();
                        //this.jumpAngle = 0;
                        this.jumpPower = 0; // reset jumpPower on mouseUp
                        this.jumpCancel = true;
                    }
                }
                // if Goo can walk and S is not being held, then move
                else if (this.walk)
                {
                    if(!eating && !invincible)
                    {
                        if(axis != 0) 
                        {
                            anim.Play("Hop");
                        }
                        else
                        {
                            anim.Play("Idle");
                        }
                    }
                    
                    var pos = rig.position;
                    
                    if (this.grounded)      // normal movement
                    {
                        pos.x += dist;
                        //this.rig.velocity = Vector2.zero;
                    }
                    else                    // mid-air movement
                    {
                        pos.x += dist * ((10f - (combinedForce)) / 10f); // weaker directional influence if goo is midair (weakens over time)
                        // update combinedForce so DI gets weaker over time
                        if (combinedForce < 5f) 
                        {
                            combinedForce += Mathf.Abs(dist) * ((10f - (combinedForce)) / 10f);
                        }
                    }
                    rig.position = pos;
                } 
                // If Right Mouse Button is no longer being held, then try to jumpjump. 
                else 
                {
                    // Condition to actually jump
                    if(this.jumpPower >= jumpPowerMin) // some soft lower bound to ensure the user cannot/does not short jump
                    {
                        // Handling JUMP action
                        // x = power * direction, y = power
                        this.jumping = true;
                        this.grounded = false;
                        this.rig.gravityScale = gravScale;
                        
                        anim.Play("JumpRelease");
                        GetComponent<AudioSource>().Stop();
                        GetComponent<AudioSource>().PlayOneShot(bounceSFX);

                        var jumpAngleRad = Mathf.PI * (90+this.jumpAngle)/180;
                        Vector2 jumpVec = new Vector2(  Mathf.Cos(jumpAngleRad)/2, // * this.jumpDir, 
                                                        Mathf.Sin(jumpAngleRad)/2 ); 

                        //this.rig.velocity = Vector2.zero; // set velocity to 0 before jumping
                        this.rig.velocity -= new Vector2(0f, 2f); // in case of mid-air jump
                        
                        if(gooMass <= 40f){
                             jumpVec *= 1.3f; // makes smaller goo jump a little bit higher
                        }

                        this.rig.AddForce(jumpVec * ( Mathf.Pow(this.jumpPower, 0.8f) ) , ForceMode2D.Impulse); // charging has diminishing returns
                        SpawnBlob(8 + Mathf.Floor(this.jumpPower / 5)); // spawn a blob after jumping
                    }
                    else{
                        GetComponent<AudioSource>().Stop();
                    }

                    this.indicator.SetActive(false); // get rid of indicator
                    this.walk = true; // re-enable normal movement
                    this.jumpPower = 0; // reset jumpPower on mouseUp
                    jumpCancel = false;
                }

                // turn jump cancel off if no buttons are pressed
                if(!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
                {
                    jumpCancel = false;
                }
            }
            // assume Goo *IS JUMPING* (handling mid-air movement)
            else 
            {
                this.rig.AddForce( new Vector2(axis * 0.5f * (15f-combinedForce), 0) , ForceMode2D.Force ); // slight directional influence direction
                if (this.combinedForce < 10.0f)
                {    
                    this.combinedForce += Mathf.Abs(axis) * 0.5f;
                }
            }

            // if indicator is active, change angle
            if(this.indicator.activeSelf)
            {
                // pressing spacebar when setting angle resets angle to 0 (pointing up)
                if(Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Space))
                {
                    this.jumpAngle = 0;
                }
            }

        }
        else {
            if(!stopped) {
                this.rig.velocity = Vector2.zero;
                stopped = true;
            }

            // this.rig.isKinematic = true;
            // play victory animation? 
            //anim.Play("Grow");
        }
    }

    private bool SpawnBlob(float val)
    {
        // instantiate new blob 
        var spawnPos = this.transform.position - new Vector3(0, 0.15f, 0);
        //this.GetComponent<BoxCollider2D>().enabled = false;

        StartCoroutine( DelayedSpawn(val, 0.1f, spawnPos, this.rooBody.transform.localScale) );
        // GameObject newBlob = Instantiate(blobObj);        
        // //newBlob.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - (0.28f) );
        
        // newBlob.transform.position = this.transform.position;
        // newBlob.transform.localScale = this.transform.localScale;

        // //newBlob.GetComponent<Green>().SetBlobValue( this.gooMass/2 );
        // //this.gooMass /= 2; // divide goo mass by 2
        // newBlob.GetComponent<Green>().SetBlobValue( this.gooMass * .5f );
        // this.gooMass *= .5f;
        // Debug.Log("Roo has " + this.gooMass + "goo.");
        
        return true;
    }

    IEnumerator DelayedSpawn(float value, float delay, Vector3 pos, Vector3 scale) 
    {
        //this.gooMass *= .5f;
        this.gooMass -= value;
        yield return new WaitForSeconds(delay);
        
        GameObject newBlob = Instantiate(blobObj);        
        newBlob.transform.position = pos;
        newBlob.transform.localScale = scale * (value/20);

        rooBody.transform.localScale = initialSize * (this.gooMass)/50 + initialSize; 

        //newBlob.GetComponent<Green>().SetBlobValue( this.gooMass/2 );
        //this.gooMass /= 2; // divide goo mass by 2
        newBlob.GetComponent<Green>().SetBlobValue( value );
        //Debug.Log("Roo has " + this.gooMass + "goo.");

        yield return new WaitForSeconds(delay);

        //this.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void Die() 
    {
        this.indicator.SetActive(false); // get rid of indicator
        // play death animation
        // play sfx?
        this.GetComponent<AudioSource>().pitch /= 2;
        this.GetComponent<AudioSource>().PlayOneShot(chargeSFX);
        this.GetComponent<AudioSource>().pitch *= 2;
        gameState.isDefeat = true; 
        this.GetComponent<SpriteRenderer>().enabled = false;
        Object.Destroy(this.gameObject, 1f);
        
    }

    public void TakeDamage(float dmg)
    {
        this.gooMass -= dmg;
        this.invincible = true;
        StartCoroutine("Invincibility");
        
        if (this.gooMass > 0) 
        {
            this.anim.Play("Hurt");
            this.GetComponent<AudioSource>().PlayOneShot(hurtSFX);
        }
        
        rooBody.transform.localScale = initialSize * (gooMass)/50 + initialSize; 

    }

    IEnumerator Invincibility() 
    {
        yield return new WaitForSeconds(1f);
        this.invincible = false;
    }


    void OnCollisionEnter2D(Collision2D col) 
    {
        if (col.gameObject.tag == "Ground")
        {
            this.grounded = true;
            this.jumping = false;
            this.combinedForce = 0;
            // kill any momentum (identified by what kind of blocks are interacted with?)
            //this.rig.velocity = new Vector2(this.rig.velocity.x * -3f, this.rig.velocity.y);
            //this.rig.angularVelocity = 0;
            this.rig.velocity = Vector2.zero;

            // set camera transform to player
            // var pos = camera.position;
            // if(this.transform.position.x < CameraControl.minX)
            //     pos.x = CameraControl.minX;
            // else if(this.transform.position.x > CameraControl.maxX)
            //     pos.x = CameraControl.maxX;
            // else pos.x = this.transform.position.x;
            // camera.position = pos;
        }
        if(col.gameObject.tag == "Bound")
        {
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    void OnCollisionExit2D(Collision2D col) 
    {
        if (col.gameObject.tag == "Ground")
        {
            this.grounded = false;
            //this.indicator.SetActive(false); // make indicator show up
        }
    }

}
