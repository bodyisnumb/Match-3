using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHighscore_script : MonoBehaviour {
	public Text HighscoreText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	HighscoreText.text = "HIGHSCORE:\n" + PlayerPrefs.GetInt("High Score", 0);
	}
}
