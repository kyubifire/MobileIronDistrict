using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Video : MonoBehaviour {
	public VideoClip videoToPlay;
	private VideoPlayer video;
	private AudioSource source;
	private bool videoDone = false;
	private int sceneIdx;

	// Use this for initialization
	void Start() {
		sceneIdx = SceneManager.GetActiveScene ().buildIndex;
		Application.runInBackground = true;
		StartCoroutine(playVideo());
	}

	IEnumerator playVideo() {
		video = gameObject.AddComponent<VideoPlayer>();
		video.aspectRatio = VideoAspectRatio.FitOutside;
		source = gameObject.AddComponent<AudioSource>();
		video.playOnAwake = true;
		video.source = VideoSource.VideoClip;
		video.audioOutputMode = VideoAudioOutputMode.AudioSource;
		video.EnableAudioTrack(0, true);
		video.SetTargetAudioSource (0, source);
		video.clip = videoToPlay;
		video.Prepare();

		//Wait until video is prepared
		WaitForSeconds waitTime = new WaitForSeconds(5);
		while (!video.isPrepared) {
			Debug.Log("Preparing Video");
			yield return waitTime;
			break;
		}

		Debug.Log("Done Preparing Video");
		video.Play();
		source.Play();

		Debug.Log("Playing Video");
		while (video.isPlaying) {
			Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)video.time));
			videoDone = true;
			yield return null;
		}

		Debug.Log("Done Playing Video");
		if (videoDone) {
			SceneManager.LoadScene(sceneIdx+1);
		}
	}
}
