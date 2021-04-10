using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Transform player;
    
    public static int minX = -3;
    public static int maxX = 40; // maxBorder - 10.5 (the maximum x value that the camera can have)
    //private float camSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        var pos = this.transform.position;
        // Debug.Log(pos);
        //Debug.Log(Input.GetAxisRaw("Horizontal"));
        
        // var newPos = pos.x + Input.GetAxisRaw("Horizontal")*camSpeed;
        // if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.S) && newPos >= minX && newPos <= maxX){
        //     // Debug.Log("Move Camera");
        //     pos.x = newPos;
        // }
        
        //var dist = player.position.x - pos.x;
        // //if(player.position.x > minX && player.position.x < maxX - 6)
        
        // if(pos.x > minX && Input.GetAxisRaw("Horizontal") < 0 && dist <= -7.5 ) { // camera moves left only if player is moving left on left quarter of the screen
        //     //pos.x += Player.speed * Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        //     pos.x = player.transform.position.x;
        // }
        //else if(pos.x < maxX && Input.GetAxisRaw("Horizontal") > 0 && dist >= 0) { // camera moves right only if player is on right half of screen moving right
        
        if(pos.x < maxX && player.position.x >= pos.x) { // camera moves right only if player is on right half of screen moving right
            pos.x = player.position.x;
        }
        this.transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D col) { // manages camera movement if player moves halfway past the screen
        var pos = this.transform.position;
        Debug.Log(pos.x);
        if (col.gameObject.tag == "Player" && pos.x < maxX){
            Debug.Log(col.gameObject.GetComponent<Transform>().position.x);
            pos.x = col.gameObject.GetComponent<Transform>().position.x; // only change x value of the camera
            this.transform.position = pos;
        }

    }
}
