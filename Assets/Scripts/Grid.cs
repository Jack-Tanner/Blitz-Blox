using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public Block[,] blockGrid;
    public int gridWidth, gridHeight;
	public string specialColour;

	public GameObject emitter;
	public GameHandlerScript gameHandler;

    float moveAmount = 0.0f;
	
    //Determines how much a block needs to move to get to the next row.
    float moveBy = (0.64f / 8.0f);

    // Use this for initialization
    void Awake () 
	{
        //Sets the size of the block grid and sets all to NULL.
        blockGrid = new Block[gridWidth, gridHeight];

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                blockGrid[i, j] = null;
            }
        }
    }

    //Adds a single block to the grid.
	public bool AddBlockToGrid(Block block) 
	{
        //Snap block in place.
		SnapBlockToGrid (block);
        //Find it's grid position and add it to the grid.
		Vector2 blockLoc = FindGridPosition (block);
        if(blockGrid[(int)blockLoc.x, (int)blockLoc.y] == null)
        {
            blockGrid[(int)blockLoc.x, (int)blockLoc.y] = block;
            return true;
        }
        return false;
	}

    //Checks through all the rows to find if a row is full.
    public bool CheckRowsForWin()
    {
        //Simply counts up the amount of blocks in a row.
        //If the count equals the gridWidth then we have a win!
        for(int y = 0; y < gridHeight; y++)
        {
            int count = 0;
            for(int x = 0; x < gridWidth; x++)
            {
                if(blockGrid[x, y] != null)
                {
                    count++;
                }
            }

            if(count == gridWidth)
            {
                return true;
            }
        }
        return false;
    }

	//Removes a single block at a location.
	public void RemoveBlock(int x, int y)
	{
		//Make sure the its within bounds...
		if (x < 0 || y < 0)
			return;
		else if (x > gridWidth || y > gridHeight)
			return;
		//Try and remove the block and emit particles.
		try
		{
			if (blockGrid[x, y] != null)
			{
				blockGrid[x, y].EmitParticles(75, blockGrid[x, y].transform.position);
				DestroyObject(blockGrid[x, y].gameObject);
				blockGrid[x, y] = null;
			}
		}
		catch
		{
			Debug.Log ("Error upon 'RemoveBlock("+ x +","+ y + ")'");
			return;
		}
	}

    //Simply removes a given row
    public void RemoveFullRow(int rowNum)
    {
		GetComponent<AudioSource>().Play ();

		int consecColour=0;
		string prevColour = blockGrid [0, rowNum].colour;
		for (int x = 0; x < gridWidth; x++)
		{
			if(blockGrid[x,rowNum].colour == prevColour)
			{
				consecColour ++;
				if(consecColour == 2)
				{
					specialColour = prevColour;
				}
			}
			else
			{
				consecColour = 0;
				prevColour = blockGrid[x,rowNum].colour;
			}
			RemoveBlock (x, rowNum);
		}
		MoveRowsDown(rowNum);
    }
    
    //Removes ALL blocks
	public void RemoveAllBlocks()
	{
		for(int x = 0; x < gridWidth; x++)
		{
			for(int y = 0; y < gridHeight; y++)
			{
				if(blockGrid[x, y] != null)
				{
					DestroyObject(blockGrid[x, y].gameObject);
					blockGrid[x, y] = null;
				}
			}
		}
	}   
    
    //Checks through each column to find if it has a hole in.
    public bool CheckColumns()
    {
        for(int x = 0; x < gridWidth; x++)
        {
            bool hasHole = blockGrid[x, 0] == null ? true : false;
            for (int y = 1; y < gridHeight; y++)
            {
                if (blockGrid[x, y] != null && hasHole)
                {
                    return false;
                }
                else if(blockGrid[x, y] == null)
                {
                    hasHole = true;
                }
            }
        }
        return true;
    }

    //Moves all the blocks down.
    public bool MoveBlocksDown()
    {
		Debug.Log ("Is this code even called?");
        //Move the blocks down.
        if(moveAmount < 0.64f)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                for (int x = 0; x < gridWidth; x++)
                {
                    Block block = (Block)blockGrid[x, y];
                    if (block != null)
                    {
						if (!block.CheckBelow())
						{
                        	block.Move(new Vector3(0, -1.0f * moveBy, 0));
						}
                    }
                }
            }

            moveAmount += moveBy;            

            return false;
        }

        //Set the blocks in their new positions.
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
				try
				{
	                Block block = blockGrid[x, y];
	                if (block != null)
	                {
						blockGrid[x,y] = null;
						AddBlockToGrid(block);
	                }
				}
				catch
				{
					continue;
				}
            }
        }

		moveAmount = 0.0f;

        return true;
    }

    //Snaps all the blocks into grid positions.
    public void SnapBlocksToGrid()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (blockGrid[x, y] != null)
                {
					Vector3 tempPos = blockGrid[x, y].block.position;
					
					tempPos.x = SnapToGridX (tempPos.x, gameHandler.blockSize);
					tempPos.y = SnapToGridY (tempPos.y, gameHandler.blockSize);
					
					blockGrid[x, y].block.position = tempPos;
                }
            }
        }
    }

    //Moves all the block GRID positions down one.
    void MoveRowsDown(int startingRow)
    {
		for (int y = startingRow; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
				if (blockGrid[x, y] != null)
                {
					Block block = blockGrid[x, y];
                    blockGrid[x, y - 1] = block;
                    blockGrid[x, y] = null;
                }
            }
        }
    }

    //Gets a block from a grid position.
	public Block GetBlock(int x, int y)
	{
		return blockGrid [x, y];
	}

    //Checks if a column is full for a gameover.
    public bool CheckForGameOver()
    {
        for (int y = 10; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (blockGrid[x, y] != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public int CalculateMultiplier()
    {
        int multiplier = 1;
        int tempBlockMultiplier = 1;
        for (int i = 1; i < gridWidth; i++)
        {
            if (blockGrid[i, 0] != null && blockGrid[i - 1, 0] != null && blockGrid[i, 0].colour == blockGrid[i - 1, 0].colour)
            { 
				if(blockGrid[i, 0].colour != "Grey")
				{
                	tempBlockMultiplier++;
				}
            }
            else
            {
                multiplier *= tempBlockMultiplier;
                tempBlockMultiplier = 1;
            }
        }

        return multiplier *= tempBlockMultiplier;
    }

	public Vector2 FindGridPosition(Block theBlock)
    {
		float x = theBlock.block.position.x / gameHandler.blockSize;
		float y = theBlock.block.position.y / gameHandler.blockSize;

		int locX = (int) (x + 3f) , locY = (int) (y + 5.5f);
		return new Vector2 (locX, locY);
	}

	Block GridAtPoint(Vector3 worldPos)
	{
		float x = worldPos.x / gameHandler.blockSize;
		float y = worldPos.y / gameHandler.blockSize;
		
		int locX = (int) (x + 3f) , locY = (int) (y + 5.5f);

		return blockGrid[locX, locY];
	}

	public void SnapBlockToGrid(Block theBlock)
    {
		Vector3 tempPos = theBlock.block.position;

		tempPos.x = SnapToGridX (tempPos.x, gameHandler.blockSize);
		tempPos.y = SnapToGridY (tempPos.y, gameHandler.blockSize);

		Block tempBlock = GridAtPoint (tempPos);

		if(tempBlock != null)
		{
			Debug.Log ("CONFLICT ALERT:" + theBlock.colour + "into" + tempBlock.colour);
		}

		theBlock.block.position = tempPos;
	}	
	
	float SnapToGridX(float currentX, float blockSize)
	{
		float signX = currentX / Mathf.Abs (currentX);
		
		currentX += blockSize * 0.5f * signX;
		currentX -= currentX % blockSize;

		if (float.IsNaN (currentX))
			currentX = 0;
		
		return currentX;
	}
	
	float SnapToGridY(float currentY, float blockSize)
	{		
		float signY = currentY / Mathf.Abs (currentY);
		
		currentY -= currentY % blockSize;
		currentY += blockSize * 0.5f * signY;
		
		if (float.IsNaN (currentY))
			currentY = 0;
		
		return currentY;
	}
}
