using System.IO;
using UnityEngine;
//using UnityEditor;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class DialogueParser : MonoBehaviour {
	int sceneIdx = 0;
	string sceneNum = "";

	List <Sprite> images;
	List <DialogueLine> lines = new List<DialogueLine> ();

	struct DialogueLine {
		public string name;
		public string content;
		public int pose;
		public string position;
        public string[] options;

		public DialogueLine(string n, string c, int p, string pos) {
			name = n;
			content = c;
			pose = p;
			position = pos;
            options = new string[0];
		}
	}

	string filePath;

	// Use this for initialization
	void Start () {
		string file = "Dialogue";
		sceneIdx = SceneManager.GetActiveScene().buildIndex;
		//Debug.Log ("Scene Index is " + sceneIdx);
		sceneNum = sceneIdx.ToString ();
		//Debug.Log (sceneNum);
		file += sceneNum;
		//file += ".txt";
		//Debug.Log (file);
		LoadDialogue (file);   // file = "Dialogue1.txt"

		filePath = System.IO.Path.Combine( Application.streamingAssetsPath, file);

		images = new List<Sprite> ();
		LoadImages ();
	}

	// Update is called once per frame
	void Update () {

	}

	public string GetName (int lineNum) {
		if (lineNum < lines.Count) {
			return lines [lineNum].name;
		}
		return "";
	}

	public string GetContent(int lineNum) {
		if (lineNum < lines.Count) {
			return lines [lineNum].content;
		}
		return "";
	}

	public Sprite GetPose(int lineNum) {
		if (lineNum < lines.Count) {
			return images[lines [lineNum].pose];
		}
		return null;
	}

	public string GetPosition (int lineNum) {
		if (lineNum < lines.Count) {
			return lines [lineNum].position;
		}
		return "";
	}

    public string[] GetOptions(int lineNum) {
        if (lineNum < lines.Count) {
            return lines[lineNum].options;
        }
        return new string[0];
    }

	void LoadImages() {
		//Debug.Log ("In Load Images");
		for (int i = 0; i < lines.Count; i++) {
			string imageName = lines [i].name;
			//Debug.Log("Assigning imageName: " + imageName);
			string imageFile = "Art/Dialogue/" + imageName;
			//Debug.Log ("** sprite to be used:" + imageFile);
			Sprite image = (Sprite) Resources.Load(imageFile, typeof(Sprite));
			//Debug.Log ("IMAGE TO BE USED: "+ image);
			if (!images.Contains(image)) {
				//Debug.Log("Added new image to list");
				images.Add (image);
			}
		}
	}
		

	void LoadDialogue (string filename) {
		//string file = Application.streamingAssetsPath + "/Files/" + filename;
		string file = "";

		string filePath = System.IO.Path.Combine( Application.streamingAssetsPath, filename + ".txt");
		//Debug.Log ("FILE PATH: " + filePath);

		WWW reader = new WWW (filePath);
		while (!reader.isDone) {}

		string realPath = Application.persistentDataPath + "/" + filename;
		//Debug.Log ("REAL PATH: " + realPath);
		System.IO.File.WriteAllText(realPath, reader.text);
		file = realPath;

		Debug.Log ("FILE BEING USED: " + file);
		string line;

		StreamReader r = new StreamReader (file);

		using(r){
			do {
				line = r.ReadLine();
				Debug.Log("Line: " + line);
				if (line != null) {
					string[] lineValues = line.Split('|');
					Debug.Log("List: " + lineValues);
					for ( int i = 0; i < lineValues.Length; i++) {
						Debug.Log("** Line Values: " + lineValues[i]);
					}
					Debug.Log(lineValues[0]);
                    Debug.Log(lineValues[1]);
					//Debug.Log(int.Parse(lineValues[2]));
					int poseLineVal;
					int.TryParse(lineValues[2], out poseLineVal);
					//Debug.Log("CHECKING VAL: " + lineValues[2]);
					//DialogueLine newLine = new DialogueLine(lineValues[0], lineValues[1], int.Parse(lineValues[2]), lineValues[3]);
					DialogueLine newLine = new DialogueLine(lineValues[0], lineValues[1], poseLineVal, lineValues[3]);
					lines.Add(newLine);
				}
			}
			while (line != null);
			r.Close();
		}
	}
}
