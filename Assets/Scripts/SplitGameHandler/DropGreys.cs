using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DropGreys : MonoBehaviour {

	public float greyDumpTime;
	float timeToGreyDump;
	public GameObject greyBlock;

	void Awake()
	{
		timeToGreyDump = Time.time + greyDumpTime;
	}
	
	//Dump Warning
	void Update () 
	{
		//If the time to dump approaches then warn the Committee of Toilet Security about the dump increase
		if(Time.time >= timeToGreyDump - 1.0f && Time.time < timeToGreyDump)
		{
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
		for (float x=-1.92f;x<2.0f;x+=0.64f)
		{
			GameObject newBlock = (GameObject)Instantiate(greyBlock, new Vector3(x,2.88f,-0.5f),Quaternion.identity);
			newBlock.GetComponent<Block>().SetFiringVectorByAngle(0.0f);
			newBlock.GetComponent<Block>().SetSpeed(GetComponent<GameHandlerScript>().spawner.speed/120.0f);
		}
		GetComponent<GameHandlerScript>().spawner.shooting = true;
	}
}
