using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelScript : MonoBehaviour {

	public GameObject fist;
	public GameObject sentinel;
	public GameObject healthBar;
	public GameObject bomb;

	bool attacking;
	bool firing;
	float attackStart;
	bool hitPlayer;

	bool hit;
	float waitTime;
	bool paused;

	Animator fistor;
	SpriteRenderer fistRndr;
	Animator sentinor;
	BoxCollider2D fistBox;


	SpriteRenderer senRender;
	public PlayerController player;


	Animator bombAnim;

	public float maxHealth = 100f;
	public float senHealth = 0f;
	public bool dead = false;
	public int fistDamage;

	// effect for damage done to sentinel
	private bool flashActive;
	private float flashLength;
	private float flashCounter;
	private Color origColor;

	//SFX
	public AudioSource source;
	public AudioClip boing;
	public AudioClip hitSound;
	//public AudioClip attackSound;

	public Manager manager;
	public bool gameStarted;
	int which;
	// Use this for initialization
	void Start () {
		gameStarted = false;

		source = GetComponent<AudioSource> ();
		sentinor = sentinel.GetComponent<Animator> ();

		fistor = fist.GetComponent<Animator> ();
		fistRndr = fist.GetComponent<SpriteRenderer> ();
		fistBox = fist.GetComponent<BoxCollider2D> ();

		fistRndr.color = new Color(1,1,1,0);


		attackStart = -5;
		fistDamage = 20;
		senHealth = maxHealth;

		player = FindObjectOfType<PlayerController> ();
		senRender = sentinel.GetComponent<SpriteRenderer> ();


		bombAnim = bomb.GetComponent<Animator> ();
		flashLength = 0.5f;
		origColor = senRender.color;

		waitTime = 2f;

		InvokeRepeating ("StartAttack", waitTime, waitTime);

	}


	// Update is called once per frame
	void Update () {
		gameStarted = manager.gameStarted;
		if (!dead) {
			//Debug.Log ("** SENTINEL'S HEALTH **" + senHealth);
			if (gameStarted) {
				// make player flash when hit by changing RGB values of sprite
				if (flashActive) {
					Debug.Log ("SENTINEL SPRITE CHANGING");
					if (flashCounter > flashLength * .66f) {
						senRender.color = new Color (0.5f, 0.5f, 0.5f, senRender.color.a);
					} else if (flashCounter > flashLength * .33f) {
						senRender.color = origColor;
					} else if (flashCounter > 0f) {
						senRender.color = new Color (0.7f, 0.5f, 0.5f, senRender.color.a);
					} else {
						senRender.color = origColor; // back to normal
						flashActive = false;
					}
					flashCounter -= Time.deltaTime;
				}

				if (gameStarted) {
					//checks if mid animation
					if (attacking) {

						//creates a variable that radomb=ly chooses between fist and bomb
						//>50 == bomb

						//Debug.Log ("Preparing Fist");

						//show fists and activates its aniamtion
						if (Time.time > attackStart + 2.25f && !firing) {
							if (which <= 40) {
								fistor.SetTrigger ("Fire!");
								fistRndr.color = new Color (1, 1, 1, 1);
								firing = true;
								//OnTriggerEnter2D (player.GetComponent<Collider2D> ());
							} else {
								//OnTriggerEnter2D (player.GetComponent<Collider2D> ());
								Instantiate (bomb);
								firing = true;
							}
							//resets the parameters and hides the fist
						} else if (Time.time > attackStart + 2.4f && Time.time < attackStart + 2.7f) {
							if (which <= 40) {
								if (player.transform.position.x > -1.8f && player.transform.position.y < .5f && !hitPlayer) {
									player.setPlayerHealth (fistDamage);
									hitPlayer = true;
								}
							}

						} else if (Time.time > attackStart + 3.4f) {
							attacking = false;
							firing = false;
							fistRndr.color = new Color (1, 1, 1, 0);
							hitPlayer = false;
							//StartCoroutine (Pause ());
						} 
					}
				}
			}
		}
	}

	public void StartAttack() {
		if(gameStarted){
			if (!attacking && Time.time > attackStart + 4.5) {
				Attack ();
				attackStart = Time.time;
			}

		}
	}
	/// <summary>
	/// Breya is hit if x < -1.8 and y < .5
	/// </summary>
	/// 

	//triggers the sentinel attack animation(call this whe you need to attack)
	public void Attack() {
		//Debug.Log ("ATTACKING");
		if (!dead) {
			attackStart = Time.time;
			sentinor.SetTrigger ("Attack!");
			attacking = true;
			which = Random.Range (0, 100);
		}
	}

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
		Debug.Log (" ???? Damage to Sentinel: " + damage);
		if (!dead) {
			source.PlayOneShot (hitSound);
			senHealth -= damage;
			flashActive = true;
			flashCounter = flashLength;

			if (senHealth <= 50) {
				waitTime = 1f;
			} else if (senHealth <= 25) {
				waitTime = .5f;
			}
		}
			
		if (senHealth > 0) {
			healthBar.transform.localScale = new Vector3 (senHealth / maxHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
		}else{
			healthBar.transform.localScale = new Vector3 (senHealth / maxHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
			sentinor.SetBool ("Dead", true);
			dead = true;


			//remove this later
			Destroy((GameObject)sentinel);
		}
	}
}