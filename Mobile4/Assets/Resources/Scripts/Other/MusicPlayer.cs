using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour {

	public AudioClip fight_theme_p1;
	public AudioClip fight_theme_p2;
	public AudioSource audio;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();
		audio.loop = true;

		StartCoroutine(playBattleMusic());
	}

	IEnumerator playBattleMusic() {
		audio.clip = fight_theme_p1;
		audio.Play();
		yield return new WaitForSeconds(audio.clip.length);
		audio.clip = fight_theme_p2;
		audio.Play();
	}

	// Update is called once per frame
	void Update () {

	}
}
