using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	private Animator anim;
	private SpriteRenderer render;

	public GameObject missile;
	public GameObject bomb;
	public Transform launchPoint;
	public Transform launchPoint2;
	public AudioSource source;

	public float speed;
	public float playerRange;
	public PlayerController player;

	private float shotCounter;
	public float waitBetweenShots;

	private float bombCounter;
	public float waitBetweenBombs;

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

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		render = GetComponent<SpriteRenderer> ();

		flashLength = 0.5f;
		origColor = render.color;

		shotCounter = waitBetweenShots;
		bombCounter = waitBetweenBombs;

		enemyCurrHealth = enemyMaxHealth;
		attackDamage = 10;
		onGround = true;
		//InvokeRepeating("decreasingHealth", 1f, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (!dead) {
			Debug.Log ("ENEMY HEALTH: " + enemyCurrHealth);
			ShootMissileAtPlayer ();
			DropBomb ();
			shotCounter -= Time.deltaTime;
			bombCounter -= Time.deltaTime;

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

	public void setEnemyHealth(float damage) {
		if (!dead) {
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

	void ShootMissileAtPlayer() {
		// right side
//		if (transform.localScale.x < 0 && player.transform.position.x > transform.position.x && player.transform.position.x < transform.position.x + playerRange) {
//			Instantiate (missile, launchPoint.position, launchPoint.rotation);
//		}

		// left side
		if (!dead) {
			if (transform.localScale.x > 0 && player.transform.position.x < transform.position.x && player.transform.position.x > transform.position.x - playerRange && shotCounter < 0) {
				Instantiate (missile, launchPoint.position, launchPoint.rotation);
				shotCounter = waitBetweenShots; //reset counter
			}
		}
	}

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

	// function to test health bar in game
	void decreasingHealth() {
		Debug.Log ("testing health bar");
		enemyCurrHealth -= 10f;
		Debug.Log("player current health: " + enemyCurrHealth);
		float newHealth = enemyCurrHealth / enemyMaxHealth;
		enemyBar.transform.localScale = new Vector3 (newHealth, enemyBar.transform.localScale.y, enemyBar.transform.localScale.z);
	}
}
