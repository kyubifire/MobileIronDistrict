using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
	Rigidbody2D bombRB;
	public float speed;
	public float fallSpeed;
	public PlayerController player;
	public float playerHealth;
	public float rotationSpeed;
	public int damage;

	public AudioClip fuse;
	public AudioClip explode;
	public AudioSource source;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController> ();
		playerHealth = player.GetComponent<PlayerController> ().playerCurrHealth;
		bombRB = GetComponent<Rigidbody2D> ();
		source = GetComponent<AudioSource> ();

		// to the left of missile
		if (player.transform.position.x < transform.position.x) {
			speed = -speed;
			rotationSpeed = -rotationSpeed;
			source.PlayOneShot (fuse);
		}
	}

	// Update is called once per frame
	void Update () {
		
		bombRB.velocity = new Vector2 (speed, bombRB.velocity.y);
		bombRB.angularVelocity = rotationSpeed;
		transform.Translate (Vector3.down * fallSpeed * Time.deltaTime, Space.Self);
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Amount of Damage from missile: " + damage);
		if (other.tag == "Player") {
			Debug.Log ("** BOMB HIT PLAYER **");
			// player lose health
			player.GetComponent<PlayerController>().setPlayerHealth(damage);
		}
		source.PlayOneShot (explode);
		Destroy (gameObject);
	}
}
