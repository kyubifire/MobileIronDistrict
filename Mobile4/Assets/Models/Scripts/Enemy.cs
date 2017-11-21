using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public thoughtBubble thought;
	public bool canMove;
	Animator animator;
	private int state;
	private Vector3 position;


	// Use this for initialization
	void Start () {
		
		animator = GetComponent<Animator> ();
		position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (state == 100) {
			changeState (0);
		}
			else if (state == 1) {
			state = 100;
			
		}
			

	
	}
		

	public void changeState(int state_){
		animator.SetInteger ("State", state_);
		state = state_;

		//remove this part once we fix the sprites(the current grave is too high)
		if (state == 66) {
			transform.position = position + new Vector3 (0, -1.5f, 0);
		}
	}
}
