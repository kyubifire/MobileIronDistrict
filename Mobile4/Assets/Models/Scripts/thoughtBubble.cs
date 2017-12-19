using System.Collections.Generic;
using UnityEngine;

public class thoughtBubble : MonoBehaviour {
	
	public int type;
	Animator animator;

	// Use this for initialization
	void Start () {
		type = 0;
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void changeState(int type_){
		type = type_;
		animator.SetInteger ("State", type);
	}

}
