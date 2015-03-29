using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour {

	int score;
	public GameObject sDispObject;
	Text sDisp;
    public GameObject scoreBoardObject;
	ScoreboardHandler scoreBoard;
	CanvasGroup panel;
	// Use this for initialization
	void Start ()
    {
		

        
    }

    public void GameOver()
    {
		sDisp = sDispObject.GetComponent<Text> ();
		scoreBoard = scoreBoardObject.GetComponent<ScoreboardHandler> ();
        Instantiate(this, new Vector3(0, 0, -2), Quaternion.identity);
		panel = this.GetComponent<CanvasGroup> ();
        panel.alpha = 1;
        panel.interactable = true;
        panel.blocksRaycasts = true;

        GameObject handler = GameObject.FindGameObjectWithTag("GameHandler");
        score = handler.GetComponent<GameHandlerScript>().score;

        string scoreMessage = "You Scored:\n" + score.ToString();

        if (score > scoreBoard.bestScore.score)
        {
            scoreBoard.AddNewBestScore("", score, 0);
        }

        sDisp.text = scoreMessage;
    }

    // Update is called once per frame
    void Update ()
    {
		if (Input.GetKeyDown (KeyCode.UpArrow)) 
		{
			Application.LoadLevel (0);
		}
	}
}