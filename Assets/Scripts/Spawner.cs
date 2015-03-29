using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	//Groups
	public GameObject[] block;
	public float speed = 21f;
	public bool shooting = true;
	private bool rotateLeft = false;
	private float maxAngle = 65f;
	private int colourState = -1;
	private int randNum = 0;
	private Animator anim;
	private Animator holder_anim;
	private int pause = 9;
	public GameObject bg;
	public GameObject holder;

	//Group Spawning Function
	void spawnNext()
	{
		//Spawn Group at current Position
		Instantiate (block[randNum], transform.position+new Vector3 (0,-0.2f,0), Quaternion.identity);
		shooting = true;
	}
//blue green lightblue red yellow
	void spawnSpecial(string colour)
	{
		GameObject specialBlock;
		//Blue is 1
		//Yellow is 2
		//Empty is 0
		switch(colour)
		{
		case "Blue":
			specialBlock = block[5];
			holder_anim.SetInteger("block_colour", 1);
			break;
		case "Green":
			specialBlock = block[6];
			holder_anim.SetInteger("block_colour", 0);
			break;
		case "LightBlue":
			specialBlock = block[7];
			holder_anim.SetInteger("block_colour", 0);
			break;
		case "Red":
			specialBlock = block[8];
			holder_anim.SetInteger("block_colour", 0);
			break;
		case "Yellow":
			specialBlock = block[9];
			holder_anim.SetInteger("block_colour", 2);
			break;
		default:
			return;
		}
		if(specialBlock != null)
		{
			Instantiate (specialBlock, transform.position + new Vector3 (0, -0.2f, 0), Quaternion.identity);
		}
		shooting = true;
	}

	void Start()
    {
		anim = bg.GetComponent<Animator> ();
		holder_anim = holder.GetComponent<Animator> ();
		randNum = RandomSelect();
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

		if (pause > 10)
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

    public void Shoot()
    {
        if (!shooting)
        {
			Debug.Log("she did WHAT?!");
            spawnNext();
            randNum = RandomSelect();
            colourState = randNum;
            anim.SetInteger("bg_colour", colourState);
			pause = 0;
        }
		Debug.Log ("Cuntsicles");
    }

	public void ShootSpecial(string colour)
	{
		if (!shooting)
		{
			spawnSpecial(colour);
			pause = 0;
		}
	}

	int RandomSelect()
	{
		return Random.Range (0, 5);
	}
}
