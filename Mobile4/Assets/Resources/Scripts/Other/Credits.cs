using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {
	public Animator creditsAnim;
	private bool animPlaying;
	private float animTime = 20.0f; // time of credits animation clip
	private float creditsDone;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		Debug.Log ("IS AN ANIMATION PLAYING? "+ animPlaying);

		if (creditsAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !creditsAnim.IsInTransition(0)) {
			StartCoroutine (returnToMenu ());
		}
		
	}

	IEnumerator returnToMenu() {
		yield return new WaitForSeconds (1); // wait a little
		SceneManager.LoadScene(0);
	}
}
