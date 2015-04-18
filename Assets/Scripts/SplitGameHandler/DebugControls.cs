using UnityEngine;
using System.Collections;

public class DebugControls : MonoBehaviour {

	bool rLast = false;	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.D))
		{
			GetComponent<GameHandlerScript>().blockGrid.RemoveAllBlocks();
		}
		if (Input.GetKey(KeyCode.R) && !rLast)
		{
			rLast = true;
			GetComponent<GameHandlerScript>().blockGrid.RemoveAllBlocks();
			GetComponent<BlockadeSetup>().GenerateLevel();
		}
		else if(!Input.GetKey(KeyCode.R))
		{
			rLast = false;
		}
		if (Input.GetKey(KeyCode.T))
		{
			GetComponent<GameHandlerScript>().spawner.shooting = false;
		}
	}
}
