using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameHandlerScript : MonoBehaviour {


	public CanvasGroup menuPanel;

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

    void Awake()
	{
		print ("Awake Game Handler");
		Screen.SetResolution (480, 800, false);
	}

    void Start () 
	{
		print ("Start Game Handler");
		_cam.GetComponent<Camera>().aspect = 3.0f / 5.0f;

        _blockade = GetComponent<BlockadeSetup>();
        
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
				}*/
				else if (Input.GetMouseButtonDown(0))
				{
					//spawner.Shoot();
				}
			}
			if(Input.GetKeyDown (KeyCode.Menu))
			{
				if(Time.timeScale == 0.0f)
				{
					GetComponent<PauseUnpause>().UnPause();
					GetComponent<MenuPanelHandler>().DeactivatePanel(menuPanel);
				}
				else
				{
					GetComponent<PauseUnpause>().Pause();
					GetComponent<MenuPanelHandler>().ActivatePanel(menuPanel);
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
						GetComponent<GameOverScript>().GameOver ();
					}
					else if(blockGrid.CheckRowsForWin())
					{
						GetComponent<HandleScore>().UpdateScore();
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
            GetComponent<GameOverScript>().GameOver();
        }
        else
        {
            if(blockGrid.CheckRowsForWin())
            {
				GetComponent<HandleScore>().UpdateScore();
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

		GetComponent<HandleScore>().UpdateMultiplier();
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
