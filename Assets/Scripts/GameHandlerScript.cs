using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameHandlerScript : MonoBehaviour {

	public int score, multiplier, lastIncrease=0;
	public Text sDisp, mDisp;

	public ScoreboardHandler scoreboard;

	public CanvasGroup menuPanel;

	public GameObject gameOverScore;
	public GameObject gameOverScreen;
	public CanvasGroup gameOverPanel;

    public Grid blockGrid;
    public Spawner spawner;
    BlockadeSetup _blockade;
	int _level;
	string specialColour = "Yellow";
	//bool specialReady = true;

	public Camera _cam;

	bool hasLoaded = false;

    public bool rowsMoving = false;
	public bool columnsMoving = false;

	public CanvasGroup SpecialButton;

    // Use this for initialization
    void Start () 
	{
		Screen.SetResolution (480, 800, false);
		_cam.GetComponent<Camera>().aspect = 3.0f / 5.0f;

		//gameOverScore.guiText.enabled = false;

        GameObject gridObject = GameObject.FindWithTag("Grid");
        blockGrid = gridObject.GetComponent<Grid>();

        GameObject blockadeObject = GameObject.FindWithTag("GameHandler");
        _blockade = blockadeObject.GetComponent<BlockadeSetup>();

        UpdateScoreDisplay ();
		UpdateMultiplierDisplay ();
		_level = 0;
		NextLevel ();
	}

	void Update()
	{
		if(spawner.enabled)
		{
			if(!spawner.shooting)
			{
				if(rowsMoving)
				{
					spawner.shooting = true;
				}/*
				else if(Input.GetMouseButtonDown(1) && specialReady)
				{
					spawner.ShootSpecial(specialColour);
					//specialReady = false;
				}
				else if (Input.GetMouseButtonDown(0))
				{
					//spawner.Shoot();
				}*/
			}
			if(Input.GetKeyDown (KeyCode.Menu))
			{
				Time.timeScale = (Time.timeScale==0.0f ? 1.0f: 0.0f);
				if(Time.timeScale == 0.0f)
				{
					spawner.shooting = true;
					menuPanel.alpha = 1;
					menuPanel.interactable = true;
					menuPanel.blocksRaycasts = true;
				}
				else
				{
					spawner.shooting = false;
					menuPanel.alpha = 0;
					menuPanel.interactable = false;
					menuPanel.blocksRaycasts = false;
				}
			}
			if(Input.GetKey(KeyCode.D))
			{
				blockGrid.RemoveAllBlocks();
			}
			if (Input.GetKey(KeyCode.R))
			{
				blockGrid.RemoveAllBlocks();
				_blockade.GenerateLevel();
			}
			if(Input.GetKey(KeyCode.Escape))
			{
				//ExitConfirm();
				Application.LoadLevel(0);
			}
		}

		if(score >= lastIncrease+100 && spawner.speed <35)
		{
			NextLevel();
			lastIncrease += 100;
		}

		if(!hasLoaded)
		{
			var blocks = Object.FindObjectsOfType<Block> ();
			foreach (Block blocky in blocks)
			{
				blockGrid.AddBlockToGrid(blocky);
			}
			spawner.shooting = false;
			hasLoaded = true;
		}

		if(!blockGrid.CheckColumns ())
		{
			rowsMoving = true;
			spawner.shooting = true;
		}

        if(rowsMoving)
		{
			if(blockGrid.MoveBlocksDown())
			{
                if (blockGrid.CheckColumns())
                {
					blockGrid.SnapBlocksToGrid();
					rowsMoving = false;
					spawner.shooting = false;
					if(blockGrid.CheckForGameOver())
					{
						GameOver ();
					}
					else if(blockGrid.CheckRowsForWin())
					{
						UpdateScore();
						blockGrid.RemoveFullRow();
						if(blockGrid.specialColour != "")
						{
							specialColour = blockGrid.specialColour;
							//specialReady = true;
							specialreadyshits(true);
						}
						rowsMoving = true;
					}
                }
			}
        }
	}

	void NextLevel()
	{
		_level++;
		if(spawner.speed <35f)	spawner.speed += 2f;
		GetComponent<DisplayMessage> ().SetMessage ("Level: " + _level.ToString (), 20);
	}

    void UpdateMultiplier()
    {
        multiplier = blockGrid.CalculateMultiplier();
        UpdateMultiplierDisplay();
    }

	void specialreadyshits(bool huh)
	{
		SpecialButton.interactable = huh;
		SpecialButton.blocksRaycasts = huh;
		SpecialButton.alpha = (huh ? 1 : 0);

	}

    public void BlockLanded()
    {
        if (blockGrid.CheckForGameOver() && !rowsMoving)
        {
            GameOver();
        }
        else
        {
            if(blockGrid.CheckRowsForWin())
            {
                UpdateScore();
                blockGrid.RemoveFullRow();
				rowsMoving = true;
				if(blockGrid.specialColour != "")
				{
					specialColour = blockGrid.specialColour;
					//specialReady = true;
					specialreadyshits(true);
				}
			}
			else
			{
                spawner.shooting = false;
            }
        }

        UpdateMultiplier();
    }

	public void MainMenu()
	{
		Application.LoadLevel (0);
	}

	public void NewGame()
	{
		Application.LoadLevel (1);
	}

    void GameOver()
    {
		spawner.shooting = true;
        spawner.enabled = false;
		
		this.GetComponent<AudioSource>().enabled = false;

		Instantiate(gameOverScreen, new Vector3(0, 0, -2), Quaternion.identity);
		gameOverPanel.alpha = 1;
		gameOverPanel.interactable = true;
		gameOverPanel.blocksRaycasts = true;
		//gameOverScore.guiText.enabled = true;
		string scoreMessage = "You Scored:\n" + score.ToString ();
		gameOverScore.GetComponent<Text> ().text = scoreMessage;
		if (score > scoreboard.bestScore.score)
		{
			scoreboard.AddNewBestScore("", score, 0);
		}
    }

    void UpdateScoreDisplay()
    {
        sDisp.text = score.ToString();
		if (score == 1337)
		{
			sDisp.text = "Nice Meme!";
		}
    }

    void UpdateMultiplierDisplay()
    {
        mDisp.text = "X" + multiplier.ToString();
    }

  
    public void UpdateScore()
    {
        UpdateMultiplier();
        score += 7 * multiplier;
        multiplier = 1;

        UpdateMultiplier();
        UpdateScoreDisplay();
    }

    /*Block[] SortBlocks(Block[] row)
	{
		Block temp;
		for(int i = 1; i < row.Length; i++)
		{
			if(row[i].block.position.x < row[i-1].block.position.x)
			{
				temp = row[i];
				row[i] = row[i-1];
				row[i-1] = temp;
				i-= 2;
			}

			if(i < 1)
			{
				i = 1;
			}
		}

		return row;
	}*/

	public void ShootSpecial()
	{
		if(!rowsMoving && !spawner.shooting)
		{
			spawner.ShootSpecial(specialColour);
			//specialReady = false;
			specialreadyshits(false);
		}
	}
	
	public void Shoot()
	{
		if(!rowsMoving && !spawner.shooting)
		{
			spawner.Shoot();
		}
	}
}
