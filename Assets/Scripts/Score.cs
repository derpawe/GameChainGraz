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
			storeHighscore(GlobalManager.time);
		}
	}

	void UpdateScore() {
		if (Application.loadedLevelName == "Startscreen") {
			text.text = loadHighscore().ToString();
		} else if (Application.loadedLevelName == "End") {
			text.text = GlobalManager.time.ToString();
		} else {
			GlobalManager.time++;
			text.text = GlobalManager.time.ToString();
		}
	}

	void storeHighscore(int time) {
		if (time < 2) // invalid highscore
			time = 99999;

		Debug.Log("Score: " + time.ToString());
		ulong newHash = ((ulong)GlobalManager.time) + 856969;
		newHash *= 119563;
		newHash %= 820901;
		newHash += 23;
		Debug.Log("ScoreHash: " + newHash.ToString());

		// store new
		if (time < loadHighscore()) {
			Debug.Log("New Highscore!");
			GameObject newScore = GameObject.Find("Canvas/NewHighscore");
			newScore.GetComponent<Text>().enabled = true;

			PlayerPrefs.SetInt(Application.levelCount + "_" + "score", time);
			PlayerPrefs.SetInt(Application.levelCount + "_" + "hash", (int) newHash);
		}
	}

	int loadHighscore() {
		int time = PlayerPrefs.GetInt(Application.levelCount + "_" + "score", 99999);
		int hash = PlayerPrefs.GetInt(Application.levelCount + "_" + "hash", 0);
		
		// validate hash ... has nothing to do with security, but is better than a plain number stored in a file
		ulong oldCorrectHash = ((ulong) time) + 856969;
		oldCorrectHash *= 119563;
		oldCorrectHash %= 820901;
		oldCorrectHash += 23;
		if (hash != (int) oldCorrectHash) {
			time = 99999;
		}

		return time;
	}


}
