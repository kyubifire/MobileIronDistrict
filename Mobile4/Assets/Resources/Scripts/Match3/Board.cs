using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {
	public int xDim;
	public int yDim;
	public float fillTime;

	private bool inverse;
	public M3_Manager manager;
	public bool gameStarted;

	private TilePiece selectedTile;
	private TilePiece selectedTile2;

	public TilePrefab[] tilePrefabs;

	private Dictionary<TileType, GameObject> tileDict;

	private TilePiece[,] tiles;

	public enum TileType {
		Bad,
		Empty,
		Count,
		Normal,
	};

	[System.Serializable]
	public struct TilePrefab {
		//public TileColor tColor; // tile color
		public TileType type;
		public GameObject prefab; // actual gameobject
	};
		
	// ========================================//
	// SFX
	private AudioSource source;
	public AudioClip swapSound;
	public AudioClip selectSound;
	public AudioClip clearSound;

	GameObject playerObj;
	GameObject enemyObj;
	//public PGB pgb;

	void Awake() {
		
		inverse = false;
		gameStarted = false;
		// has keys of tile color and value of gameobject
		tileDict = new Dictionary<TileType, GameObject> (); // initialize new dictionary

		for (int i = 0; i < tilePrefabs.Length; i++) { // fill dictionary
			if (!tileDict.ContainsKey(tilePrefabs[i].type)) {
				tileDict.Add(tilePrefabs[i].type, tilePrefabs[i].prefab);
			}
		}

		tiles = new TilePiece[xDim, yDim];

		for (int x = 0; x < xDim; x++) {
			for (int y = 0; y < yDim; y++) {
				SpawnNewTile (x, y, TileType.Empty);
			}
		}
		// THROUGH IN PIECES WHICH ARE IMMOVABLE AND SERVE AS OBJECTS
		// FOR PLAYER
		int randX = Random.Range (1, 11); // random xpos
		int randY = Random.Range (1, 3); // random ypos

		int randX2 = Random.Range (1, 11);
		int randY2 = Random.Range (1, 3);

		int randX3 = Random.Range (1, 11);
		int randY3 = Random.Range (1, 3);

		int randX4 = Random.Range (1, 11);
		int randY4 = Random.Range (1, 3);

//		Destroy (tiles [randX, randY].gameObject); // destroy normal tile first
//		SpawnNewTile (randX, randY, TileType.Bad); // put in obstacle tile

//		Destroy (tiles [randX2, randY2].gameObject);
//		SpawnNewTile (randX2, randY2, TileType.Bad);
//
//		Destroy (tiles [randX3, randY3].gameObject);
//		SpawnNewTile (randX3, randY3, TileType.Bad);
//
//		Destroy (tiles [randX4, randY4].gameObject);
//		SpawnNewTile (randX4, randY4, TileType.Bad);

			//StartCoroutine (Fill ());
	}

	void Start() {
		source = GetComponent<AudioSource> ();
		playerObj = GameObject.FindGameObjectWithTag ("Player");
		enemyObj = GameObject.FindGameObjectWithTag("Enemy");


		//pgb = (PGB)Instantiate(pgb);
		//pgb.gameObject.SetActive (false);
	}

	void Update() {
		gameStarted = manager.GetComponent<M3_Manager> ().gameStarted;
		Debug.Log ("Has game Started? " + gameStarted);

		if (gameStarted) {
			StartCoroutine (Fill ());
			gameStarted = false;
		}
	}

	public IEnumerator Fill() { // fills whole board
		bool needToRefill = true;
		while (needToRefill) {
			yield return new WaitForSeconds (fillTime);
			while (FillStep ()) {
				inverse = !inverse;
				yield return new WaitForSeconds (fillTime);
			}
			needToRefill = ClearAllMatches ();
		}
	}

	public bool FillStep() { // fills in piece by piece (incrementally)
		bool movedTile = false;

		for (int y = yDim-2; y >= 0; y--) { //loop through from bottom (ignore bottommost row) to top, column to row to see what can be moved down
			for (int _x = 0; _x < xDim; _x++) { // 0 is at top
				int x = _x;

				if (inverse) {
					x = xDim - 1 - _x;
				}

				TilePiece tile = tiles [x, y];

				if (tile.IsMovable ()) {
					TilePiece tileBelow = tiles [x, y + 1];
					if (tileBelow.Type == TileType.Empty) { // found empty piece, fill in
						Destroy (tileBelow.gameObject);
						tile.Movable.Move (x, y + 1, fillTime);
						tiles [x, y + 1] = tile;
						SpawnNewTile (x, y, TileType.Empty); 
						movedTile = true;
					} else {
						for (int diag = -1; diag <= 1; diag++) {
							if (diag != 0) {
								int diagX = x + diag;
								if (inverse) {
									diagX = x - diag;
								}

								if (diagX >= 0 && diagX < xDim) {
									TilePiece diagonalTile = tiles [diagX, y + 1];

									if (diagonalTile.Type == TileType.Empty) {
										bool above = true;

										for (int newY = 0; newY >= 0; newY--) {
											TilePiece tileAbove = tiles[diagX, newY];
											if (tileAbove.IsMovable ()) {
												break;
											} else if (!tileAbove.IsMovable () && tileAbove.Type != TileType.Empty) {
												above = false;
												break;
											}
										}

										if (!above) {
											Destroy (diagonalTile.gameObject);
											tile.Movable.Move (diagX, y + 1, fillTime);
											tiles [diagX, y + 1] = tile;
											SpawnNewTile (x, y, TileType.Empty);
											movedTile = true;
											break;
										}
									}
								}
							}
						}
					}
				}
			}
		}

		for (int x = 0; x < xDim; x++) {
			TilePiece tileBelow = tiles [x, 0];

			if (tileBelow.Type == TileType.Empty) {
				Destroy (tileBelow.gameObject);
				GameObject newTile = (GameObject)Instantiate(tileDict[TileType.Normal], GetPosition(x, -1), Quaternion.identity);
				newTile.transform.parent = transform;

				tiles [x, 0] = newTile.GetComponent<TilePiece> ();
				tiles [x, 0].Init (x, -1, this, TileType.Normal);
				tiles [x, 0].Movable.Move (x, 0, fillTime);
				tiles [x, 0].SpriteColor.SetColor ((TileColor.ColorType)Random.Range (0, tiles [x, 0].SpriteColor.numColors));
				movedTile = true;
			}
		}
		return movedTile;
	}
		
	public Vector2 GetPosition(int x, int y) {
		return new Vector2 (transform.position.x - xDim / 2.0f + x, transform.position.y + yDim / 2.0f - y);
	}

	public TilePiece SpawnNewTile (int x, int y, TileType type) {
		GameObject newTile = (GameObject)Instantiate ((tileDict [type]), GetPosition (x, y), Quaternion.identity);
		newTile.transform.parent = transform; // parents to Board GameObject

		tiles [x, y] = newTile.GetComponent<TilePiece> ();
		tiles [x, y].Init (x, y, this, type);

		return tiles [x, y];
	}


	public bool GetAdjacents(TilePiece tile1, TilePiece tile2) {
		// in same row, adjacent column or same column adjacent row
		return (tile1.X == tile2.X && (int)Mathf.Abs(tile1.Y - tile2.Y) == 1) 
			|| (tile1.Y == tile2.Y && (int)Mathf.Abs(tile1.X - tile2.X) == 1); // same x pos but diff of 1 in y position or same y pos but diff of 1 in x
	}
		
	public void SwapTiles(TilePiece tile1, TilePiece tile2) {
		Debug.Log ("** SWAPPING TILES **");
		// check if movable first
		if (tile1.IsMovable () && tile2.IsMovable ()) {
			tiles [tile1.X, tile1.Y] = tile2;
			tiles [tile2.X, tile2.Y] = tile1;

			if (GetMatch (tile1, tile2.X, tile2.Y) != null || GetMatch (tile2, tile1.X, tile1.Y) != null) {

				int tile1_X = tile1.X;
				int tile1_Y = tile1.Y;

				tile1.Movable.Move (tile2.X, tile2.Y, fillTime);
				tile2.Movable.Move (tile1_X, tile1_Y, fillTime);
				source.PlayOneShot (swapSound);

				ClearAllMatches ();
				StartCoroutine (Fill ());

			} else {
				tiles [tile1.X, tile1.Y] = tile1;
				tiles [tile2.X, tile2.Y] = tile2;
			}
		}
	}

	public void Select(TilePiece tile) {
		Debug.Log ("** TILE SELECTED **");
		selectedTile = tile;
		//selectedTile.GetComponent<SpriteRenderer> ().color = selectedColor;
	}

	public void SwitchTile(TilePiece tile) {
		Debug.Log ("** SECOND TILE SELECTED **");
		selectedTile2 = tile;
	}

	public void DoSwap() {
		Debug.Log ("** CHECKING IF ADJACENT **");
		if (GetAdjacents (selectedTile, selectedTile2)) {
			Debug.Log ("** GETTING READY TO SWAP TILES **");
			SwapTiles (selectedTile, selectedTile2);
		}
	}

	public List<TilePiece> GetMatch(TilePiece tile, int posX, int posY) {
		if (tile.IsColored ()) {
			TileColor.ColorType color = tile.SpriteColor.CType;
			List<TilePiece> horizontalTiles = new List<TilePiece> ();
			List<TilePiece> verticalTiles = new List<TilePiece> ();
			List<TilePiece> matchingTiles = new List<TilePiece> ();

			// horizontal check
			horizontalTiles.Add(tile);

			for (int dir = 0; dir <= 1; dir++) {
				for (int xOffset = 1; xOffset < xDim; xOffset++) {
					int x;
					if (dir == 0) { //left
						x = posX - xOffset;
					} else { //right
						x = posX + xOffset;
					}

					if (x < 0 || x >= xDim) {
						break;
					}

					if (tiles [x, posY].IsColored () && tiles [x, posY].SpriteColor.CType == color) {
						horizontalTiles.Add (tiles [x, posY]);
					} else {
						break; // no more tiles found
					}
				}
			}

			if (horizontalTiles.Count >= 3) {
				for (int i = 0; i < horizontalTiles.Count; i++) {
					matchingTiles.Add (horizontalTiles [i]);
				}
			}

			// comparing vertical AND horizontal match
			if (horizontalTiles.Count >= 3) {
				for (int i = 0; i < horizontalTiles.Count; i++) {
					for (int j = 0; j <= 1; j++) {
						for (int yOffset = 1; yOffset < yDim; yOffset++) {
							int y;

							if (j == 0) { //up
								y = posY - yOffset;
							} else {
								y = posY + yOffset;
							}

							if (y < 0 || y >= yDim) {
								break;
							}

							if (tiles[horizontalTiles[i].X, y].IsColored() && tiles[horizontalTiles[i].X, y].SpriteColor.CType == color) {
								verticalTiles.Add(tiles[horizontalTiles[i].X,y]);
							} else {
								break;
							}
						}
					}

					if (verticalTiles.Count < 2) {
						verticalTiles.Clear();
					} else {
						for (int j = 0; j < verticalTiles.Count; j++) {
							//pgb.gameObject.SetActive (true);
							//pgb.changeState(1);
							matchingTiles.Add(verticalTiles[j]);
						}

						break;
					}
				}
			}

			if (matchingTiles.Count >= 3) {
				playerObj.GetComponent<M3_Player> ().Attack ();
				return matchingTiles;
			}

			// vertical check
			horizontalTiles.Clear();
			verticalTiles.Clear ();
			verticalTiles.Add(tile);

			for (int dir = 0; dir <= 1; dir++) {
				for (int yOffset = 1; yOffset < yDim; yOffset++) {
					int y;
					if (dir == 0) { // up
						y = posX - yOffset;
					} else { // down
						y = posX + yOffset;
					}

					if (y < 0 || y >= yDim) {
						break;
					}

					if (tiles [posX, y].IsColored () && tiles [posX, y].SpriteColor.CType == color) {
						verticalTiles.Add (tiles [posX, y]);
					} else {
						break; // no more tiles found
					}
				}
			}

			if (verticalTiles.Count >= 3) {
				for (int i = 0; i < verticalTiles.Count; i++) {
					matchingTiles.Add (verticalTiles [i]);
				}
			}

			// comparing vertical AND horizontal match
			if (verticalTiles.Count >= 3) {
				for (int i = 0; i < verticalTiles.Count; i++) {
					for (int j = 0; j <= 1; j++) {
						for (int xOffset = 1; xOffset < xDim; xOffset++) {
							int x;

							if (j == 0) { //up
								x = posX - xOffset;
							} else {
								x = posX + xOffset;
							}

							if (x < 0 || x >= yDim) {
								break;
							}

							if (tiles[x, verticalTiles[i].Y].IsColored() && tiles[x, verticalTiles[i].Y].SpriteColor.CType == color) {
								horizontalTiles.Add(tiles[x, verticalTiles[i].Y]);
							} else {
								break;
							}
						}
					}

					if (horizontalTiles.Count < 2) {
						horizontalTiles.Clear();
					} else {
						for (int j = 0; j < horizontalTiles.Count; j++) {
							//pgb.gameObject.SetActive (true);
							//pgb.changeState(1);
							matchingTiles.Add(horizontalTiles[j]);
						}

						break;
					}
				}
			}

			if (matchingTiles.Count >= 3) {
				playerObj.GetComponent<M3_Player> ().Attack ();
				return matchingTiles;
			}
		}
		return null;
	}

	public bool ClearAllMatches() {
		bool refillBoard = false;
		for (int y = 0; y < yDim; y++) {
			for (int x = 0; x < xDim; x++) {
				if (tiles [x, y].IsClearable ()) {
					Debug.Log ("get matched tiles to clear from board");
					List<TilePiece> matchedTiles = GetMatch (tiles [x, y], x, y);

					if (matchedTiles != null) {
						for (int i = 0; i < matchedTiles.Count; i++) {
							if (ClearTile (matchedTiles [i].X, matchedTiles [i].Y)) {
								Debug.Log ("clearing tiles");
								refillBoard = true;
							}
						}
					}
				}
			}
		}
		return refillBoard;
	}

	public bool ClearTile(int x, int y) {
		if (tiles [x, y].IsClearable() && !tiles [x, y].ClearedTile.Cleared) {
			tiles [x, y].ClearedTile.Clear ();
			source.PlayOneShot (clearSound);
			SpawnNewTile (x, y, TileType.Empty);
			Debug.Log ("tiles cleared !!");
			return true;
		}
		return false;
	}
}
