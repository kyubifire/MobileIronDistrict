using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour {
	public AudioSource audio;
	public float startVolume;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
		audio.volume = 0.0f;
		startVolume = 0.0f;
	}

	// Update is called once per frame
	void Update () {
		fadeIn();
	}

	void fadeIn()
	{
		if (audio.volume < 1)
		{
			startVolume += 0.2f * Time.deltaTime;
			audio.volume = startVolume;
		}
	}
}