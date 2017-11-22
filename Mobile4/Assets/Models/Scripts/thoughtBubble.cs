using System.Collections.Generic;
using UnityEngine;

public class thoughtBubble : MonoBehaviour {
	
	public int type;
	Animator anim;

	// Use this for initialization
	void Start () {
		type = 0;
		anim = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {
		this.anim.SetInteger ("State", type);
	}

	public void changeState(int type_){
		type = type_;

	}

}
