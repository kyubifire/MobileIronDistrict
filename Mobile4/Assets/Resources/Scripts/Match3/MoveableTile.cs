using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableTile : MonoBehaviour {
	private TilePiece tile;
	private IEnumerator moveTile;
	void Awake() {
		tile = GetComponent<TilePiece> ();
	}

	public void Move(int posX, int posY, float moveSpeed) {
//		tile.X = posX;
//		tile.Y = posY;
//
//		tile.transform.localPosition = tile.boardRef.GetPosition (posX, posY);

		if (moveTile != null) {
			StopCoroutine (moveTile);
		}

		moveTile = MoveTile (posX, posY, moveSpeed);
		StartCoroutine (moveTile);
	}

	private IEnumerator MoveTile(int x, int y, float moveSpeed) {
		tile.X = x;
		tile.Y = y;

		Vector3 startPos = transform.position;
		Vector3 endPos = tile.boardRef.GetPosition (x, y);

		for (float time = 0; time <= moveSpeed * 1; time += Time.deltaTime) {
			tile.transform.position = Vector3.Lerp (startPos, endPos, time / moveSpeed);
			yield return 0;
		}
		tile.transform.position = endPos;

	}
}
