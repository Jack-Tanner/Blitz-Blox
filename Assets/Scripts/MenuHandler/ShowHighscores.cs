using UnityEngine;
using System.Collections;

public class ShowHighscores : MonoBehaviour {

	public Camera _cam;

	public void LoadHighscores()
	{
		//include logic for loading and displaying highscores
	}

	void Start()
	{
		Screen.SetResolution (480, 800, false);
		_cam.GetComponent<Camera>().aspect = 3.0f / 5.0f;
	}
}
