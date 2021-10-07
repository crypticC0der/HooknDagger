using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookshot : MonoBehaviour
{
	public float len;
	public Camera cam;
	public float elst;
	public static bool hooked=false;
	bool shot=false;
	float cooldown=0;
	GameObject l;
	float spd=40;
	float mult=80;
	Vector3 v;
	Vector3 endPos;
	Rigidbody2D rb;
	Transform attached;
    // Start is called before the first frame update
    void Start(){
		rb = GetComponent<Rigidbody2D>();
    }

	public static Vector3 RotateVec(Vector3 v, float theata){
		// cos -sin
		// sin cos
		theata *= Mathf.PI/180;
		Vector3 r = new Vector3(0,0,0);
		r.x = Mathf.Cos(theata)*v.x - Mathf.Sin(theata)*v.y;
		r.y = Mathf.Sin(theata)*v.x + Mathf.Cos(theata)*v.y;
		return r;
	}

	public Vector3 End(){
		if(!hooked){
			return endPos;
		}else{
			Vector3 localChange = RotateVec(endPos,attached.eulerAngles.z);
			return attached.position+localChange;
		}
	}

	void FixedUpdate(){
		if(!hooked&&shot){
			RaycastHit2D rh = Physics2D.Raycast(endPos,v,v.magnitude,~ (1<<6));
			if(rh){
				v=new Vector3(0,0.00000000000000000000000000001f);
				attached=rh.transform;
				endPos=(Vector3)rh.point-attached.position;
				endPos=RotateVec(endPos,-attached.eulerAngles.z);
				hooked=true;
			}
		}
	}
	
    // Update is called once per frame
    void Update(){
		cooldown-=Time.deltaTime;
        if(Input.GetAxis("Fire1")!=0 && cooldown<0){
			cooldown=0.3f;
			if(shot){
				GameObject.Destroy(l);
			}
			l=GameObject.Instantiate(Resources.Load("line") as GameObject);
			float d = spd+mult*Vector3.Dot(rb.velocity,transform.up);
			v=(cam.ScreenToWorldPoint(Input.mousePosition)-transform.position);
			v=v.normalized*Mathf.Abs(d);
			endPos=transform.position;
			shot=true;
			hooked=false;
		}

		if(shot){
			endPos+=v*Time.deltaTime;
			Vector3 p= (End()+transform.position)/2;
			p.z=1;
			l.transform.position=p;
			Vector3 d = End()-transform.position;
			d.z=0;
			l.transform.localScale = new Vector3(1,d.magnitude,1);
			float theata = Mathf.Atan(-d.x/d.y)*180/Mathf.PI;
			if(d.y<0){theata+=180;}
			l.transform.eulerAngles = new Vector3(0,0,theata);
			transform.eulerAngles = new Vector3(0,0,theata);
			if(d.magnitude/v.magnitude>3 && !hooked){
				shot=false;
				GameObject.Destroy(l);
			}
		}

		if(hooked){
			Vector3 d = End() - transform.position;
			rb.AddForce(d.normalized * elst * (d.magnitude-len)/len);
		}
    }
}
