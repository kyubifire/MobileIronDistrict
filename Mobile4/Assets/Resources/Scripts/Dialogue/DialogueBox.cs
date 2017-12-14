using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class DialogueBox : MonoBehaviour {
	private DialogueParser parser;

	private GameObject L_sprite; // left character sprite
	private GameObject R_sprite; // right character sprite
	private GameObject C_sprite; // center character sprite

	SpriteRenderer L_render;
	SpriteRenderer R_render;
	SpriteRenderer C_render;

	private static Color fadeColor = new Color(.5f, .5f, .5f, 1.0f); 		// set sprite color to this if character is not talking.
	private static Color origColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);	   // reset RGBA values to get original sprite coloring

	public string dialogue;
	public string name;
	public Sprite pose;
	public string position;

	public int lineNum;
	public int mode;

	public bool next;

	public bool nextScene;

	private bool leftTalking;
	private bool rightTalking;
	private bool centerTalking;

	private bool isTyping;
	private bool cancelTyping;
	public float typeSpeed;

	public AudioClip click;
	public AudioSource source;

	public GameObject nameText;
	public GameObject dText;
	private Vector3 origTextPos;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		dialogue = "";
		lineNum = 0;
		mode = 0;
		parser = GameObject.Find ("DialogueParser").GetComponent<DialogueParser>();

		leftTalking = false;
		rightTalking = false;
		centerTalking = false;

		next = false;
		isTyping = false;
		cancelTyping = false;

		origTextPos =  new Vector3 (nameText.transform.position.x, nameText.transform.position.y, nameText.transform.position.x);

		// get character data for first character
		name = parser.GetName (lineNum);
		Debug.Log ("First character Name: " + name);
		pose = parser.GetPose(lineNum);
		position = parser.GetPosition (lineNum);
		dialogue = parser.GetContent (lineNum);
		DisplayImages ();

		if (!isTyping) {
			StartCoroutine (TextScroll (dialogue));
		}
		lineNum++;

		// get character data for second character
		//		name = parser.GetName (lineNum);
		//		Debug.Log ("Second character Name:" + name);
		//		pose = parser.GetPose(lineNum);
		//		position = parser.GetPosition (lineNum);
		//		lineNum = 0;		// reset to start of dialogue
		//		DisplayImages ();
	}

	// Update is called once per frame
	void Update () {
		if ((next)) {
			DialogueDisplay();
		}
		if (dialogue != "") {
			SetText ();
		} else {
			if (Input.anyKey) {
				SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
			}
		}
	}

	public void DialogueDisplay() {
		source.PlayOneShot(click);
		next = false;
		if (parser.GetName(lineNum) == "Player" && !isTyping) {
			name = "";
			dialogue = "";
			// what does pose equal? --> the sprite being used, can use multiple sprites to have different mannerisms or facial expressions
			pose = null;
			position = "";
			lineNum++;
		}
		else if (!isTyping) {
			ResetImages();
			name = parser.GetName(lineNum);
			dialogue = parser.GetContent(lineNum);
			pose = parser.GetPose(lineNum);
			position = parser.GetPosition(lineNum);
			DisplayImages();
			lineNum++;
			StartCoroutine(TextScroll(dialogue));
		} else if (isTyping && !cancelTyping) {
			source.PlayOneShot(click);
			cancelTyping = true;
		}

		if (mode == 2) {
			lineNum++;
		}
	}

	void ResetImages() {
		nameText.transform.position = origTextPos; // reset to center each time
		//Debug.Log ("In Reset images");
		if (name != "") {
			GameObject character = GameObject.Find (name);
			Debug.Log ("Character speaking now: " + character);
			SpriteRenderer currSprite = character.GetComponent<SpriteRenderer> ();
			Debug.Log ("Current sprite: " + currSprite);
		}
	}

	void DisplayImages() {
		//Debug.Log ("In display Images");
		if (name != "") {
			GameObject character = GameObject.Find (name);
			//Debug.Log ("current character is : " + character);
			SetSpritePositions (character);
			SpriteRenderer currSprite = character.GetComponent<SpriteRenderer> ();
			//Debug.Log ("CURRENT SPRITE: " + currSprite);
			currSprite.sprite = pose;
		}
	}

	void SetSpritePositions (GameObject spriteObj) {
		//Debug.Log ("In SETSPRITEPOSITIONS");
		if (position == "L") {
			nameText.transform.position = new Vector3 (nameText.transform.position.x - 45f, nameText.transform.position.y, nameText.transform.position.x);
			spriteObj.transform.position = new Vector3 (-6,Screen.height/384f,0); // set according to hardware width/height, don't hard code values
			L_sprite = spriteObj;
			L_render = L_sprite.GetComponent<SpriteRenderer>();
			//Debug.Log("LEFT SPRITE: " + L_sprite);
			leftTalking = true;
			//Debug.Log("Is Left character talking? " + leftTalking);
		} else if (L_sprite != null) {
			leftTalking = false;
		}

		if (position == "R") {
			nameText.transform.position = new Vector3 (nameText.transform.position.x + 45f, nameText.transform.position.y, nameText.transform.position.x);
			spriteObj.transform.position = new Vector3 (5, Screen.height/384f,  0);
			R_sprite = spriteObj;
			R_render = R_sprite.GetComponent<SpriteRenderer> ();
			//Debug.Log ("Right SPRITE: " + R_sprite);
			rightTalking = true;
			//Debug.Log ("Is Right character talking? " + rightTalking);
		} else if (R_sprite != null) {
			rightTalking = false;
		}

		if (position == "C") {
			spriteObj.transform.position = new Vector3 (0, Screen.height/384f, 0);
			C_sprite = spriteObj;
			C_render = C_sprite.GetComponent<SpriteRenderer> ();
			//Debug.Log ("Center SPRITE: " + C_sprite);
			centerTalking = true;
			//Debug.Log ("Is Center character talking? " + centerTalking);
		} else if (C_sprite != null) {
			centerTalking = false;
		}

		if (L_sprite != null && R_sprite != null && C_sprite == null) {
			if (leftTalking) {
				Debug.Log ("left sprite ok");
				R_render.color = fadeColor;
				L_render.color = origColor;
			} else if (!leftTalking && rightTalking) {
				Debug.Log ("right sprite ok");
				R_render.color = origColor;
				L_render.color = fadeColor;
			}
		}

		if (L_sprite != null && R_sprite != null && C_sprite != null) {
			if (leftTalking) {
				Debug.Log ("left sprite ok 2");
				R_render.color = fadeColor;
				C_render.color = fadeColor;
				L_render.color = origColor;
			} else if (rightTalking) {
				Debug.Log ("right sprite ok 2");
				R_render.color = origColor;
				C_render.color = fadeColor;
				L_render.color = fadeColor;
			} else if (centerTalking) {
				Debug.Log ("center sprite ok 2");
				L_render.color = fadeColor;
				R_render.color = fadeColor;
				C_render.color = origColor;
			}
		}
	}

	void SetText() {
		Text nText = nameText.GetComponent<Text> ();
		Text diaText = dText.GetComponent<Text> ();
		nText.text = name;
		diaText.text = dialogue;
	}

	private IEnumerator TextScroll (string lineOfText) {
		int letter = 0;	// keep track of which letter you are on
		dialogue = "";
		isTyping = true;			
		cancelTyping = false;// disable player ability to skip text since it is just now being displayed

		while (isTyping && !cancelTyping && (letter < lineOfText.Length-1)) {
			dialogue += lineOfText[letter];	// looks at index of character and displays that letter
			letter += 1;					// move on to next letter
			yield return new WaitForSeconds(typeSpeed);								
		}
		dialogue = lineOfText;
		isTyping = false;
		cancelTyping = false;
	}
}
