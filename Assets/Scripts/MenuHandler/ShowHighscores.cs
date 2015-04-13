using UnityEngine;
using System.Collections.Generic;

public class ShowHighscores : MonoBehaviour {

	public Camera _cam;
    public ScoreboardHandler scoreHandler;

    public GameObject first, second, third;

	public void LoadHighscores()
	{
        //include logic for loading and displaying highscores
        List<ScoreEntry> scores = scoreHandler.GetAllEntries();

        if(scoreHandler.bestScore.score > 0)
            first.GetComponent<UnityEngine.UI.Text>().text = "1) " + scoreHandler.bestScore.score.ToString();

        ScoreEntry secondPlace, thirdPlace;
        secondPlace.name = "";
        secondPlace.score = 0;
        secondPlace.time = 0;
        thirdPlace = secondPlace;

        if (scores.Count >= 2)
        {
            foreach (ScoreEntry score in scores)
            {
                if(score.score > secondPlace.score)
                {
                    secondPlace = score;
                }
                else if(score.score > thirdPlace.score)
                {
                    thirdPlace = score;
                }
            }

            if (secondPlace.score > 0)
            {
                second.GetComponent<UnityEngine.UI.Text>().text = "2) " + secondPlace.score.ToString();
            }
            if (thirdPlace.score > 0)
            {
                third.GetComponent<UnityEngine.UI.Text>().text = "3) " + thirdPlace.score.ToString();
            }
        }
        else if(scores.Count == 1)
        {
            if (scores[0].score > 0)
            {
                secondPlace = scores[0];
                second.GetComponent<UnityEngine.UI.Text>().text = "2) " + secondPlace.score.ToString();
            }
        }
	}

	void Start()
	{
		Screen.SetResolution (480, 800, false);
		_cam.GetComponent<Camera>().aspect = 3.0f / 5.0f;
	}
}
