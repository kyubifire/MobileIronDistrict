﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FallingGearGame : MonoBehaviour
{
	public int time;
	private double nextSpawnTime;
	public Gear falling;
	private int health;
	private int maxHealth;
	public int wantGear;
	public int Score = 0;

	public thoughtBubble thought;
	public GameObject victory;
	public GameObject defeat;
	public GameObject player;
	public GameObject healthBar;
	public GameObject backgrounds;
	public GameObject progress;
	private bool endGame;
	private bool gameStart;

	public GameObject instructions;
	public Button moveOnButton;
	public Button reloadButton1;
	public Button reloadButton2;
	public Button closeBtn;

	private int sceneIdx;
	// Use this for initialization
	void Start () {
		gameStart = false;
		//player = (GameObject)Instantiate (player);
		healthBar = (GameObject)Instantiate (healthBar);
		healthBar.transform.position -= new Vector3 (3, 0, 0);

		health = 10;
		maxHealth = 10;
		Instantiate (backgrounds);
		nextSpawnTime = Time.time;
		progress = (GameObject)Instantiate (progress);
		endGame = false;
		thought = (thoughtBubble)Instantiate (thought);
		wantGear = (int)Random.Range (1, 4);
		thought.changeState (wantGear);

		falling.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f); //gears too large, shrink them

		sceneIdx = SceneManager.GetActiveScene ().buildIndex;
	}
	
	// Update is called once per frame
	void Update () { 

		closeBtn.onClick.AddListener (CloseInstructions);

		if (endGame && Input.GetMouseButton (0)) { // back up if win/lose dont work until new phone is activated to test mobibe build -- gabby
			SceneManager.LoadScene(sceneIdx + 1);
		}

		if (gameStart == true) {

			progress.transform.localScale = new Vector3 ((float)Score / 2f, .5f, 1f);

			if (Score >= 10 && !endGame) {
				//VICTORY
				endGame = true;
				//Instantiate (victory);
				victory.SetActive(true);
			}

			if ((health <= 0) && !endGame) {
				//DEATH
				endGame = true;
				//Instantiate (defeat);
				defeat.SetActive(true);
				Destroy (healthBar);
			}
			healthBar.transform.localScale = new Vector3 ((float) 8 * health / maxHealth, .5f, 1f);

			if (Time.time >= nextSpawnTime && !endGame) {
				Instantiate (falling);
				nextSpawnTime += Random.Range (.3f, .7f);
			}

			//this check for touch controls
			if (Input.touchCount > 0) {
				Vector3 worldPos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
				Vector2 touchPos = new Vector2 (worldPos.x, worldPos.y);

				// on click it checks if overlaos with gear scripted object
				var hit = Physics2D.OverlapPoint (touchPos);
				if (hit && hit.gameObject.GetComponent<Gear> () != null) {
					int a = hit.gameObject.GetComponent <Gear> ().type;
					if (a == wantGear) {
						wantGear = Random.Range (1, 4);
						Score += 1;
						Destroy (hit.gameObject);
						thought.changeState (wantGear);
					} else if (a == 0 ) {
						health -= 20;
						Destroy (hit.gameObject);
					} else  {
						health -= 10;
						Destroy (hit.gameObject);
					}
				}
			}
		}
	}

	void CloseInstructions() {
		instructions.SetActive (false);
		gameStart = true;
	}
}
