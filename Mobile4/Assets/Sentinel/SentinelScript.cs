using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelScript : MonoBehaviour {

	public GameObject fist;
	public GameObject sentinel;
	bool attacking;
	bool firing;
	float attackStart;

	Animator fistor;
	SpriteRenderer fistRndr;
	Animator sentinor;
	// Use this for initialization
	void Start () {
		fistor = fist.GetComponent<Animator> ();
		sentinor = sentinel.GetComponent<Animator> ();
		fistRndr = fist.GetComponent<SpriteRenderer> ();

		fistRndr.color = new Color(1,1,1,0);
		attackStart = -5;
	}
	
	// Update is called once per frame
	void Update () {


		//plays animation on any key press
		if (Input.anyKeyDown && !attacking && Time.time > attackStart + 4.5) {
			Attack ();
			attackStart = Time.time;
		}

		//checks if mid animation
		if (attacking) {

			//show fsts and activates its aniamtion
			if (Time.time > attackStart + 2.25f && !firing) {
				fistor.SetTrigger ("Fire!");
				fistRndr.color = new Color (1, 1, 1,1);
				firing = true;
			
				//resets the parameters and hides the fist
			} else if (Time.time > attackStart + 3.4f) {
				attacking = false;
				firing = false;
				fistRndr.color = new Color(1,1,1,0);
			}
		}
		
	}


	//triggers the sentinel attack animation(call this whe you need to attack)

	public void Attack(){

		sentinor.SetTrigger ("Attack!");
		attacking = true;

			
	}
}
