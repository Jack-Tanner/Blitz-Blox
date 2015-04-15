using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {

	public ScoreboardHandler scoreboard;

	public GameObject gameOverScore;
	public GameObject gameOverScreen;
    public GameObject highScore;
	public CanvasGroup gameOverPanel;

    public void GameOver()
	{
		//Pause the game
		GetComponent<PauseUnpause> ().Pause ();		
		GetComponent<AudioSource>().enabled = false;
		GetComponent<GameHandlerScript> ().gameRunning = false;


        //Display Game Over Screen
        Instantiate(gameOverScreen, new Vector3(0, 0, 1), Quaternion.identity);
		GetComponent<MenuPanelHandler> ().ActivatePanel (gameOverPanel);
		string scoreMessage = "You Scored:\n" + GetComponent<HandleScore>().score.ToString ();
		gameOverScore.GetComponent<Text> ().text = scoreMessage;

        if (GetComponent<HandleScore>().score > 0)
        {
            //Update Scoreboard (?)
            if (GetComponent<HandleScore>().score > scoreboard.bestScore.score)
            {
                highScore.SetActive(true);
                scoreboard.AddNewBestScore("", GetComponent<HandleScore>().score, 0);
            }
            else
            {
                scoreboard.AddScore("", GetComponent<HandleScore>().score, 0);
            }
        }
	}
}