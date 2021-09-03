using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player") 
        {
            Player Roo = col.gameObject.GetComponent<Player>();
            Roo.gooMass /= 2;
            Roo.rooBody.transform.localScale = Roo.initialSize * (Roo.gooMass)/50 + Roo.initialSize; 
        }
    }
}
