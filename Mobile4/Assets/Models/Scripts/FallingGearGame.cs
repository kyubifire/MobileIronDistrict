using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class FallingGearGame : MonoBehaviour {
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
	public GameObject healthBar;
	public GameObject player;
	public GameObject progress;
	public bool gameStart;
	private bool endGame;

	public GameObject gearThing;
	public GameObject[] gearThingList = new GameObject[10];

	public GameObject instructions;
	public Button closeBtn;

	// Use this for initialization
	void Start () {
		maxHealth = 50;
		health = maxHealth;
		player = (GameObject)Instantiate (player);

		healthBar = (GameObject)Instantiate (healthBar);
		healthBar.transform.position -= new Vector3 (4, -3, 0);
		healthBar.transform.Translate(new Vector3(-3,0,0));
		healthBar.transform.Rotate (new Vector3 (0, 0, 90));
		healthBar.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		//Instantiate (backgrounds);
		nextSpawnTime = Time.time;

		progress = (GameObject)Instantiate (progress);
		progress.transform.Translate (new Vector3 (-9, 0, 0));
		progress.transform.Rotate(new Vector3 (0,0,90));

		endGame = false;
		gameStart = false;
		thought = (thoughtBubble)Instantiate (thought);
		wantGear = Random.Range (1, 4);


		for (int i = 0; i < 10; i++) {
			gearThingList [i] = Instantiate ((GameObject) gearThing);
			gearThingList [i].transform.position = new Vector2 (gearThingList [i].transform.position.x + .9f * i, gearThingList [i].transform.position.y);

			if (i % 2 == 1) {
				gearThingList[i].GetComponent<SpriteRenderer> ().flipX = true;
				gearThingList [i].transform.rotation = Quaternion.Euler (0, 0, -28.25f);
			}
		}

		victory.SetActive (false);
		defeat.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

		closeBtn.onClick.AddListener (CloseInstructions);

		if (gameStart) {

			progress.transform.localScale = new Vector3 ((float)Score / 2f, .5f, 1f);

			if (Score > 0 && !endGame) {
				for (int i = 0; i < Score; i++) {
					gearThingList [i].transform.position = new Vector3 (-6f + .9f * i, gearThingList [i].transform.position.y, -6);
				}
			}

			//VICTORY
			if (Score >= 10 && !endGame) {
				endGame = true;
				victory.SetActive (true);
			}

			//DEATH
			if ((health <= 0) && !endGame) {
				endGame = true;
				defeat.SetActive (true);
				Destroy (healthBar);
			}


			thought.type = wantGear;
			thought.changeState (thought.type);

			healthBar.transform.localScale = new Vector3 ((float)8 * health / maxHealth, .5f, 1f);

	
			if (Time.time >= nextSpawnTime && !endGame) {
				Instantiate (falling);
				nextSpawnTime += Random.Range (.8f, 1.5f);
			}
	
			//this check for touch controls
			if (Input.touchCount > 0) {
				Vector3 worldPos = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
				Vector2 touchPos = new Vector2 (worldPos.x, worldPos.y);


				//on click it checks if overlaos with gear scripted object
				var hit = Physics2D.OverlapPoint (touchPos);
				if (hit && hit.gameObject.GetComponent<Gear> () != null) {
					int a = hit.gameObject.GetComponent <Gear> ().type;
					if (a == wantGear) {
						wantGear = Random.Range (1, 4);
						Score += 1;
						Destroy (hit.gameObject);

					} else if (a == 0) {
						health -= 20;
						Destroy (hit.gameObject);

					} else {
						health -= 10;
						Destroy (hit.gameObject);
					}
				}
				
			}
		}
	}


	void CloseInstructions() {
		gameStart = true;
		instructions.SetActive (false);
		nextSpawnTime = Time.time;
	}
}