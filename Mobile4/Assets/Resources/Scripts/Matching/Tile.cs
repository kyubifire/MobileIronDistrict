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

	// UI
	public GameObject winObj;
	public GameObject lossObj;
	//*********************************************************************\\
	GameObject playerObj;
	GameObject enemyObj;

	public int moveCounter;  		// after # of moves, enemy power bar gets added to
	public int multiplier;   			// After getting matches back to back, player can get a multiplier value added to attackDamage
	public int numSwaps;    			// After 3 swaps and no matches, enemy attacks -> penalty for player
	public bool canAttack;
	public bool enemyAttacking;

	public bool win;
	public bool loss;

	//GameObject timerObj;
	//public float time;
	//private bool timeUp;
	private int sceneIdx;

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
		//timeExpired = false;
		//multiplier = 0;
		moveCounter = 0;

		playerObj = GameObject.FindGameObjectWithTag ("Player");
		enemyObj = GameObject.FindGameObjectWithTag("Enemy");
		//Debug.Log (playerObj);
		//Debug.Log (enemyObj);

		winObj.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
		winObj.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);

		lossObj.transform.localScale = new Vector3 (0.02f, 0.02f, 0.02f);
		lossObj.transform.position = new Vector3 (Screen.width/768f, Screen.height/768f, 0f);

		//InvokeRepeating ("TimeAttack", 1f, 1f);
	}

	void Update () {
		//time -= Time.deltaTime;
		//Debug.Log (time);

		if (enemyObj.GetComponent<M3_Enemy> ().dead) {
			//win = true;
			// display win screen
			Instantiate (winObj);

		} else if (playerObj.GetComponent<M3_Player>().dead) {
			//loss = true;
			Instantiate (lossObj);
			// else display lose screen
		} 

//		if (win) {
//			Debug.Log ("YOU WON!!");
//			Instantiate (winObj);
//		} else if (loss) {
//			Debug.Log ("YOU LOST !!");
//			Instantiate (lossObj);
//		}

		if (Input.GetKeyDown(KeyCode.Return)) {
			// return to start
			SceneManager.LoadScene(sceneIdx + 1);
		}

		if (moveCounter == 2) {
			Debug.Log ("Player made two moves, filling enemy power bar");
			enemyObj.GetComponent<M3_Enemy>().setPowerBar(10f);
			moveCounter = 0;
			Debug.Log ("move Counter: " + moveCounter);
		}
	}

	private void Select() {
		isSelected = true;
		render.color = selectedColor;
		previousSelected = gameObject.GetComponent<Tile>();
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
					moveCounter++;
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
		numSwaps += 1;
		Debug.Log ("Num Swaps: " + numSwaps);
		if (numSwaps >= 2) {
			enemyObj.GetComponent<M3_Enemy>().setPowerBar(10f);
			numSwaps = 0;		// reset
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

	private void ClearMatch(Vector2[] paths) {
		List<GameObject> matchingTiles = new List<GameObject>();
		for (int i = 0; i < paths.Length; i++) { 
			matchingTiles.AddRange(FindMatch(paths[i])); 
		}
		if (matchingTiles.Count >= 2) {
			for (int i = 0; i < matchingTiles.Count; i++) {
				matchingTiles[i].GetComponent<SpriteRenderer>().sprite = null;
			}
			matchFound = true;
		}
		if (matchFound == true) {
			playerObj.GetComponent<M3_Player>().setPowerBar(10f);
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
			moveCounter++;
		}
	}
		
//	void TimeAttack() {
//		if (time <= 0) {
//			//Debug.Log (timerObj.GetComponent<CombatTimer> ().timeExpired);
//			Debug.Log ("TIME RAN OUT!! ENEMY ATTACKING!!");
//			enemyAttacking = true;
//			enemyAnim.SetBool ("IsAttacking", true);
//			enemyAnim.SetTrigger ("IsAttacking");
//
//			playerCurrHealth -= enemyAttackDamage;
//			calc_playerHealth = playerCurrHealth / playerMaxHealth;  // new scale by which to set health bar
//			playerObj.GetComponent<M3_Player> ().setPlayerHealth (calc_playerHealth);
//			source.PlayOneShot (enemyAttackSound);
//
//			//Debug.Log ("Player Health: " + playerCurrHealth);
//			if (playerCurrHealth <= 0) {
//				playerAnim.SetBool ("Dead", true);
//				//SceneManager.LoadScene(0);
//				loss = true;
//			}
//			Debug.Log ("STOPPING ENEMY ATTACK");
//			Invoke ("StopAttack", 1);
//		}
//	}
}