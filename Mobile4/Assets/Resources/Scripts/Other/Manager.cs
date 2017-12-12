using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {
	public Camera camera;
	public PlayerController player;
	public SentinelScript senPrefab;


	public GameObject instructions;
	public GameObject win;
	public GameObject loss;

	public int chargeAmount;
	//public GameObject specialAtkBG;

	// Use this for initialization
	void Start () {

		win.SetActive (false);
		loss.SetActive (false);
		//chargeAmount = player.GetComponent<PlayerController> ().chargeAmount;
		chargeAmount = 15;

		//SpecialAttack ();
	}
	
	// Update is called once per frame
	void Update () {
		//if (chargeAmount >= 15) {
		//	SpecialAttack ();
		//}

		chargeAmount = 0;

		if (Input.anyKey) {
			instructions.SetActive (false);
		}

		if (senPrefab.GetComponent<SentinelScript> ().dead) {
			win.SetActive (true);
		}

		if (player.GetComponent<PlayerController> ().dead) {
			loss.SetActive (true);
		}
	}

//	void SpecialAttack() {
//		Instantiate (specialAtkBG);
//		camera.fieldOfView = 2000.0f;
//		player.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y+2,player.transform.position.z);
//		enemy.transform.position = new Vector3 (enemy.transform.position.x, enemy.transform.position.y+2, enemy.transform.position.z);
//	}

}
