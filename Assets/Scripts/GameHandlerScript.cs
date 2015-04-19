using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameHandlerScript : MonoBehaviour {

	public float blockSize = 0.64f;

	public Grid blockGrid;
    public Spawner spawner;

	string specialColour = "";
	//bool specialReady = true;

	bool hasLoaded = false;

    public bool rowsMoving = false;
	public bool columnsMoving = false;

	public CanvasGroup SpecialButton;

	public bool gameRunning;

    public int gameType;

	void Awake()
	{
		gameRunning = true;
        GetComponent<MenuPanelHandler>().DeactivatePanel(SpecialButton);
    }

    void Start()
    {
        GetComponent<PauseUnpause>().UnPause();
        
    }

	void Update()
	{
        if (spawner.enabled)
        {
            if (!spawner.shooting)
            {
                if (rowsMoving)
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
            if (Input.GetKey(KeyCode.Escape))
            {
                //ExitConfirm();
                Application.LoadLevel(0);
            }
        }

        if (!hasLoaded)
        {
            spawner.shooting = false;
            hasLoaded = true;
        }

        if (!blockGrid.CheckColumns())
        {
            rowsMoving = true;
            spawner.shooting = true;
        }

        if (rowsMoving)
        {
            if (blockGrid.MoveBlocksDown())
            {
                if (blockGrid.CheckColumns())
                {
                    blockGrid.SnapBlocksToGrid();
                    if (blockGrid.CheckForGameOver())
                    {
                        GetComponent<GameOverScript>().GameOver();
                        spawner.shooting = false;
                    }
                    else if (blockGrid.CheckRowsForWin())
                    {
                        GetComponent<HandleScore>().UpdateScore();
                        blockGrid.RemoveFullRow(0);
                        if (blockGrid.specialColour != "")
                        {
                            specialColour = blockGrid.specialColour;
							Debug.Log (specialColour + " is the game handler's special");
							spawner.ShowSpecial(specialColour);
                            GetComponent<MenuPanelHandler>().ActivatePanel(SpecialButton);
                        }
                        rowsMoving = true;
                    }

                    rowsMoving = false;
                    spawner.shooting = false;
                }
            }
        }
        
	}	  

    public void BlockLanded()
    {
        if (blockGrid.CheckForGameOver() && !rowsMoving)
        {
            GetComponent<GameOverScript>().GameOver();
            spawner.shooting = false;
        }
        else
        {
            if(blockGrid.CheckRowsForWin())
            {
				GetComponent<HandleScore>().UpdateScore();
                blockGrid.RemoveFullRow(0);
				rowsMoving = true;
				if(blockGrid.specialColour != "")
				{
					specialColour = blockGrid.specialColour;
					Debug.Log (specialColour + " is the game handler's special");
					spawner.ShowSpecial(specialColour);
					GetComponent<MenuPanelHandler>().ActivatePanel(SpecialButton);
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
        if (specialColour != "" && !rowsMoving && !spawner.shooting)
        {
            spawner.ShootSpecial(specialColour);
            GetComponent<MenuPanelHandler>().DeactivatePanel(SpecialButton);
            specialColour = "";
            blockGrid.specialColour = "";
        }
	}
	
	public void Shoot()
    { 
        if (!rowsMoving && !spawner.shooting)
        {
            spawner.Shoot();
        }
	}

}
