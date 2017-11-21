using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

	// Other scripts will need to access BoardManager so create a static reference which
	// allows it to be called from any script
	public static BoardManager instance;

	// list of sprites that will be used as tile pieces
	public List<Sprite> characters = new List<Sprite>();

	// reference for prefab attached to game manager which will be instantiated with board
	public GameObject tile;

	// width and height of board
	public int xSize, ySize;

	// array for storing game board tiles
	private GameObject[,] tiles;

	//used for when a match is found and board is filling
	public bool IsShifting { get; set; }

	void Start () {
		//source.playOnAwake();
		instance = GetComponent<BoardManager>();

		Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
		// pass in the bounds for tile sprite size
        CreateBoard(offset.x, offset.y);
    }

	private void CreateBoard (float xOffset, float yOffset) {
		tiles = new GameObject[xSize, ySize];

		// get starting positions for board generation
        float startX = transform.position.x;
		float startY = transform.position.y;

		Sprite[] previousLeft = new Sprite[ySize];
		Sprite previousBelow = null;

		// iterate through 2D, instatiatng row and columns of board and populating it with tiles
		for (int x = 0; x < xSize; x++) {
			for (int y = 0; y < ySize; y++) {
				GameObject newTile = Instantiate(tile, new Vector3(startX + (xOffset * x), startY + (yOffset * y), 0), tile.transform.rotation);
				tiles[x, y] = newTile;
				// parent tiles to BoardManage
				newTile.transform.parent = transform;

				// Create a list of possible images for this sprite.
				List<Sprite> possibleCharacters = new List<Sprite>();
				// add all images to the list
				possibleCharacters.AddRange(characters);
				// remove tiles from left and below if the same tile
				possibleCharacters.Remove(previousLeft[y]);
				possibleCharacters.Remove(previousBelow);

				// randomly filled grid from list
				Sprite newSprite = characters[Random.Range(0, possibleCharacters.Count)];
				// set newly created sprite to randomly generated sprite
				newTile.GetComponent<SpriteRenderer>().sprite = newSprite;

				previousLeft[y] = newSprite;
				previousBelow = newSprite;
			}
        }
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

	private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = .08f) {
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
		List<Sprite> possibleCharacters = new List<Sprite>();
		possibleCharacters.AddRange(characters);

		if (x > 0) {
			possibleCharacters.Remove(tiles[x - 1, y].GetComponent<SpriteRenderer>().sprite);
		}
		if (x < xSize - 1) {
			possibleCharacters.Remove(tiles[x + 1, y].GetComponent<SpriteRenderer>().sprite);
		}
		if (y > 0) {
			possibleCharacters.Remove(tiles[x, y - 1].GetComponent<SpriteRenderer>().sprite);
		}

		return possibleCharacters[Random.Range(0, possibleCharacters.Count)];
	}
}
