using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour {


	public GameObject[] gameModes;
	private int gameMode;
	public Player player;


	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			Application.LoadLevel(Application.loadedLevel);
		}



		//change the key code to whatever button you want to start the game
		if (gameMode == 0) {
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				gameMode = 1;
				Instantiate(gameModes[0]);
				player.canMove = true;

			}
			else if (Input.GetKeyDown (KeyCode.Return)) {
				gameMode = 2;
				Instantiate(gameModes[1]);
			}


		}

	
	}
}
