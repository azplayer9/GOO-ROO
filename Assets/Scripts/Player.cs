using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    
    public float speed = 5.0f;
    public float gooMass = 100;
    public float combinedForce = 0;
    private float jumpAngle = 0; // angle at which Goo will jump
    public float jumpPower = 0; // force with which Goo will jump
        
    public bool grounded = false; // whether or not Goo is on the ground
    public bool jumping = false; // whether or not Goo is jumping
    private bool walk = true;   // whether or not Goo can walk
    public bool jumpCancel = false;
    public static bool dead = false;
    
    public GameManager gameState;

    private Rigidbody2D rig; 
    private Animator anim;
    private GameObject indicator;
    public GameObject blobObj;
    //private new Transform camera;
    
    void Start() {
        rig = this.GetComponent<Rigidbody2D>();
        //anim = this.GetComponent<Animator>();
        
        //camera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        indicator = GameObject.FindWithTag("Arrow");
        indicator.SetActive(false);

        // initialize variables upon scene start/restart
        gooMass = 100;
        combinedForce = 0;
        jumpAngle = 0;
        jumpPower = 0;
        
        grounded = false;
        jumping = false;
        walk = true;
        jumpCancel = false;
        Player.dead = false;
    }

    void FixedUpdate() {
        if(!gameState.isVictory && !dead){
            var axis = Input.GetAxisRaw("Horizontal"); // get input direction
            var dist =  speed * axis * Time.deltaTime;
            // constantly update size based on gooMass
            this.transform.localScale = new Vector3(2, 2, 2) * (this.gooMass)/100 + new Vector3(3,3,3);

            // Code for if Goo is *NOT* jumping
            if (!jumping){
                // if Goo can walk and S is not being held, then move
                if (this.walk && axis != 0){ 
                    //moving = true; 
                    //anim.Play("walk");
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

                // on rightclick, charge the jump.
                if (Input.GetMouseButton(1) || Input.GetMouseButtonDown(1)) {
                    if(!this.jumpCancel && this.gooMass >= 10f){
                        this.walk = false; // disable normal movement
                        this.indicator.SetActive(true); // make indicator show up
                        
                        // shifting jumpAngle/indicator using input axis
                        if (this.jumpAngle - axis * 10 < 90 && this.jumpAngle - axis * 10 > -90){
                            this.jumpAngle -= axis * SettingsManager.angle_sensitivity; // default angle sensitivity is 3.0f
                        }
                        this.indicator.transform.rotation = Quaternion.Euler(0, 0, (this.jumpAngle)); // * jumpDir);
                        
                        // CHARGE JUMP
                        if(this.jumpPower < 100){ // can charge the jump to a limit (~2-4 seconds?)
                            //Debug.Log("CHARGING:" + this.jumpPower);
                            this.jumpPower += 2;
                        }
                    }
                    // CANCEL JUMP
                    if (Input.GetMouseButton(0)){ // cancel jump on left click
                        this.walk = true; // re-enable normal movement
                        this.indicator.SetActive(false); // get rid of indicator
                        //this.jumpAngle = 0;
                        this.jumpPower = 0; // reset jumpPower on mouseUp
                        this.jumpCancel = true;
                    }
                }
                // If Right Mouse Button is no longer being held, then jump. 
                else {
                    // conditions for actually jumping
                    if(this.jumpPower >= 5 && SpawnBlob()){ // some soft lower bound to ensure the user cannot/does not short jump
                        // x = power * direction, y = power
                        this.jumping = true;
                        var jumpAngleRad = Mathf.PI * (90+this.jumpAngle)/180;
                        Vector2 jumpVec = new Vector2(  Mathf.Cos(jumpAngleRad), // * this.jumpDir, 
                                                        Mathf.Sin(jumpAngleRad)); 

                        this.rig.AddForce(jumpVec * ( Mathf.Pow(this.jumpPower, 0.9f) * 0.5f) , ForceMode2D.Impulse); // charging has diminishing returns
                    }

                    this.walk = true; // re-enable normal movement
                    this.indicator.SetActive(false); // get rid of indicator
                    this.jumpPower = 0; // reset jumpPower on mouseUp
                    jumpCancel = false;
                }
            }

            else {  // assume Goo *IS* jumping
                this.rig.AddForce( new Vector2(axis * (15f-combinedForce), 0) , ForceMode2D.Force ); // slight directional influence direction
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

    private bool SpawnBlob(){
        if (this.gooMass >= 10){
            // instantiate new blob 
            GameObject newBlob = Instantiate(blobObj);        
            //newBlob.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - (0.28f) );
            newBlob.transform.position = this.transform.position;
            newBlob.transform.localScale = this.transform.localScale;

            //newBlob.GetComponent<Green>().SetBlobValue( this.gooMass/2 );
            //this.gooMass /= 2; // divide goo mass by 2
            newBlob.GetComponent<Green>().SetBlobValue( this.gooMass * .5f );
            this.gooMass *= .5f;
            Debug.Log("Roo has " + this.gooMass + "goo.");
            
            return true;
        }

        return false;
    }

    public void Die() {
        // play death animation
        Player.dead = true;
        Object.Destroy(this.gameObject);
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
    }

    void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "Ground"){
            this.grounded = false;
            this.indicator.SetActive(false); // make indicator show up
        }
    }


}
