using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTile : MonoBehaviour {

	public AnimationClip clearAnim;
	private bool cleared;
	public bool Cleared {
		get { return cleared; }
	}

	private TilePiece tile;

	void Awake() {
		tile = GetComponent<TilePiece> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Clear() {
		cleared = true;
		StartCoroutine (ClearTiles ());
	}
	private IEnumerator ClearTiles() {
		Animator anim = GetComponent<Animator> ();
		if (anim) {
			anim.Play (clearAnim.name);

			yield return new WaitForSeconds (clearAnim.length);
		}

	}
}
