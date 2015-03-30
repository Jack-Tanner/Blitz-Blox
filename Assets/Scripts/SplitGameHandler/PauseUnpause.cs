using UnityEngine;
using System.Collections;

public class PauseUnpause : MonoBehaviour {

	public void Pause()
	{
		Time.timeScale = 0.0f;

		GetComponent<GameHandlerScript>().spawner.shooting = true;
	}

	public void UnPause()
	{
		Time.timeScale = 1.0f;
		
		GetComponent<GameHandlerScript>().spawner.shooting = false;
	}
}
