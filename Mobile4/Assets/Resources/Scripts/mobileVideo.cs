using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class mobileVideo : MonoBehaviour {
	//public VideoClip video;

	// Use this for initialization
	void Start () {
		Handheld.PlayFullScreenMovie ("intro.mp4", Color.black, FullScreenMovieControlMode.Minimal);
		
	}
}
