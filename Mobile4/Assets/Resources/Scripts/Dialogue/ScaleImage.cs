using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleImage : MonoBehaviour {
	SpriteRenderer renderer;
	float width;
	float height;
	float cameraH;
	float cameraW;


	// Use this for initialization
	void Start () {
		resizeImage ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void resizeImage() {
		renderer = GetComponent<SpriteRenderer> ();
		Debug.Log (renderer);

		cameraH = Camera.main.orthographicSize * 2;
		cameraW = cameraH / Screen.height * Screen.width;

		transform.localScale = new Vector3(cameraW / renderer.sprite.bounds.size.x,cameraH/ renderer.sprite.bounds.size.y, 1);
	}
}
