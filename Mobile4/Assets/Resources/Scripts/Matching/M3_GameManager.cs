using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class M3_GameManager : MonoBehaviour {
	public static M3_GameManager instance;
	public Tile tile;
	public M3_Enemy enemyObj;
	public M3_Player playerObj;

	public Button closeBtn;	
	public Button reloadBtn;
	//public Button moveOnBtn;

	// UI
	public GameObject instructions;
//	public GameObject winObj;
//	public GameObject lossObj;

	public bool gameOver;
	public bool gameStarted;

	public int sceneIdx;


	// Use this for initialization
	void Start () {
		instance = GetComponent<M3_GameManager> ();

		gameStarted = false;

		sceneIdx = SceneManager.GetActiveScene ().buildIndex;

		//tile = GetComponent<Tile> ();
		playerObj = GetComponent<M3_Player> ();
		enemyObj = GetComponent<M3_Enemy> ();
//
//		instructions.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
//		instructions.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);
//
//		winObj.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
//		winObj.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);
//
//		winObj.SetActive (false);
//
//		lossObj.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
//		lossObj.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);
//
//		lossObj.SetActive (false);

	}
	
	// Update is called once per frame
	void Update () {
		
		
		Debug.Log ("** IN GAME MANAGER **");
		Debug.Log ("IS GAME OVER FOR MANAGER? " + gameOver);
		closeBtn.onClick.AddListener (CloseInstructions);
		reloadBtn.onClick.AddListener (ReloadLevel);


//		if (tile.gameEnd == true) {
//			Debug.Log ("GAME IS OVER************");
//			if (enemyObj.GetComponent<M3_Enemy> ().dead) {
//				Debug.Log ("showing win screen");
//				// display win screen
//				Instantiate (winObj);
//			} else if (playerObj.GetComponent<M3_Player> ().dead) {
//				Debug.Log ("showing loss screen");
//				Instantiate (lossObj);
//				// else display lose screen
//			} 
//		}
	}

	void CloseInstructions() {
		Debug.Log ("** CLOSE BUTTON CLICKED** ");
		instructions.SetActive(false);
		gameStarted = true;
	}

	void ReloadLevel() {
		SceneManager.LoadScene (sceneIdx);
	}
}
