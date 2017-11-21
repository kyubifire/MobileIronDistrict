using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class M3_GameManager : MonoBehaviour {
	public GameObject tile;

	// UI
	//public GameObject winObj;
	//public GameObject lossObj;
	public GameObject instructions;
	public GameObject instructionsClone;

	public bool win;
	public bool loss;
	public bool gameOver;
	public bool gameStarted;


	// Use this for initialization
	void Start () {

		win = false;
		loss = false;
		gameOver = false;
		gameStarted = false;

		instructions.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
		instructions.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);
			
		//winObj.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
		//winObj.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);

		//lossObj.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
		//lossObj.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);

		instructionsClone = Instantiate (instructions);

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) || Input.GetKeyDown(KeyCode.Space)) {
			Destroy (instructionsClone);
		}

		//gameOver = tile.GetComponent<Tile> ().gameOver;
		//win = tile.GetComponent<Tile> ().win;
		//loss = tile.GetComponent<Tile> ().loss;
	}
}
