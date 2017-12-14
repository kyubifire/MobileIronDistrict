using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneScript : MonoBehaviour {
	private bool loadScene = false;
	private int scene;
	private int newScene;

	public Text loadingText;

	//public GameObject loadingBar;
	public GameObject gear;

	void Start() {
		scene = SceneManager.GetActiveScene ().buildIndex;
		newScene = scene + 1;

		gear = (GameObject)Instantiate (gear);
		InvokeRepeating ("RotateGear", 0.5f, 0.5f);
	}

	// Updates once per frame
	void Update() {

		// If the player has pressed the space bar and a new scene is not loading yet...
		if (loadScene == false) {
			loadScene = true;
			loadingText.text = "Loading...";
			StartCoroutine(LoadNewScene());
		}

		if (loadScene == true) {
			// make text pulse by changing transparency
			loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
		}
	}

	IEnumerator LoadNewScene() {
		yield return new WaitForSeconds (3);
		AsyncOperation async = SceneManager.LoadSceneAsync(newScene);

		// While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
		while (!async.isDone) {
			yield return null;
		}
	}

	void RotateGear() {
		gear.transform.Rotate(new Vector3(0,0,90));
	}
}
