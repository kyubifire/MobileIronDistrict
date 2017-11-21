using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTimer : MonoBehaviour {
	// timer
	public float timeRemaining = 6;
	public float WaitTime = 3f;
	public bool timeExpired;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timeRemaining -= Time.deltaTime;
		//Debug.Log ("Time left: " + timeRemaining);
		if (timeRemaining <= 0) {
			resetTimer ();
		}
		
	}

//	void OnGUI() {
//		if (timeRemaining > 0) {
//			//x, y, width, height
//			//GUI.Label (new Rect (475, 30, 300, 100), "Time: " + (int)timeRemaining);
//		} else {
//			GUI.Label (new Rect (475, 30, 300, 100), "Time's Up");
//			timeExpired = true;
//			//Debug.Log (" *** Is time expired? ***" + timeExpired);
//
//			// small delay before function call
//			StartCoroutine("resetTimer", WaitTime);
//		}
//	}

	public void resetTimer() {
		timeRemaining = 6;
	}
}
