using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Video : MonoBehaviour {
	//public RawImage image;
	public VideoClip videoToPlay;

	private VideoPlayer vdp;
	private VideoSource vds;

	//Audio
	private AudioSource source;

	private bool videoDone = false;
	private int sceneIdx;

	// Use this for initialization
	void Start()
	{
		sceneIdx = SceneManager.GetActiveScene ().buildIndex;
		Application.runInBackground = true;
		StartCoroutine(playVideo());
	}

	IEnumerator playVideo()
	{
		//Add VideoPlayer to the GameObject
		vdp = gameObject.AddComponent<VideoPlayer>();

		//Add AudioSource
		source = gameObject.AddComponent<AudioSource>();

		//Disable Play on Awake for both Video and Audio
		vdp.playOnAwake = true;
		vdp.playOnAwake = true;

		//We want to play from video clip not from url
		vdp.source = VideoSource.VideoClip;

		//Set Audio Output to AudioSource
		vdp.audioOutputMode = VideoAudioOutputMode.AudioSource;

		//Assign the Audio from Video to AudioSource to be played
		vdp.EnableAudioTrack(0, true);
		vdp.SetTargetAudioSource(0, source);

		//Set video To Play then prepare Audio to prevent Buffering
		vdp.clip = videoToPlay;
		vdp.Prepare();

		//Wait until video is prepared
		WaitForSeconds waitTime = new WaitForSeconds(5);
		while (!vdp.isPrepared)
		{
			Debug.Log("Preparing Video");
			yield return waitTime;
			break;
		}

		Debug.Log("Done Preparing Video");

		//Assign the Texture from Video to RawImage to be displayed
		//image.texture = videoPlayer.texture;

		//Play Video
		vdp.Play();

		//Play Sound
		source.Play();

		Debug.Log("Playing Video");
		while (vdp.isPlaying)
		{
			Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)vdp.time));
			videoDone = true;
			yield return null;
		}

		Debug.Log("Done Playing Video");
		if (videoDone) {
			SceneManager.LoadScene(sceneIdx+1);
		}
	}
}
