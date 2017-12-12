using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	public enum TileColor {
		red,
		blue,
		black,
		green,
		purple,
		yellow,
	};

	[System.Serializable]
	public struct TilePrefab {
		public TileColor tColor;
		public GameObject tPrefab;

	};

	public int xDim;
	public int yDim;
	public int fillTime;


	public TilePrefab[] tilePrefabs;
	private Dictionary<TileColor, GameObject> prefabDict;

	void Awake() {
		prefabDict = new Dictionary<TileColor, GameObject> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
