using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HandleScore : MonoBehaviour {

	public int score, multiplier, lastIncrease=0;
	public Text sDisp, mDisp;

	Grid blockGrid;

	void Awake()
	{
		print ("Awake Score Handler");

		UpdateScoreDisplay ();
		UpdateMultiplierDisplay ();
	}

	void Start () 
	{
		print ("Start Score Handler");

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
