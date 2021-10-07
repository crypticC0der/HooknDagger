using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
	Rigidbody2D rb;
	float acceleration=50;
    // Start is called before the first frame update
    void Start(){
		rb = GetComponent<Rigidbody2D>(); 
		Player.player = new Player(rb,100,10,100,gameObject);
    }

    // Update is called once per frame
    void FixedUpdate(){
		Vector3 acc = acceleration*Input.GetAxis("Vertical")*transform.up;
		if(Input.GetAxis("Vertical")<0){
			acc*=0;
		}
		acc += acceleration*Input.GetAxis("Horizontal")*transform.right;
		if(Hookshot.hooked){
			acc*=3;
		}
		rb.AddForce(acc);
    }
}
