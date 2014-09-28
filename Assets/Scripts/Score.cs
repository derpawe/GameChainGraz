using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {
	Text text;

	void Awake () {
		text = GetComponent<Text>();
		UpdateScore();
		InvokeRepeating("UpdateScore", 1, 1);

		if (Application.loadedLevelName == "End") {
			Debug.Log ("Score: " + GlobalManager.time.ToString());
			ulong loghash = ((ulong)GlobalManager.time) + 856969;
			loghash *= 119563;
			loghash %= 820901;
			Debug.Log("ScoreHash: " + loghash.ToString ());
		}
	}

	void UpdateScore() {
		if (Application.loadedLevelName == "Startscreen") {
			text.text = "0";
		} else if (Application.loadedLevelName == "End") {
			text.text = GlobalManager.time.ToString();
		} else {
			GlobalManager.time++;
			text.text = GlobalManager.time.ToString();
		}
	}
}
