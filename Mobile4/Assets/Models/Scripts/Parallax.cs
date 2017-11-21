using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

	//create a list of backgrounds
	public GameObject[] backgrounds;
	private Vector3 velocity1;
	private Vector3 velocity2;
	private bool rotatingLeft = true;
	private Vector3 backgroundStart;
	private Vector3 backgroundEnd;

	// Use this for initialization
	void Start () {
		velocity1 = new Vector3 (.3f,0,0);
		velocity2 = new Vector3 (.5f,0,0);
		backgroundStart = backgrounds [3].transform.position;
		backgroundEnd = backgroundStart + new Vector3(3,0,0);
	}
	
	// Update is called once per frame
	void Update () {

		if (backgrounds [3].transform.position.x < backgroundStart.x || backgrounds [3].transform.position.x > backgroundEnd.x) {
			rotatingLeft = !rotatingLeft;
		}
		for (int i = 0; i < backgrounds.Length; i++) {

			//I KNOW THIS PART IS CONVOLUTED BUT I GOT LAZY
			// SO IT CHECKS SPECIFIC Z POSITIONS AND USES A PREDETERINED VELOICTY
			//ILL FIX IT LATER
			if (backgrounds [i].transform.position.z == 5) {
				if (rotatingLeft) {
							
					backgrounds [i].transform.Translate (velocity1 * Time.deltaTime);
				} else {
					backgrounds [i].transform.Translate (velocity1  * -1 * Time.deltaTime);
				}
			} else if (backgrounds [i].transform.position.z == 10) {
				if (rotatingLeft) {

					backgrounds [i].transform.Translate (velocity2 * Time.deltaTime);
				} else {
					backgrounds [i].transform.Translate (velocity2  * -1 * Time.deltaTime);
				}
			} else if(backgrounds [i].transform.position.z < 0){
				if (rotatingLeft) {

					backgrounds [i].transform.Translate (velocity1 * -1 * Time.deltaTime);
				} else {
					backgrounds [i].transform.Translate ((velocity1) * Time.deltaTime);
				}


			}


		}
		
	}
}
