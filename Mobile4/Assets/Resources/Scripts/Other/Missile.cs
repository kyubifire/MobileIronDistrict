using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {
	Rigidbody2D missileRB;
	public float speed;
	public PlayerController player;
	public float playerHealth;
	public float rotationSpeed;
	public int damage;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController> ();
		playerHealth = player.GetComponent<PlayerController> ().playerCurrHealth;
		missileRB = GetComponent<Rigidbody2D> ();

		// to the left of missile
		if (player.transform.position.x < transform.position.x) {
			speed = -speed;
			rotationSpeed = -rotationSpeed;
		}
	}
	
	// Update is called once per frame
	void Update () {
		missileRB.velocity = new Vector2 (speed, missileRB.velocity.y);
		missileRB.angularVelocity = rotationSpeed;
	}

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("Amount of Damage from missile: " + damage);
		if (other.tag == "Player") {
			Debug.Log ("** MISSILE HIT PLAYER **");
			// player lose health
			player.GetComponent<PlayerController>().setPlayerHealth(damage);
		}
		Destroy (gameObject);
	}
}
