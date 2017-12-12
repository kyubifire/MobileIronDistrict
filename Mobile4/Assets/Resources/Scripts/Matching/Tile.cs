using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Tile : MonoBehaviour {
	public static Tile instance;
	private static Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);
	private static Tile previousSelected = null;

	private SpriteRenderer render;
	private bool isSelected = false;
	public bool matchFound = false;

	private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

	// SFX for tiles
	private AudioSource source;
	public AudioClip swapSound;
	public AudioClip selectSound;
	public AudioClip clearSound;
	//*********************************************************************\\
	GameObject playerObj;
	GameObject enemyObj;

	public int multiplier;   			// After getting matches back to back, player can get a multiplier value added to attackDamage
	public bool canAttack;
	public bool enemyAttacking;

	public bool win;
	public bool loss;
	public bool gameEnd;
	public bool gameStarted;

	private int numSwap;

	//GameObject timerObj;
	//public float time;
	//private bool timeUp;
	private int sceneIdx;

	// variables for checking & getting instructions to pop
	private int showOnce;
	private bool displayText;
	public GUIText instructionsText;

	void Awake() {
		render = GetComponent<SpriteRenderer>();
		source = GetComponent<AudioSource> ();
	}

	void Start () {
		instance = GetComponent<Tile> ();
		sceneIdx = SceneManager.GetActiveScene ().buildIndex;

		canAttack = false;
		enemyAttacking = false;
		win = false;
		loss = false;
		gameEnd = false;
		gameStarted = false;

		playerObj = GameObject.FindGameObjectWithTag ("Player");
		enemyObj = GameObject.FindGameObjectWithTag("Enemy");
		//Debug.Log (playerObj);
		//Debug.Log (enemyObj);

		numSwap = 0;
		showOnce = 1;
		displayText = true;

//		instructions = GameObject.FindGameObjectWithTag ("Instructions");
//		winObj = GameObject.FindGameObjectWithTag ("winObj");
//		lossObj = GameObject.FindGameObjectWithTag ("lossObj");
	}

	void Update () {
		if (numSwap >= 2) {
			enemyObj.GetComponent<M3_Enemy> ().AttackPlayer ();
			numSwap = 0; //reset
		}
//		if (!gameStarted) {
//			if (Input.anyKey) {
//				instructions.SetActive (false);
//				gameStarted = true;
//			}
//		}
//
//		if (enemyObj.GetComponent<M3_Enemy> ().dead) {
//			gameEnd = true;
//			// display win screen
//			winObj.SetActive(true);
//
//		} else if (playerObj.GetComponent<M3_Player>().dead) {
//			gameEnd = true;
//			// else display lose screen
//			lossObj.SetActive(true);
//		} 
	}

	private void Select() {
		isSelected = true;
		render.color = selectedColor;
		previousSelected = gameObject.GetComponent<Tile> ();
		source.PlayOneShot (selectSound);
		Debug.Log ("Selected tile: " + previousSelected.render);
	}

	private void Deselect() {
		isSelected = false;
		render.color = Color.white;
		previousSelected = null;
	}

	// "Queries Start in Colliders" must be uncheck.
	// Edit -> Project Settings -> Physics 2D -> UNCHECK Queries Start in Colliders
	void OnMouseDown() {
			// not selectable conditions
			if (render.sprite == null || BoardManager.instance.IsShifting) {
				return;
			}

		if (isSelected) {
			Deselect ();
		} else {
			if (previousSelected == null) {
				Select ();
			} else {
				//Debug.Log ("Previously Selected: " + previousSelected);
				//Debug.Log ("Checking Adjacents: " + GetAllAdjacentTiles ().Contains (previousSelected.gameObject));
				if (GetAllAdjacentTiles ().Contains (previousSelected.gameObject)) {
					SwapSprite (previousSelected.render);
					previousSelected.ClearAllMatches ();
					previousSelected.Deselect ();
					ClearAllMatches ();
				} else {
					previousSelected.GetComponent<Tile> ().Deselect ();
					Select ();
				}
			}
		}

	}

	public void SwapSprite(SpriteRenderer render2) {
		//Debug.Log (" ** IN SWAPSPRITE **");
		if (render.sprite == render2.sprite) {
			return;
		}
		Sprite tempSprite = render2.sprite;
		render2.sprite = render.sprite;
		render.sprite = tempSprite;
		//Debug.Log("** NEW SPRITE: " + render.sprite);
		source.PlayOneShot (swapSound);

		if (!matchFound) {
			numSwap += 1;
		}
	}

	private GameObject GetAdjacent(Vector2 castDir) {
		//Debug.Log ("** IN GET ADJACENT **");
		RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);
		//Debug.Log (hit.collider);
		if (hit.collider != null) {
			//Debug.Log ("Collider not null: " + hit.collider.gameObject);
			return hit.collider.gameObject;
		}
		return null;
	}

	private List<GameObject> GetAllAdjacentTiles() {
		//Debug.Log (" ** IN GET ALL ADJACENT TILES");
		List<GameObject> adjacentTiles = new List<GameObject>();
		for (int i = 0; i < adjacentDirections.Length; i++) {
			//Debug.Log("Adjacent Directions: " + adjacentDirections[i]);
			adjacentTiles.Add(GetAdjacent(adjacentDirections[i]));
		}
		//Debug.Log ("Adjacent tiles: " + adjacentTiles);
		return adjacentTiles;
	}

	private List<GameObject> FindMatch(Vector2 castDir) {
		List<GameObject> matchingTiles = new List<GameObject>();
		RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);
		while (hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == render.sprite) {
			matchingTiles.Add(hit.collider.gameObject);
			hit = Physics2D.Raycast(hit.collider.transform.position, castDir);
		}
		return matchingTiles;
	}

