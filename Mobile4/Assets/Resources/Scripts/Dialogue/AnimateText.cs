using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateText : MonoBehaviour {
	public void animateText (string dialogueLine) {
		int i = 0;
		string st = "";
		while (i < dialogueLine.Length) {
			st += dialogueLine[i++];
			new WaitForSeconds(0.5f);
		}

	}
//	int currentPosition = 0;
//	float delay = 0.1f;  //Or whatever you want the delay to be
//	string text = "";
//	string[] additionalLines;
//
//	public GUIStyle customStyle;
//
//	// Use this for initialization
//	void Start() {
////        for (var S : String in additionalLines)
////            Text += "\n" + S;
////        while (true)
////        {
////            if (currentPosition < Text.Length)
////                guiText.text += Text[currentPosition++];
////            yield WaitForSeconds(Delay);
////        }
//
//    }
//
//    // Update is called once per frame
//    void Update() {
//
//    }
//
//	void WriteText(string t)
//    {
//		//GUI.TextField = (new Rect(50, 350, 1300, 210), "", customStyle);
//        currentPosition = 0;
//        text = t;
//    }

}


