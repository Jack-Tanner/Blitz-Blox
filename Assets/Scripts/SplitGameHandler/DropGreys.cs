using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DropGreys : MonoBehaviour {

	public float greyDumpTime;
	float timeToGreyDump;
	public Block greyBlock;

	Spawner spawner;
	Grid blockGrid;

	// Use this for initialization
	void Start () 
	{
		timeToGreyDump = Time.time + greyDumpTime;

		spawner = GetComponent<GameHandlerScript>().spawner;
		blockGrid = GetComponent<GameHandlerScript>().blockGrid;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If the time to dump approaches then warn the Committee of Toilet Security about the dump increase
		if(Time.time >= timeToGreyDump - 1.0f && Time.time < timeToGreyDump)
		{
			print ("should be displaying message now...");
			GetComponent<DisplayMessage>().SetMessage("Greys Incoming", -1);
		}

		//If it's time to dump then dump and reset the timer
		if(Time.time >= timeToGreyDump)
		{
			DumpGreys();
			timeToGreyDump = Time.time + greyDumpTime;
			GetComponent<DisplayMessage>().EmptyMessage();
		}
	}

	void DumpGreys()
	{
		spawner.shooting = true;
		Block thisBlock;
		for (float x=-1.92f;x<2.0f;x+=0.64f)
		{
			thisBlock = (Block)Instantiate(greyBlock, new Vector3(x,2.88f,-0.5f),Quaternion.identity);
			blockGrid.AddBlockToGrid(thisBlock);
		}
	}
}
