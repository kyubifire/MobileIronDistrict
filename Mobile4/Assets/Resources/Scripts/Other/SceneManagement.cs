using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour {
	public int index;
	public string levelName;

	public Image blackImg;
	public Animator anim;

	// Use this for initialization
	void Start () {
		index = SceneManager.GetActiveScene ().buildIndex;
		StartCoroutine (Fading());
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Fading () {
		anim.SetBool ("Fade", true);
		yield return new WaitUntil (() => blackImg.color.a == 1);
		SceneManager.LoadScene(index);

	}
}
