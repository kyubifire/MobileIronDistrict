using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour {
	Rigidbody2D fistRB;
	public PlayerController player;
	public float playerHealth;
	public int damage;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController> ();
		playerHealth = player.GetComponent<PlayerController> ().playerCurrHealth;
		fistRB = GetComponent<Rigidbody2D> ();

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			Debug.Log ("** PUNCHED PLAYER **");
			// player lose health
			player.GetComponent<PlayerController>().setPlayerHealth(damage);
		}
		Debug.Log ("destroy punch");
		Destroy (gameObject);
	}
}
