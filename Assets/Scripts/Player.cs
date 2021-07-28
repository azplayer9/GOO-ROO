using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
    
    public static float jumpPowerMin = 10f;
    public static float jumpPowerMax = 60f;

    public float speed = 5.0f;
    public float gooMass = 100;
    public float combinedForce = 0;
    private float jumpAngle = 0; // angle at which Goo will jump
    public float jumpPower = 0; // force with which Goo will jump
        
    public bool grounded = false; // whether or not Goo is on the ground
    public bool jumping = false; // whether or not Goo is jumping
    private bool walk = true;   // whether or not Goo can walk
    public bool jumpCancel = false;
    public bool eating = false;

    private GameManager gameState;

    private Rigidbody2D rig; 
    public Animator anim;
    public GameObject indicator;
    public GameObject blobObj;
    //private new Transform camera;
    
    void Start() 
    {
        gameState = FindObjectOfType<GameManager>();

        rig = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        
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
    }

    void FixedUpdate() 
    {
        if(!gameState.isVictory && !gameState.isDefeat)
        {
            var axis = Input.GetAxisRaw("Horizontal"); // get input direction
            var dist =  speed * axis * Time.deltaTime;
            // constantly update size based on gooMass
            this.transform.localScale = new Vector3(3, 3, 3) * (this.gooMass)/100 + new Vector3(2,2,2);

            // Code for if Goo is *NOT* jumping
            if (!jumping)
            {
                // on rightclick, charge the jump IF have enough goo.
                if (this.gooMass > 10 && (Input.GetMouseButton(1) || Input.GetMouseButtonDown(1))) 
                {
                    if(!this.jumpCancel && this.gooMass >= 10f && !eating)
                    {
                        this.indicator.SetActive(true); // make indicator show up
                        
                        if (this.walk)
                        {
                            this.walk = false; // disable normal movement
                            anim.Play("JumpCharge");
                            
                        }
                        
                        // shifting jumpAngle/indicator using input axis
                        if (this.jumpAngle - axis * 10 < 90 && this.jumpAngle - axis * 10 > -90)
                        {
                            this.jumpAngle -= axis * SettingsManager.angle_sensitivity; // default angle sensitivity is 3.0f
                        }

                        this.indicator.transform.rotation = Quaternion.Euler(0, 0, (this.jumpAngle)); // * jumpDir);
                        
                        // CHARGE JUMP
                        if( 9 + Mathf.Floor(this.jumpPower / 5) < this.gooMass &&       // goo needs to make sure he doesnt use up all his resources to jump
                            this.jumpPower < jumpPowerMax )                             // can charge the jump to a limit (~2-4 seconds?)
                        {
                            //Debug.Log("CHARGING:" + this.jumpPower);
                            this.jumpPower += 1;
                        }
                        else {
                            anim.Play("JumpBlink");
                        }
                    }

                    // CANCEL JUMP
                    if (Input.GetMouseButton(0)) // cancel jump on left click
                    {
                        this.walk = true; // re-enable normal movement
                        this.indicator.SetActive(false); // get rid of indicator
                        anim.Play("Idle");

                        //this.jumpAngle = 0;
                        this.jumpPower = 0; // reset jumpPower on mouseUp
                        this.jumpCancel = true;
                    }
                }
                // if Goo can walk and S is not being held, then move
                else if (this.walk){
                    if(!eating){
                        if(axis != 0) {
                            anim.Play("Hop");
                        }
                        else {
                            anim.Play("Idle");
                        }
                    }
                    
                    var pos = rig.position;
                    
                    if (this.grounded){     // normal movement
                        pos.x += dist;
                        this.rig.velocity = Vector2.zero;
                    }
                    else {                  // mid-air movement
                        pos.x += dist * ((10f - (combinedForce)) / 10f); // weaker directional influence if goo is midair (weakens over time)
                        // update combinedForce so DI gets weaker over time
                        if (combinedForce < 5f) {
                            combinedForce += Mathf.Abs(dist) * ((10f - (combinedForce)) / 10f);
                        }
                    }
                    rig.position = pos;
                } 
                // If Right Mouse Button is no longer being held, then jump. 
                else {
                    // conditions for actually jumping
                    if(this.jumpPower >= jumpPowerMin){ // some soft lower bound to ensure the user cannot/does not short jump
                        // x = power * direction, y = power
                        this.jumping = true;
                        anim.Play("JumpRelease");

                        var jumpAngleRad = Mathf.PI * (90+this.jumpAngle)/180;
                        Vector2 jumpVec = new Vector2(  Mathf.Cos(jumpAngleRad)/2, // * this.jumpDir, 
                                                        Mathf.Sin(jumpAngleRad)/2 ); 
                        this.rig.AddForce(jumpVec * ( Mathf.Pow(this.jumpPower, 0.8f) ) , ForceMode2D.Impulse); // charging has diminishing returns
                        SpawnBlob(8 + Mathf.Floor(this.jumpPower / 5));
                    }
                    
                    this.indicator.SetActive(false); // get rid of indicator
                    this.walk = true; // re-enable normal movement
                    this.jumpPower = 0; // reset jumpPower on mouseUp
                    jumpCancel = false;
                }

                if(Input.GetMouseButtonUp(1)){
                    jumpCancel = false;
                }
            }
            // assume Goo *IS* jumping
            else {
                this.rig.AddForce( new Vector2(axis * 0.5f * (15f-combinedForce), 0) , ForceMode2D.Force ); // slight directional influence direction
                if (this.combinedForce < 10.0f){    
                    this.combinedForce += Mathf.Abs(axis) * 0.5f;
                }
            }

            // if (Input.GetKeyUp(KeyCode.S)){
            //     this.indicator.SetActive(false);
            // }

            // if indicator is active, change angle
            if(this.indicator.activeSelf){
                if(Input.GetKeyDown(KeyCode.Space)){ // pressing spacebar when setting angle resets angle to 0 (pointing up)
                    this.jumpAngle = 0;
                }
            }

        }
    }

    private bool SpawnBlob(float val){
        // instantiate new blob 
        var spawnPos = this.transform.position - new Vector3(0, 0.25f, 0);
        this.GetComponent<BoxCollider2D>().enabled = false;

        StartCoroutine( DelayedSpawn(val, 0.1f, spawnPos, this.transform.localScale) );
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

    IEnumerator DelayedSpawn(float value, float delay, Vector3 pos, Vector3 scale) {
        //this.gooMass *= .5f;
        this.gooMass -= value;

        yield return new WaitForSeconds(delay);
        
        GameObject newBlob = Instantiate(blobObj);        
        //Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), newBlob.GetComponent<BoxCollider2D>(), true);
        newBlob.transform.position = pos;
        newBlob.transform.localScale = scale;

        //newBlob.GetComponent<Green>().SetBlobValue( this.gooMass/2 );
        //this.gooMass /= 2; // divide goo mass by 2
        newBlob.GetComponent<Green>().SetBlobValue( value );
        Debug.Log("Roo has " + this.gooMass + "goo.");

        yield return new WaitForSeconds(delay);

        this.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void Die() {
        // play death animation
        Object.Destroy(this.gameObject);
        gameState.isDefeat = true; 
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Ground"){
            this.grounded = true;
            this.jumping = false;
            this.combinedForce = 0;
            // kill any momentum (identified by what kind of blocks are interacted with?)
            //this.rig.velocity = Vector2.zero;
            //this.rig.angularVelocity = 0;

            // set camera transform to player
            // var pos = camera.position;
            // if(this.transform.position.x < CameraControl.minX)
            //     pos.x = CameraControl.minX;
            // else if(this.transform.position.x > CameraControl.maxX)
            //     pos.x = CameraControl.maxX;
            // else pos.x = this.transform.position.x;
            // camera.position = pos;
        }
        if(col.gameObject.tag == "Bound"){
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "Ground"){
            this.grounded = false;
            //this.indicator.SetActive(false); // make indicator show up
        }
    }


}
