using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColor : MonoBehaviour {
		public enum ColorType {
		red,
		any,
		blue,
		black,
		green,
		purple,
		yellow,
		count,
	};

	[System.Serializable]
	public struct tileColor {
		public ColorType color;
		public Sprite tileSprite;

	};

	private ColorType cType;
	public ColorType CType {
		get { return cType; } 
		set { SetColor (value); }
	}

	public int numColors;
	private SpriteRenderer spRender;

	public tileColor[] colorSprites;
	private Dictionary <ColorType, Sprite> spriteColorDict;

	void Awake() {
		numColors = colorSprites.Length;

		//spRender = GameObject.FindGameObjectWithTag ("Tile").GetComponent<SpriteRenderer>();
		spRender = transform.Find("tile").GetComponent<SpriteRenderer> ();;
		spriteColorDict = new Dictionary<ColorType, Sprite> ();

		for (int i = 0; i < colorSprites.Length; i++) {
			if (!spriteColorDict.ContainsKey (colorSprites[i].color)) {
				spriteColorDict.Add (colorSprites [i].color, colorSprites [i].tileSprite);
			}
		}
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetColor(ColorType newColor) {
		cType = newColor;
		if (spriteColorDict.ContainsKey(cType)) {
			spRender.sprite = spriteColorDict[cType];
		}
	}
}
