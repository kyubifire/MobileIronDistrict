using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour {
	public static Instructions instance;

	private bool showText = false;
	public bool gameStarted = true;

	private float currentTime = 0.0f;
	float executedTime = 0.0f;
	private float timeToWait = 5.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		currentTime = Time.time;
		showText = true;

		if (executedTime != 0.0f) {
			if (currentTime - executedTime > timeToWait) {
				executedTime = 0.0f;
			}
		}
	}

	void OnMouseDown() {
		executedTime = Time.time;
	}

	void OnGUI() {
		if (showText) {
			GUI.Label (new Rect (0, 0, 100, 100), "RANDOM TEXT");
		}
		
	}
}
