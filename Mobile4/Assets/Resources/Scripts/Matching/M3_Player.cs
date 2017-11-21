using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class M3_Player : MonoBehaviour {
	public M3_Player instance;
	public Animator anim;

	public GameObject playerHealthBar;
	public GameObject powerUpBar;

	private float calc_playerHealth;

	float playerAttackDamage = 10f;
	public float playerMaxHealth = 100f;
	public float playerCurrHealth = 0f;
	public float barAmount = 0f;
	public float totalAmount = 50f;


	float totalAttackTime = 1.000f;
	float attackTime;
	bool attacking = false;
	public bool dead = false;
	// Audio and Sound Effects;
	public AudioClip playerAttackSound;
	public AudioClip fillSound;
	public AudioClip completeFill;
	private AudioSource source;


	GameObject enemyObj;

	// Use this for initialization
	void Start () {
		instance = GetComponent<M3_Player> ();
		anim = GetComponent<Animator> ();
		source = GetComponent<AudioSource> ();

		playerCurrHealth = playerMaxHealth;

		// set power bar to 0 in x so bar is empty at start of game
		powerUpBar.transform.localScale = new Vector3(0f, powerUpBar.transform.localScale.y, powerUpBar.transform.localScale.z);

		// create reference for enemy 
		enemyObj = GameObject.FindGameObjectWithTag("Enemy");


		// function call to test health bars
		//InvokeRepeating("decreasingHealth", 1f, 1f);

		// function call to test power up bar
		//InvokeRepeating("increaseBar", 1f, 1f);
	}

	// Update is called once per frame
	void Update () {
		if (attacking && Time.time > attackTime) {
			anim.SetBool ("IsAttacking", false);
			attacking = false;
		}
	}

	public void setPlayerHealth(float damage) {
		//Debug.Log ("Amount of Damage taken from player health: " + damage);
		playerCurrHealth -= damage;
		//Debug.Log ("player current health: " + playerCurrHealth);
		float newHealth = playerCurrHealth / playerMaxHealth;
		//Debug.Log ("changing playerhealth bar by factor:" + newHealth);
		if (newHealth > 0) {
			playerHealthBar.transform.localScale = new Vector3 (newHealth, playerHealthBar.transform.localScale.y, playerHealthBar.transform.localScale.z);
		} else {
			playerHealthBar.transform.localScale = new Vector3 (0f, playerHealthBar.transform.localScale.y, playerHealthBar.transform.localScale.z);
			powerUpBar.transform.localScale = new Vector3 (0f, powerUpBar.transform.localScale.y, powerUpBar.transform.localScale.z);
			anim.SetBool ("Dead", true);
			dead = true;
		}
	}

	public void setPowerBar(float newAmount) {
		//Debug.Log ("** adding power up bar: " + newAmount);
		barAmount += newAmount;
		//Debug.Log ("plaer bar amount: " + barAmount);
		float calcAmount = barAmount / totalAmount;
		//Debug.Log ("** adding power up bar by factor: " + calcAmount);

		if (barAmount < totalAmount) {
			source.PlayOneShot (fillSound);
			powerUpBar.transform.localScale = new Vector3 (calcAmount, powerUpBar.transform.localScale.y, powerUpBar.transform.localScale.z);
		} 
		if (barAmount >= totalAmount && calcAmount  == 1) {
			source.PlayOneShot (completeFill);
			anim.SetBool("IsAttacking", true);
			attacking = true;
			enemyObj.GetComponent<M3_Enemy> ().setEnemyHealth (playerAttackDamage);
			attackTime = Time.time + totalAttackTime;
			// reset power bar
			powerUpBar.transform.localScale = new Vector3 (0, powerUpBar.transform.localScale.y, powerUpBar.transform.localScale.z);
			barAmount = 0;
			source.PlayOneShot (playerAttackSound);
		}
	}

	// function to test health bar in game
	void decreasingHealth() {
		Debug.Log ("testing health bar");
		playerCurrHealth -= 10f;
		Debug.Log("player current health: " + playerCurrHealth);
		playerHealthBar.transform.localScale = new Vector3 (barAmount, powerUpBar.transform.localScale.y, powerUpBar.transform.localScale.z);
	}

	// function to test power up bar
	void increaseBar() {
		Debug.Log ("testing player power bar");
		barAmount += 10f;
		Debug.Log ("current player bar amount: " + barAmount);
		powerUpBar.transform.localScale = new Vector3 (barAmount, powerUpBar.transform.localScale.y, powerUpBar.transform.localScale.z);
	}
}
