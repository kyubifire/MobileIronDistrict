using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingRhythmGear : MonoBehaviour {


	public GameObject colorChanger;
	public GameObject GearRotating;
	float lastChanged;
	SpriteRenderer sprtrnd;


	// Use this for initialization
	void Start () {
		sprtrnd = colorChanger.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (sprtrnd.color != Color.white && Time.time > lastChanged + .1f) {
			sprtrnd.color = Color.white;
		}


	}


	public void changeColor(string colorC){
		if (colorC == "bad") {
			sprtrnd.color =  new Color(.5f, 0,0, .8f);
		} else if (colorC == "good") {
			sprtrnd.color = new Color (0, .5f,0, .8f);
		} else if (colorC == "neutral") {
			sprtrnd.color = Color.white;
		}
		lastChanged = Time.time;
	}
}
