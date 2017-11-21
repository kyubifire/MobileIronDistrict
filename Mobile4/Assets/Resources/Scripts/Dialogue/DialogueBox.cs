using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class DialogueBox : MonoBehaviour {
	DialogueParser parser;

	GameObject L_sprite;
	GameObject R_sprite;
	GameObject C_sprite;

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

	public bool playerChoosing;
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
	public Button continueBtn;

	//public GUIStyle customStyle, customStyleName;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		continueBtn.gameObject.SetActive (false);

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
		if ((next || Input.GetKeyDown(KeyCode.Space)) && playerChoosing == false) {
			DialogueDisplay();
		}

		if (dialogue != "") {
			SetText ();
		} else {
			continueBtn.gameObject.SetActive (true);
			continueBtn.onClick.AddListener (NextScene);
		}
	}

	void NextScene() {
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void DialogueDisplay() {
		source.PlayOneShot(click);
		next = false;
		if (parser.GetName(lineNum) == "Player" && !isTyping) {
			playerChoosing = true;
			name = "";
			dialogue = "";
			// what does pose equal?
			pose = null;
			position = "";
			lineNum++;
		}
		else if (!isTyping && !playerChoosing) {
			ResetImages();
			name = parser.GetName(lineNum);
			Debug.Log("new name: " + name);
			dialogue = parser.GetContent(lineNum);
			pose = parser.GetPose(lineNum);
			position = parser.GetPosition(lineNum);
			DisplayImages();
			lineNum++;
			StartCoroutine(TextScroll(dialogue));
		} else if (isTyping && !cancelTyping && !playerChoosing) {
			source.PlayOneShot(click);
			cancelTyping = true;
		}

		if (mode == 2) {
			lineNum++;
		}
	}

	void ResetImages() {
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
			spriteObj.transform.position = new Vector3 (-6,Screen.height/768f,0); // set according to width, don't hard code values
			L_sprite = spriteObj;
			L_render = L_sprite.GetComponent<SpriteRenderer>();
			//Debug.Log("LEFT SPRITE: " + L_sprite);

			leftTalking = true;
			//Debug.Log("Is Left character talking? " + leftTalking);
		} else {
			leftTalking = false;
		}

		if (position == "R") {
			spriteObj.transform.position = new Vector3 (6, Screen.height/768f,  0);
			R_sprite = spriteObj;
			R_render = R_sprite.GetComponent<SpriteRenderer> ();
			//Debug.Log ("Right SPRITE: " + R_sprite);

			rightTalking = true;
			//Debug.Log ("Is Right character talking? " + rightTalking);
		} else {
			rightTalking = false;
		}

		if (position == "C") {
			spriteObj.transform.position = new Vector3 (2, Screen.height/768f, 0);
			C_sprite = spriteObj;
			C_render = R_sprite.GetComponent<SpriteRenderer> ();
			//Debug.Log ("Center SPRITE: " + C_sprite);
			centerTalking = true;
			//Debug.Log ("Is Center character talking? " + centerTalking);
		} else {
			centerTalking = false;
		}

		if (!leftTalking && L_sprite != null) {
			//Debug.Log ("RENDER: " + L_render);
			L_render.color = fadeColor;
		} else {
			if (L_sprite != null) {
				L_render.color = origColor;
			}
		}

		if (!rightTalking && R_sprite != null) {
			//Debug.Log ("RENDER: " + R_render);
			R_render.color = fadeColor;
		} else {
			if (R_sprite != null) {
				R_render.color = origColor;
			}
		}

		if (!centerTalking && C_sprite != null) {
			//Debug.Log ("RENDER: " + C_render);
			C_render.color = fadeColor;
		} else {
			if (C_sprite != null) {
				C_render.color = origColor;
			}
		}
	}

	private IEnumerator TextScroll (string lineOfText) {
		int letter = 0;							// keep track of which letter you are on
		//string displayText = "";
		dialogue = "";
		isTyping = true;			
		cancelTyping = false;					// disable player ability to skip text since it is just now being displayed

		while (isTyping && !cancelTyping && (letter < lineOfText.Length-1)) {
			dialogue += lineOfText[letter];			// looks at index of character and displays that letter
			letter += 1;							// move on to next letter
			yield return new WaitForSeconds(typeSpeed);								
		}
		dialogue = lineOfText;
		isTyping = false;
		cancelTyping = false;
	}

	void SetText() {
		Text nText = nameText.GetComponent<Text> ();
		Text diaText = dText.GetComponent<Text> ();
		nText.text = name;
		diaText.text = dialogue;

	}
}
