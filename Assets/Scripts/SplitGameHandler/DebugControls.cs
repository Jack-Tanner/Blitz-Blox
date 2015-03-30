using UnityEngine;
using System.Collections;

public class DebugControls : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.D))
		{
			GetComponent<GameHandlerScript>().blockGrid.RemoveAllBlocks();
		}
		if (Input.GetKey(KeyCode.R))
		{
			GetComponent<GameHandlerScript>().blockGrid.RemoveAllBlocks();
			GetComponent<BlockadeSetup>().GenerateLevel();
		}
		if (Input.GetKey(KeyCode.T))
		{
			//UNLIMITED POWAAAAAAAAHHHHH
			GetComponent<GameHandlerScript>().spawner.shooting = false;
		}
	}
}
