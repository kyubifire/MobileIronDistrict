using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class M3_Manager : MonoBehaviour {
	public M3_Player player;
	public M3_Enemy enemy;

	public bool gameStarted;

	// UI
	public GameObject winObj;
	public GameObject lossObj;
	public GameObject instructions;
	public Button closeBtn;

	public int sceneIdx;

	// Use this for initialization
	void Start () {
		gameStarted = false;
		sceneIdx = SceneManager.GetActiveScene ().buildIndex;

		winObj.SetActive (false);
		lossObj.SetActive (false);
		
	}
	
	// Update is called once per frame
	void Update () {
		closeBtn.onClick.AddListener (CloseInstructions);

		if (enemy.GetComponent<M3_Enemy> ().dead) {
			// display win screen
			winObj.SetActive (true);
		} else if (player.GetComponent<M3_Player> ().dead) {
			// else display lose screen
			lossObj.SetActive (true);
		} 
	}

	void CloseInstructions() {
		gameStarted = true;
		instructions.SetActive (false);
	}
}
