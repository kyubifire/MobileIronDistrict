using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGB : MonoBehaviour {
	SpriteRenderer sprtrndr;
	Animator animator;
	int state;
	public float shown;
	float rotationFactor;
	float opacity;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		sprtrndr = GetComponent<SpriteRenderer> ();
	}

	// Update is called once per frame
	void Update () {

		if (state != 0 && Time.time > shown + .1) {
			transform.position = new Vector3 (0, 3, -9);
			rotationFactor += 1;
			if (Time.time > shown + .5 && Time.time < shown + 1) {
				transform.Rotate(0, 0, Time.deltaTime*730);
				sprtrndr.color = new Color(1,1,1,.2f*rotationFactor);
			}
			if (Time.time > shown + 2.5) {
				changeState (0);
			}

		} else {

			transform.position = new Vector3 (0, 3, 15);
		}

	}

	public void changeState(int state_){
		animator.SetInteger ("State", state_);
		state = state_;
		shown = Time.time;
		sprtrndr.color = new Color(1,1,1,0);
		rotationFactor = 0;

	}
}
