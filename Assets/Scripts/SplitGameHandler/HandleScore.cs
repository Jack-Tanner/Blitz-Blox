using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HandleScore : MonoBehaviour {

	public int score, multiplier, lastIncrease=0;
	public Text sDisp, mDisp;

	Grid blockGrid;

	void Awake()
	{
		UpdateScoreDisplay ();
		UpdateMultiplierDisplay ();
	}

	void Start () 
	{
		GameObject gridObject = GameObject.FindWithTag("Grid");
		blockGrid = gridObject.GetComponent<Grid>();
	}

	public void UpdateScore()
	{
		UpdateMultiplier();
		score += 7 * multiplier;
		multiplier = 1;
		
		UpdateMultiplier();
		UpdateScoreDisplay();
	}

    public void AddToScore(int amount)
    {
        UpdateMultiplier();
        score += amount * multiplier;

        UpdateMultiplier();
        UpdateScoreDisplay();
    }

	void UpdateScoreDisplay()
	{
		sDisp.text = score.ToString();
		if (score == 1337)
		{
			sDisp.text = "Nice Meme!";
		}
	}

	public void UpdateMultiplier()
	{
		multiplier = blockGrid.CalculateMultiplier();
		UpdateMultiplierDisplay();
	}
	
	void UpdateMultiplierDisplay()
	{
		mDisp.text = "X" + multiplier.ToString();
	} 
}
