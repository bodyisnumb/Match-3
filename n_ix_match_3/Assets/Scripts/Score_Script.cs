using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score_Script : MonoBehaviour {

public Text ScoreText;
public Text HighscoreText;
public int score;
public int highScore;
public int highScoreNew;
	// Use this for initialization
	void Start () {
	highScore = PlayerPrefs.GetInt("High Score", 0);
	}
	
	// Update is called once per frame
	void Update () {
		ScoreText.text = "SCORE:\n" + score;
		HighscoreText.text = "HIGHSCORE:\n" + highScore;
		if (score > highScore){
        	highScore = score;
    	}
	}
	public void IncreaseScore(int amountToIncrease){
		score += amountToIncrease;
}


	public void ExitButton(int scene){
		SceneManager.LoadScene(scene);
		int originalHighScore = PlayerPrefs.GetInt("High Score", 0);
		if (highScore > originalHighScore)
		{
    	PlayerPrefs.SetInt("High Score", highScore);
		}
	}
}
