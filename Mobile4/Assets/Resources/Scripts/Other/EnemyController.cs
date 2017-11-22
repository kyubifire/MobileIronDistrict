using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	private Animator anim;
	private SpriteRenderer render;

	public GameObject missile;
	public GameObject bomb;
	public GameObject fist;
	public Transform launchPoint;
	public Transform launchPoint2;

	public float speed;
	public float playerRange;
	public PlayerController player;

	private float shotCounter;
	public float waitBetweenShots;

	private float bombCounter;
	public float waitBetweenBombs;

	private float punchCounter;
	public float waitBetweenPunches;

	public int attackDamage;  // regular attack outside of missiles

	public GameObject enemyBar;
	private float calc_EnemyHealth;
	public float enemyMaxHealth = 100f;
	public float enemyCurrHealth = 0f;
	public bool dead = false;
	public bool onGround;

	private bool flashActive;
	public float flashLength;
	private float flashCounter;
	private Color origColor;

	//SFX
	public AudioSource source;
	public AudioClip hitSound;
	public AudioClip attackSound;


	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		render = GetComponent<SpriteRenderer> ();
		source = GetComponent<AudioSource> ();

		flashLength = 0.5f;
		origColor = render.color;

		shotCounter = waitBetweenShots;
		bombCounter = waitBetweenBombs;

		enemyCurrHealth = enemyMaxHealth;
		attackDamage = 10;
		onGround = true;

		//fist.SetActive (false);
		//InvokeRepeating("decreasingHealth", 1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (!dead) {
			//Debug.Log ("ENEMY HEALTH: " + enemyCurrHealth);
			//ShootMissiles();
			//DropBomb ();
			//PunchPlayer ();
			shotCounter -= Time.deltaTime;
			bombCounter -= Time.deltaTime;
			punchCounter -= Time.deltaTime;

			// make player flash red when hit by changing RGB values of sprite
			if (flashActive) {
				Debug.Log ("ENEMY SPRITE CHANGING");
				if (flashCounter > flashLength * .66f) {
					render.color = new Color (0.5f, 0.5f, 0.5f, render.color.a);
				} else if (flashCounter > flashLength * .33f) {
					render.color = origColor;
				} else if (flashCounter > 0f) {
					render.color = new Color (0.5f, 0.5f, 0.5f, render.color.a);
				} else {
					render.color = origColor; // back to normal
					flashActive = false;
				}
				flashCounter -= Time.deltaTime;
			}

			if (enemyCurrHealth <= 50) {
				anim.speed = 1.5f;
			} else if (enemyCurrHealth <= 25) {
				anim.speed = 2.0f;
			}
		}
	}



	IEnumerator GetReady() {
		yield return new WaitForSeconds (2);
		anim.SetBool ("Prepping", true);
	}

	// set a timer for sentinel to perodically pause leaving him open for attack
	IEnumerator Paused() {
		Debug.Log ("Sentinel is taking a break");
		anim.SetBool ("Paused", true);
		yield return new WaitForSeconds (2);
		anim.SetBool ("Paused", false);
	}

//	IEnumerator Attack () {
//		anim.SetBool ("IsAttack", true);
//
//	}

//	void Attack() {
//		anim.SetBool ("Attacking", true);
//		Pun
//	}

	void PunchPlayer() {
		anim.SetBool ("IsAttacking", true);
//		fist.SetActive (true);
		if (transform.localScale.x > 0 && player.transform.position.x < transform.position.x && player.transform.position.x > transform.position.x - playerRange && punchCounter < 0) {
			Instantiate (fist, launchPoint.position, launchPoint.rotation);
			punchCounter = waitBetweenPunches; //reset counter
		}
	}
//
//	void ShootMissiles() {
//		// right side
////		if (transform.localScale.x < 0 && player.transform.position.x > transform.position.x && player.transform.position.x < transform.position.x + playerRange) {
////			Instantiate (missile, launchPoint.position, launchPoint.rotation);
////		}
//		anim.SetBool("IsAttacking", true);
//		// left side
//		if (!dead) {
//			if (transform.localScale.x > 0 && player.transform.position.x < transform.position.x && player.transform.position.x > transform.position.x - playerRange && shotCounter < 0) {
//				Instantiate (missile, launchPoint.position, launchPoint.rotation);
//				shotCounter = waitBetweenShots; //reset counter
//			}
//		}
//	}

	void DropBomb() {
		Debug.Log ("DROPPING BOMB");
		if (!dead) {
			if (transform.localScale.x > 0 && player.transform.position.x < transform.position.x && player.transform.position.x > transform.position.x - playerRange && bombCounter < 0) {
				Instantiate (bomb, launchPoint2.position, launchPoint2.rotation);
				bombCounter = waitBetweenBombs; //reset counter
				Debug.Log("DROPPED BOMB");
			}
		}
	}

	public void setEnemyHealth(float damage) {
		if (!dead) {
			source.PlayOneShot (hitSound);
			Debug.Log ("Amount of Damage taken from enemy health: " + damage);
			enemyCurrHealth -= damage;
			flashActive = true;
			flashCounter = flashLength;
			//Debug.Log ("player current health: " + playerCurrHealth);
			float newHealth = enemyCurrHealth / enemyMaxHealth;
			//Debug.Log ("changing playerhealth bar by factor:" + newHealth);
			if (newHealth > 0) {
				enemyBar.transform.localScale = new Vector3 (newHealth, enemyBar.transform.localScale.y, enemyBar.transform.localScale.z);
			} else {
				enemyBar.transform.localScale = new Vector3 (0f, enemyBar.transform.localScale.y, enemyBar.transform.localScale.z);
				anim.SetBool ("Dead", true);
				dead = true;
			}
		}
	}

	// function to test health bar in game
	void decreasingHealth() {
		Debug.Log ("testing health bar");
		enemyCurrHealth -= 10f;
		Debug.Log("player current health: " + enemyCurrHealth);
		float newHealth = enemyCurrHealth / enemyMaxHealth;
		enemyBar.transform.localScale = new Vector3 (newHealth, enemyBar.transform.localScale.y, enemyBar.transform.localScale.z);
	}
}
