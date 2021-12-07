using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour {
	
	//particlesystem visible in the inspector
	public ParticleSystem dieEffect;
	
	void Update(){
	//set positions to start rays from
	Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
	Vector3 pos1 = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
	//check for player character with 2 rays and than play attack animation
	if((Physics.Raycast(pos, transform.right, 12) || Physics.Raycast(pos1, transform.right, 12)) && !GetComponent<Animation>().IsPlaying("Attack")){
	GetComponent<Animation>().CrossFade("Attack");
	}
	}
	
	void OnTriggerEnter(Collider other){
	if(other.gameObject.name == "bullet"){
	//if this enemy gets hit by a bullet, destroy it and destroy the bullet
	Vector3 pos = other.gameObject.transform.position;
	Instantiate(dieEffect, pos, transform.rotation);
	Destroy(other.gameObject);
	Destroy(gameObject);
	}	
	}
}
