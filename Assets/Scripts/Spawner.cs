using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	//Block Groups
	public GameObject[] normalBlocks;
	public GameObject[] specialBlocks;

	//The minimum speed
	public float baseSpeed = 25.0f;
	//The current speed
	public float speed;

	//is a block currently on the move
	public bool shooting = true;

	//direction of rotation
	private bool rotateLeft = false;
	//max rotation angle
	private float maxAngle = 65f;

	//number representing next block colour to be fired
	private int randNum = 0;

	//pause timer
	private int pause;
	//length of pause timer
	public int pauseTime = 5;


	public GameObject bg;
	public GameObject holder;

	void Awake()
	{
		speed = baseSpeed;
		randNum = Random.Range (0, normalBlocks.Length);
		pause = pauseTime;
	}
    
	void FixedUpdate()
	{
		if (transform.eulerAngles.z >= maxAngle && transform.eulerAngles.z < 100) 
		{
			rotateLeft = true;
		}
        else if (transform.eulerAngles.z <= 360-maxAngle && transform.eulerAngles.z > 100) 
		{
			rotateLeft = false;
		}

		if (pause > pauseTime)
		{
			if (!rotateLeft) 
			{
				transform.Rotate (0, 0, speed/10f);
			} 
			else 
			{
				transform.Rotate (0,0,-speed/10f);
			}
		}
		else
		{
			pause++;
		}

	}

	void SpawnBlock(GameObject blockToSpawn)
	{
		GameObject temp = (GameObject)Instantiate (blockToSpawn, transform.position + new Vector3 (0, -0.2f, 0), Quaternion.identity);
		temp.GetComponent<Block>().SetFiringVectorByAngle(transform.eulerAngles.z);
		temp.GetComponent<Block>().SetSpeed(speed/120.0f);
		shooting = true;
		pause = 0;
	}

    public void Shoot()
    {
        if (!shooting)
        {
			SpawnBlock(normalBlocks[randNum]);

			randNum = Random.Range (0, normalBlocks.Length);
			bg.GetComponent<Animator>().SetInteger("bg_colour", randNum);
        }
    }

	public void ShootSpecial(string colour)
	{
		if (!shooting)
		{
			//fire the appropriate colour and reset the special block holder to empty
			switch(colour)
			{
			case "Blue":
				SpawnBlock(specialBlocks[0]);
				holder.GetComponent<Animator>().SetInteger("block_colour", 0);
				break;
			case "Green":
				SpawnBlock(specialBlocks[1]);
				holder.GetComponent<Animator>().SetInteger("block_colour", 0);
				break;
			case "Cyan":
				SpawnBlock(specialBlocks[2]);
				holder.GetComponent<Animator>().SetInteger("block_colour", 0);
				break;
			case "Red":
				SpawnBlock(specialBlocks[3]);
				holder.GetComponent<Animator>().SetInteger("block_colour", 0);
				break;
			case "Yellow":
				SpawnBlock(specialBlocks[4]);
				holder.GetComponent<Animator>().SetInteger("block_colour", 0);
				break;
			default:
				return;
			}
		}
	}

	public void ShowSpecial(string colour)
	{
		//Show the current special block
		switch(colour)
		{
		case "Blue":
			holder.GetComponent<Animator>().SetInteger("block_colour", 0);
			break;
		case "Green":
			holder.GetComponent<Animator>().SetInteger("block_colour", 1);
			break;
		case "Cyan":
			holder.GetComponent<Animator>().SetInteger("block_colour", 2);
			break;
		case "Red":
			holder.GetComponent<Animator>().SetInteger("block_colour", 3);
			break;
		case "Yellow":
			holder.GetComponent<Animator>().SetInteger("block_colour", 4);
			break;
		default:
            return;
		}
	}
}
