using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
	public Manager manager;

	Animator anim;
	Rigidbody2D rigidBody;
	SpriteRenderer render;
	public SentinelScript senPrefab;


	private bool onGround;
	private Vector3 velocity;

	// attack stuff
	float totalAttackTime = 1.000f;
	public int attackDamage;
	float attackTime;
	float attackStart;
	bool attacking = false;
	bool walking = false;


	public AudioSource source;
	public AudioClip attackSound;
	public AudioClip hitSound;

	public GameObject healthBar;
	private float calc_playerHealth;
	public float playerMaxHealth = 100f;
	public float playerCurrHealth = 0f;
	public bool dead = false;
	public bool damaged;
	public bool dying;
	public bool gameStarted;

	private bool flashActive;
	public float flashLength;
	private float flashCounter;
	private Color origColor;

	// get horizontal and vertical axes of movement
	private Vector2 moveVec;
	// button for attack
	public Button attackBtn;


	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		anim = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody2D> ();
		render = GetComponent<SpriteRenderer> ();

		origColor = render.color;
		flashLength = 0.5f;

		gameStarted = false;
		onGround = true;
		velocity = new Vector3 (.1f, 0f, 0f);

		playerCurrHealth = playerMaxHealth;
		attackDamage = 10;
		attackStart = -5;
		senPrefab = FindObjectOfType<SentinelScript> ();
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log ("game started? (playerCon)" + gameStarted);
		gameStarted = manager.GetComponent<Manager> ().gameStarted;

		if (gameStarted) {
			if (!dead) {
				//Debug.Log ("PLAYER'S HEALTH: " + playerCurrHealth);
				moveVec = new Vector2 (CrossPlatformInputManager.GetAxis ("Horizontal"), CrossPlatformInputManager.GetAxis ("Vertical"));
				rigidBody.AddForce (moveVec);

				if (CrossPlatformInputManager.GetAxis ("Horizontal") != 0) {
					Debug.Log (" *** moving stick left or right *** ");
					Debug.Log (CrossPlatformInputManager.GetAxis ("Horizontal"));
				}

			
				if (CrossPlatformInputManager.GetAxis ("Vertical") != 0) {
					Debug.Log ("*** moving stick up or down ***");
					Debug.Log (CrossPlatformInputManager.GetAxis ("Vertical"));
				}


				if (CrossPlatformInputManager.GetAxis ("Horizontal") < 0) {
					MoveLeft ();
				}


				if (CrossPlatformInputManager.GetAxis ("Horizontal") > 0) {
					MoveRight ();
				}


				if (CrossPlatformInputManager.GetAxis ("Vertical") > 0) {
					Jump ();
				}
			
				attackBtn.onClick.AddListener (Attack);

				if (attacking && Time.time > attackTime) {
					anim.SetBool ("IsAttacking", false);
					attacking = false;
				}
				
				// make player flash red when hit by changing RGB values of sprite
				if (flashActive) {
					if (flashCounter > flashLength * .66f) {
						render.color = new Color (render.color.r, 0f, 0f, render.color.a); // red
					} else if (flashCounter > flashLength * .33f) {
						render.color = origColor; // normal
					} else if (flashCounter > 0f) {
						render.color = new Color (render.color.r, 0f, 0f, render.color.a); // final red
					} else {
						render.color = origColor; // back to normal
						flashActive = false;
					}
					flashCounter -= Time.deltaTime;
				}
			}
		}
	}

	public void MoveLeft() {
		//Debug.Log ("pressed key to go left");
		if (transform.position.x > -7) {
			transform.Translate(-1 * velocity);
			anim.SetBool ("Walking", true);
			render.flipX = true;
		}
	}

	public void MoveRight() {
		//Debug.Log ("pressed key to go right");
		if (transform.position.x < 1.5f) {
			transform.Translate(velocity);
			anim.SetBool ("Walking", true);
			render.flipX = false;
		} 
	}

	public void Jump() {
		// jump
		if (onGround) {
			//Debug.Log ("pressed key to jump");
			anim.SetBool ("Jumping", true);
			//rigidBody.AddForce (new Vector2 (0, Mathf.Clamp(125, 1, 150)));
			float jumpHeight = 15f;
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpHeight);
			onGround = false;
		}
	}
		
	public void Attack() {
		Debug.Log ("pressed key to to attack");
		// attacking
		if (Time.time > attackStart + 1f) {
			source.PlayOneShot (attackSound);
			attacking = true;
			if (transform.position.x > 1 && transform.position.y < -1) {
				senPrefab.GetComponent<SentinelScript> ().setEnemyHealth (10);
			}

			anim.SetTrigger ("Attack");
			anim.SetBool ("IsAttacking", true);

			attackTime = Time.time + totalAttackTime; // set to 1 sec -- doesn't have to be accurate need to be less than the actual animation time w/ exit time -- see into using triggers as well
			attackStart = Time.time;
		}
	}


	public void setPlayerHealth(float damage) {
		if (!dead) {
			Debug.Log ("Amount of Damage taken from player health: " + damage);
			playerCurrHealth -= damage;
			flashActive = true;
			flashCounter = flashLength;
			source.PlayOneShot(hitSound);
			//Debug.Log ("player current health: " + playerCurrHealth);
			float newHealth = playerCurrHealth / playerMaxHealth;
			//Debug.Log ("changing playerhealth bar by factor:" + newHealth);
			if (newHealth > 0) {
				healthBar.transform.localScale = new Vector3 (newHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
			} else {
				healthBar.transform.localScale = new Vector3 (0f, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
				anim.SetBool ("Dead", true);
				dead = true;
			}
		}
	}
		
	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.transform.tag == "Ground" ) {
			anim.SetBool("Jumping", false);
			onGround = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Player Attack Damage: " + attackDamage);
		if (other.tag == "Enemy") {
			Debug.Log ("** PLAYER HIT ATTACKED ENEMY **");
			senPrefab.GetComponent<SentinelScript> ().setEnemyHealth (10);
			// enemy lose health
		}
	}


	// function to test health bar in game
	void decreasingHealth() {
		Debug.Log ("testing health bar");
		playerCurrHealth -= 10f;
		Debug.Log("player current health: " + playerCurrHealth);
		float newHealth = playerCurrHealth / playerMaxHealth;
		healthBar.transform.localScale = new Vector3 (newHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
	}
}
