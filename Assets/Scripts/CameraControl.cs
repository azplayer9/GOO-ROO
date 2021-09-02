using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Transform player;
    
    public int minX = -3;
    public int maxX = 45; // maxBorder - 10.5 (the maximum x value that the camera can have)
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
        
        // camera moves right only if player is on right half of screen moving right
        if(pos.x > maxX) {
            pos.x = maxX;
            this.transform.position = pos;
        } 
        else if (pos.x < minX) {
            pos.x = minX;
            this.transform.position = pos;
        } 
        else if(player.position.x > pos.x + 2 &&  pos.x < maxX) {
            pos.x = player.position.x - 2;
            this.transform.position = pos;
        } 
        else if(player.position.x < pos.x - 2 && pos.x > minX) { 
            pos.x = player.position.x + 2;
            this.transform.position = pos;
        }

        
    }
}
