using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {

	public ScoreboardHandler scoreboard;

	public GameObject gameOverScore;
	public GameObject gameOverScreen;
	public CanvasGroup gameOverPanel;

	public void GameOver()
	{
		//Pause the game
		GetComponent<PauseUnpause> ().Pause ();		
		GetComponent<AudioSource>().enabled = false;

		//Display Game Over Screen
		Instantiate(gameOverScreen, new Vector3(0, 0, -1), Quaternion.identity);
		GetComponent<MenuPanelHandler> ().ActivatePanel (gameOverPanel);
		string scoreMessage = "You Scored:\n" + GetComponent<HandleScore>().score.ToString ();
		gameOverScore.GetComponent<Text> ().text = scoreMessage;

		//Update Scoreboard (?)
		if (GetComponent<HandleScore>().score > scoreboard.bestScore.score)
		{
			scoreboard.AddNewBestScore("", GetComponent<HandleScore>().score, 0);
		}
	}
}