using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour {

	// Load a given Scene
	public void LoadScene(string scene) 
	{
		Application.LoadLevel (scene);
	}
}
 