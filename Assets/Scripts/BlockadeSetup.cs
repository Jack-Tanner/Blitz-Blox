using UnityEngine;
using System.Collections;

public class BlockadeSetup : MonoBehaviour {
	
	public GameObject block;
	
	const int totalColumns = 7;
	const int totalRows = 10;
	const float blockSize = 0.64f;
	
	Vector3 blockPos;
	const float minX = -1.92f;
	float maxX = 0f;
	const float minY = -3.52f;
	float maxY = 0f;
	const float blockZ = -0.5f;
	int blockRateX = totalColumns - 1;
	int blockRateY = totalRows - 1;
	int randNum = 0;
	
	public Grid grid;
	
	// Use this for initialization
	void Start () 
	{
        GenerateLevel();
	}

    public void GenerateLevel()
    {
        maxX = minX + (blockSize * totalColumns);
        maxY = minY + (blockSize * totalRows);

        //Set up Block field
        //Rows
        for (float ix = minX; ix < maxX; ix += blockSize)
        {
            randNum = Random.Range(0, blockRateX);

            if (randNum != 0)
            {
                blockRateX--;
                //Columns
                for (float iy = minY; iy < maxY; iy += blockSize)
                {
                    blockPos = new Vector3(ix, iy, blockZ);

                    randNum = Random.Range(0, blockRateY);
                    if (randNum != 0)
                    {
						GameObject newBlock = (GameObject)Instantiate(block, blockPos,Quaternion.identity);
						newBlock.GetComponent<Block>().SetFiringVectorByAngle(0.0f);
						newBlock.GetComponent<Block>().SetSpeed(GetComponent<GameHandlerScript>().spawner.speed/120.0f);
						newBlock.GetComponent<Block>().isInAir = false;
                        //b.enabled = false;

						if (!grid.AddBlockToGrid(newBlock.GetComponent<Block>()))
						{
							DestroyObject(newBlock);
						}

                        blockRateY--;    
                    }
                    else
                    {
                        blockRateY = totalRows - 1;
                        break;
                    }
                    //}
                }
            }
            else
            {
                blockRateX = totalColumns - 1;
            }
        }
    }
}
