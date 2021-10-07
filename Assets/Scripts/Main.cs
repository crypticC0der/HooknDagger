using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity{
	protected float health;
	protected float regenSpeed;
	protected float damage;
	protected GameObject gameObject;
	protected float regenCooldown;

	public Entity(float h,float r, float d, GameObject o){
		gameObject = o;
		health = h;
		regenSpeed=r;
		damage=d;
		regenCooldown=0;
	}

	public void Loop(){
		Regen();
	}

	public void Regen(){
		if(regenCooldown<0){
			health+=Time.deltaTime*regenSpeed;
		}
		regenCooldown-=Time.deltaTime;
	}

	public void takeDamage(float d){
		regenCooldown=3;
		health-=d;
		if(health<=0){
			GameObject.Destroy(gameObject);
		}
	}
}

public class Player : Entity{
	public static Player player;
	Rigidbody2D rb;

	public Player(Rigidbody2D rib, float h,float r, float d, GameObject o) :base(h,r,d,o){
		rb = rib;
	}

	public void Regen(){
		if(regenCooldown<0){
			health+=Time.deltaTime*regenSpeed/(rb.velocity.magnitude+0.0001f);
		}
		regenCooldown-=Time.deltaTime;
	}
}
