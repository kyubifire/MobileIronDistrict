using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadFirstScene () {
		SceneManager.LoadScene (1);
	}

	public void LoadArcade() {

	}

	public void LoadCredits() {
		SceneManager.LoadScene (18);
	}

	public void ExitGame () {
		Application.Quit ();
	}
}