//	private void ClearMatch(Vector2[] paths) {
//		List<GameObject> matchingTiles = new List<GameObject>();
//		for (int i = 0; i < paths.Length; i++) { 
//			matchingTiles.AddRange(FindMatch(paths[i])); 
//		}
//		if (matchingTiles.Count >= 2) {
//			for (int i = 0; i < matchingTiles.Count; i++) {
//				matchingTiles[i].GetComponent<SpriteRenderer>().sprite = null;
//			}
//			matchFound = true;
//			playerObj.GetComponent<M3_Player> ().Attack ();
//		}
//		//if (matchFound == true) {
//		//	playerObj.GetComponent<M3_Player> ().Attack ();
//		//}
//	}


	private void ClearMatch(Vector2[] paths) {
		List<GameObject> matchingTiles = new List<GameObject>();
		//List<> matchingTiles = new List<>();
		for (int i = 0; i < paths.Length; i++) { 
			matchingTiles.AddRange(FindMatch(paths[i])); 
		}
		if (matchingTiles.Count >= 2) {
			bool goodMatch = false;

			for (int i = 0; i < matchingTiles.Count; i++) {
				if (matchingTiles [i].GetComponent<SpriteRenderer> ().sprite == BoardManager.instance.tileSprites [0] || matchingTiles [i].GetComponent<SpriteRenderer> ().sprite == BoardManager.instance.tileSprites [1]) {
					goodMatch = true;
				} else {
					goodMatch = false;
				}
			}

			if (goodMatch) {
				Tile[] allTiles = FindObjectsOfType<Tile>();
				for (int j = 0;  j < allTiles.Length; j++) {
					if (allTiles[j].transform.position.x == matchingTiles[0].transform.position.x && allTiles[j].transform.position.x == matchingTiles[1].transform.position.x) {
						matchingTiles.Add(allTiles[j].gameObject);
					} else if (allTiles[j].transform.position.y == matchingTiles[0].transform.position.y && allTiles[j].transform.position.y == matchingTiles[1].transform.position.y) {
						matchingTiles.Add(allTiles[j].gameObject);
					} else {
						continue;
					}
				}
			}

			for (int i = 0; i < matchingTiles.Count; i++) {
				matchFound = true;
				matchingTiles[i].GetComponent<SpriteRenderer>().sprite = null;
				playerObj.GetComponent<M3_Player> ().Attack ();
			}
		}
	}

	public void ClearAllMatches() {
		if (render.sprite == null)
			return;
		ClearMatch(new Vector2[2] { Vector2.left, Vector2.right });
		ClearMatch(new Vector2[2] { Vector2.up, Vector2.down });
		if (matchFound) {
			render.sprite = null;
			matchFound = false;
			StopCoroutine(BoardManager.instance.FindNullTiles());
			StartCoroutine(BoardManager.instance.FindNullTiles());
			source.PlayOneShot (clearSound);
		}
	}
}