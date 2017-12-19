using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bomb : MonoBehaviour {
	float timeStart;
	float currentTime;
	float timer = 1f;
	bool flash;
	SpriteRenderer sprtrndr;


	Rigidbody2D bombRB;
	private float playerHealth;
	public PlayerController player;

	public float speed;
	public float fallSpeed;


	public int damage;
	public float rotationSpeed;
	public AudioClip explode;
	public AudioSource source;
	bool soundPlaying;

	// Use this for initialization
	void Start () {
		//sets start frame to time start
		timeStart = Time.time;
		currentTime = Time.time;
		sprtrndr = GetComponent<SpriteRenderer> ();

		bombRB = GetComponent<Rigidbody2D> ();
		player = FindObjectOfType<PlayerController> ();
		playerHealth = player.GetComponent<PlayerController> ().playerCurrHealth;
		source = GetComponent<AudioSource> ();

		// to the left of bomb
		if (player.transform.position.x < transform.position.x) {
			speed = -speed;
			rotationSpeed = -rotationSpeed;
			//source.PlayOneShot (fuse);
		}
	}
	
	// Update is called once per frame
	void Update () {

		bombRB.velocity = new Vector2 (speed, bombRB.velocity.y);
		bombRB.angularVelocity = rotationSpeed;
		transform.Translate (Vector3.down * fallSpeed * Time.deltaTime, Space.Self);

		//creates a blingke, and every time it gets faster
		if (Time.time - timer >= currentTime && ! (timer <= .05f)) {
			sprtrndr.color = Color.red;
			if (Time.time - timer >= currentTime + .05f) {
				sprtrndr.color = Color.white;
				currentTime = Time.time;
				timer *= .75f;
			}
		}

		//once it blinks too fast, it initiates explosion
		if (timer <= .1f && GetComponent<Animator> ().GetInteger("State") != 1) {

			timeStart = Time.time;
			sprtrndr.color = Color.white;
			GetComponent<Animator> ().SetInteger ("State", 1);
		}

		//after 1 second after the explosion start it destroys the bomb
		if (GetComponent<Animator> ().GetInteger ("State") == 1 && Time.time - timeStart > 1) {
			Destroy (gameObject);


		}
		if (GetComponent<Animator> ().GetInteger ("State") == 1 && Time.time - timeStart > .2) {
			if (!soundPlaying) {
				source.PlayOneShot (explode);
				soundPlaying = true;
			}
		}
	}

	// check for collision with player
	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Amount of Damage from bomb: " + damage);
		if (other.tag == "Player") {
			Debug.Log ("** BOMB HIT PLAYER **");
			// player lose health
			player.GetComponent<PlayerController> ().setPlayerHealth (damage);
			Physics2D.IgnoreCollision (GetComponent<Collider2D> (), player.GetComponent<Collider2D> ());
			bombRB.velocity = new Vector2(0,0);
			//after player is hit, go ahead and start exloding animation

			GetComponent<Animator>().SetTrigger("Explode!");
			GetComponent<Animator> ().SetInteger ("State", 1);
			timeStart = Time.time;
			//Destroy (gameObject);
			}
		}
		//source.PlayOneShot (explode);
}
