using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelScript : MonoBehaviour {
	public GameObject fist;
	public GameObject sentinel;

	bool attacking;
	bool firing;
	float attackStart;

	bool hit;
	float waitTime;
	bool paused;

	Animator fistor;
	SpriteRenderer fistRndr;
	Animator sentinor;

	SpriteRenderer senRender;
	public PlayerController player;

	public float maxHealth = 100f;
	public float senHealth = 0f;
	public bool dead = false;
	public int fistDamage;

	// effect for damage done to sentinel
	private bool flashActive;
	public float flashLength;
	private float flashCounter;
	private Color origColor;

	//SFX
	public AudioSource source;
	public AudioClip boing;
	public AudioClip hitSound;
	public AudioClip attackSound;


	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		fistor = fist.GetComponent<Animator> ();
		sentinor = sentinel.GetComponent<Animator> ();
		fistRndr = fist.GetComponent<SpriteRenderer> ();

		fistRndr.color = new Color(1,1,1,0);
		attackStart = -5;

		senHealth = maxHealth;

		player = FindObjectOfType<PlayerController> ();
		senRender = sentinel.GetComponent<SpriteRenderer> ();

		flashLength = 0.5f;
		origColor = senRender.color;

		waitTime = 3f;

		InvokeRepeating ("StartAttack", waitTime, waitTime);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("** SENTINEL'S HEALTH **" + senHealth);

		//if (fistor.GetCurrentAnimatorStateInfo(0).IsName("fist")) {
		//	StartCoroutine(Pause());
		//}

		// make player flash when hit by changing RGB values of sprite
		if (flashActive) {
			Debug.Log ("SENTINEL SPRITE CHANGING");
			if (flashCounter > flashLength * .66f) {
				senRender.color = new Color (0.5f, 0.5f, 0.5f, senRender.color.a);
			} else if (flashCounter > flashLength * .33f) {
				senRender.color = origColor;
			} else if (flashCounter > 0f) {
				senRender.color = new Color (0.5f, 0.5f, 0.5f, senRender.color.a);
			} else {
				senRender.color = origColor; // back to normal
				flashActive = false;
			}
			flashCounter -= Time.deltaTime;
		}

		//plays animation on any key press
		//if (Input.anyKeyDown && !attacking && Time.time > attackStart + 4.5) {
		//	Attack ();
		//	attackStart = Time.time;
		//}
			

		//checks if mid animation
		if (attacking) {
			Debug.Log ("Preparing Fist");
			//show fists and activates its aniamtion
			if (Time.time > attackStart + 2.25f && !firing) {
				fistor.SetTrigger ("Fire!");
				fistRndr.color = new Color (1, 1, 1,1);
				firing = true;
				OnTriggerEnter2D (player.GetComponent<Collider2D>());
				//resets the parameters and hides the fist
			} else if (Time.time > attackStart + 3.4f) {
				attacking = false;
				firing = false;
				fistRndr.color = new Color(1,1,1,0);
				//StartCoroutine (Pause ());
			}
		}
	}

	public void StartAttack() {
		if (!attacking && Time.time > attackStart + 4.5) {
			Attack ();
			attackStart = Time.time;
		}
	}

	//triggers the sentinel attack animation(call this whe you need to attack)
	public void Attack() {
		Debug.Log ("ATTACKING");
		attackStart = Time.time;
		sentinor.SetTrigger ("Attack!");
		attacking = true;
	}

//	public void Pause() {
//		Debug.Log ("SENTINEL HAS PAUSED");
//		sentinor.SetTrigger ("Pause!");
//	}

//	IEnumerator Pause() {
//		//paused = true;
//		firing = false;
//		attacking = false;
//		Debug.Log ("SENTINEL HAS PAUSED");
//		sentinor.SetTrigger ("Pause!");
//		yield return new WaitForSeconds (4);
//
//		firing = true;
//		attacking = true;
//		yield break;
//
//	}

	void OnTriggerEnter2D(Collider2D other) {
		// to detect if fist hits player
		if (other.tag == "Player") {
			Debug.Log ("** PUNCHED PLAYER **");
			source.PlayOneShot (boing);
			// player lose health
			player.GetComponent<PlayerController>().setPlayerHealth(fistDamage);
		}
	}

	public void setEnemyHealth(float damage) {
		Debug.Log ("Damage to Sentinel: " + damage);
		if (!dead) {
			source.PlayOneShot (hitSound);
			Debug.Log ("Amount of Damage taken from enemy health: " + damage);
			senHealth -= damage;
			flashActive = true;
			flashCounter = flashLength;

			if (senHealth <= 50) {
				waitTime = 2f;
			} else if (senHealth <= 25) {
				waitTime = 1f;
			}
		}



		if (senHealth <= 0) {
			sentinor.SetBool ("Dead", true);
			dead = true;
		}
	}
}
