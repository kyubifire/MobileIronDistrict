using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {
	public static BoardManager instance;

	public List<Sprite> tileSprites = new List<Sprite>(); 	// list of sprites that will be used as tile pieces

	public GameObject tile; // reference for prefab attached to game manager which will be instantiated with board

	public int xSize, ySize; 	// width and height of board

	private GameObject[,] tiles; // array for storing game board tiles

	public bool IsShifting { get; set; } //used for when a match is found and board is filling




	private bool showHint;
	public bool findHint = false;

	private List<GameObject> hintMove = new List<GameObject> ();

	public const int minMatch = 3;


	void Start () {
		instance = GetComponent<BoardManager>();
		Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
		CreateBoard(offset.x, offset.y); // pass in the bounds for tile sprite size
    }

	private void CreateBoard (float xOffset, float yOffset) {
		tiles = new GameObject[xSize, ySize];
		float startX = transform.position.x; // get starting positions for board generation
		float startY = transform.position.y;

		Sprite[] previousLeft = new Sprite[ySize];
		Sprite previousBelow = null;

		// iterate through 2D, instatiatng row and columns of board and populating it with tiles
		for (int x = 0; x < xSize; x++) {
			for (int y = 0; y < ySize; y++) {
				GameObject newTile = Instantiate(tile, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), tile.transform.rotation);
				tiles[x, y] = newTile;
				newTile.transform.parent = transform; // parent tiles to BoardManage

				List<Sprite> otherTiles = new List<Sprite>(); // Create a list of possible images for this sprite.
				otherTiles.AddRange(tileSprites); // add all images to the list
				otherTiles.Remove(previousLeft[y]); // remove tiles from left and below if the same tile
				otherTiles.Remove(previousBelow);

				Sprite newSprite = tileSprites[Random.Range(0, otherTiles.Count)]; // randomly filled grid from list
				newTile.GetComponent<SpriteRenderer>().sprite = newSprite; // set newly created sprite to randomly generated sprite

				previousLeft[y] = newSprite;
				previousBelow = newSprite;
			}
        }
    }
		
	public bool ShowHint() {
		if (findHint) {
			foreach (var tile in hintMove) {
				FlashHint ();
				showHint = true;
			}
		} else {
				showHint = false;
			}
		return showHint;
	}

	public bool FindHit() {
		int rowCount = tiles.GetLength (0);
		int colCount = tiles.GetLength (1);

		// search rows
		for (int row = 0; row < rowCount; row++) {
			// search for chain of tiles
			for (int matchStart = 0; matchStart < colCount - 3; ++matchStart) {
				// add initial tile in chain
				hintMove.Clear();
				hintMove.Add (tiles [row, matchStart]);

				for (int nextMatch = matchStart + 1; nextMatch < colCount; ++nextMatch) {
					if (tiles [row, nextMatch] == hintMove [0]) {
						hintMove.Add (tiles [row, nextMatch]);
					} else {
						break;
					}
				}

				if (hintMove.Count >= minMatch) {
					findHint = true;
				} else {
					findHint = false;
				}
			}
		}

		// search columns
		for (int col = 0; col < colCount; ++col) {
			for (int matchStart = 0; matchStart < rowCount - 3; ++matchStart) {
				hintMove.Clear ();
				hintMove.Add (tiles [matchStart, col]);

				for (int nextMatch = matchStart + 1; nextMatch < rowCount; ++nextMatch) {
					if (tiles [nextMatch, col] == hintMove [0]) {
						hintMove.Add (tiles [nextMatch, col]);
					} else {
						break;
					}
				}
			}

			if (hintMove.Count >= minMatch) {
				findHint = true;
			}
		}

		hintMove.Clear ();
		findHint = false;
		return findHint;
	}

	private void FlashHint() {
		//Instantiate (sparkles);

	}

	public IEnumerator FindNullTiles() {
		for (int x = 0; x < xSize; x++) {
			for (int y = 0; y < ySize; y++) {
				if (tiles[x, y].GetComponent<SpriteRenderer>().sprite == null) {
					yield return StartCoroutine(ShiftTilesDown(x, y));
					break;
				}
			}
		}
		for (int x = 0; x < xSize; x++) {
			for (int y = 0; y < ySize; y++) {
				tiles[x, y].GetComponent<Tile>().ClearAllMatches();
			}
		}
	}

	private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = .1f) {
		IsShifting = true;
		List<SpriteRenderer>  renders = new List<SpriteRenderer>();
		int nullCount = 0;

		for (int y = yStart; y < ySize; y++) {
			SpriteRenderer render = tiles[x, y].GetComponent<SpriteRenderer>();
			if (render.sprite == null) {
				nullCount++;
			}
			renders.Add(render);
		}

		for (int i = 0; i < nullCount; i++) {
			yield return new WaitForSeconds(shiftDelay);

			// fix for empty tile spots in top of board
			if (renders.Count == 1) {
				renders [0].sprite = GetNewSprite (x, ySize - 1);
			}
			// end of fix
			for (int k = 0; k < renders.Count - 1; k++) {
				renders[k].sprite = renders[k + 1].sprite;
				renders[k + 1].sprite = GetNewSprite(x, ySize - 1);
			}
		}
		IsShifting = false;
	}

	private Sprite GetNewSprite(int x, int y) {
		List<Sprite> otherTiles = new List<Sprite>();
		otherTiles.AddRange(tileSprites);

		if (x > 0) {
			otherTiles.Remove(tiles[x - 1, y].GetComponent<SpriteRenderer>().sprite);
		}
		if (x < xSize - 1) {
			otherTiles.Remove(tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite);
		}
		if (y > 0) {
			otherTiles.Remove(tiles[x, y - 1].GetComponent<SpriteRenderer>().sprite);
		}

		return otherTiles[Random.Range(0, otherTiles.Count)];
	}
}
