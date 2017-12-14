using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePiece : MonoBehaviour {
	private int x;
	private int y;

	private static Color selectedColor = new Color(.5f, .5f, .5f, 1.0f);
	public int X {
		get { return x; }
		set {
			if (IsMovable ()) {
				x = value;
			}
		}
	}

	public int Y {
		get { return y; }
		set {
			if (IsMovable ()) {
				y = value;
			}
		}
	}

	private Board.TileType type;
	public Board.TileType Type {
		get { return type; }
	}

	private Board board;
	public Board boardRef {
		get { return board; }
	}

	private MoveableTile movable;
	public MoveableTile Movable {
		get { return movable; }
	}

	private TileColor spriteColor;
	public TileColor SpriteColor {
		get { return spriteColor; }
	}

	private ClearTile clearedTile;
	public ClearTile ClearedTile {
		get { return clearedTile; }
	}

	void Awake() {
		movable = GetComponent<MoveableTile> ();
		spriteColor = GetComponent<TileColor> ();
		clearedTile = GetComponent<ClearTile> ();
	}

	public void Init (int _x, int _y, Board _board, Board.TileType _type) {
		x = _x;
		y = _y;
		board = _board;
		type = _type;
	}

	void OnMouseEnter() {
		board.Select (this); // "this" references gameobject script is on
		//spriteColor.GetComponent<SpriteRenderer>().color = selectedColor;
	}

	void OnMouseDown() {
		board.SwitchTile (this);
	}

	void OnMouseUp() {
		board.DoSwap ();

	}

	public bool IsMovable() {
		return movable != null;
	}

	public bool IsColored() {
		return spriteColor != null;
	}

	public bool IsClearable() {
		return clearedTile != null;
	}
}
