using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	Animator anim;
	Rigidbody2D rigidBody;
	SpriteRenderer render;

	public EnemyController enemy;

	public Button attackBtn;
	public Button jumpButton;

	private bool onGround;
	private Vector3 velocity;

	// attack stuff
	float totalAttackTime = 1.000f;
	public int attackDamage;
	float attackTime;
	bool attacking = false;


	public int chargeAmount;

	public AudioSource source;
	public AudioClip lowHealth;
	public AudioClip attackSound;


	public GameObject healthBar;
	private float calc_playerHealth;
	public float playerMaxHealth = 100f;
	public float playerCurrHealth = 0f;


	public bool dead;
	public bool damaged;
	public bool dying;

	private bool flashActive;
	public float flashLength;
	private float flashCounter;
	private Color origColor;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		anim = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody2D> ();
		render = GetComponent<SpriteRenderer> ();

		origColor = render.color;
		flashLength = 0.5f;


		dead = false;
		onGround = true;
		velocity = new Vector3 (.1f, 0f, 0f);

		playerCurrHealth = playerMaxHealth;
		attackDamage = 10;

		//InvokeRepeating("decreasingHealth", 1f, 1f);
	}

	// Update is called once per frame
	void Update () {
		if (!dead) {
			//Debug.Log ("PLAYER'S HEALTH: " + playerCurrHealth);

			// jump
			jumpButton.onClick.AddListener (Jump);
			
			// attacking
			attackBtn.onClick.AddListener (Attacking);
			

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

	public void setPlayerHealth(float damage) {
		if (!dead) {
			Debug.Log ("Amount of Damage taken from player health: " + damage);
			playerCurrHealth -= damage;
			flashActive = true;
			flashCounter = flashLength;

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

			//if (playerCurrHealth <= 50) {
			//	LowHealth ();
			//}
		}
	}

	void LowHealth() {
		source.PlayOneShot (lowHealth);
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
			// enemy lose health
			//enemy.GetComponent<EnemyController>().setEnemyHealth(attackDamage);
		}
	}

	void Attacking() {
		Debug.Log ("** ATTACK BUTTON PRESSED ** ");
		source.PlayOneShot (attackSound);
		attacking = true;
		enemy.GetComponent<EnemyController> ().setEnemyHealth (2);
		anim.SetBool ("IsAttacking", true);
		attackTime = Time.time + totalAttackTime; // set to 1 sec -- doesn't have to be accurate need to be less than the actual animation time w/ exit time -- see into using triggers as well
	}

	void Jump() {
		Debug.Log ("pressed key to jump");
		rigidBody.AddForce (new Vector2 (0, 75));
		onGround = false;
		anim.SetBool ("Jumping", true);
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
