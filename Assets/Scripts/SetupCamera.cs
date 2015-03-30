using UnityEngine;
using System.Collections;

public class SetupCamera : MonoBehaviour {

	//Used to setup the camera, because that aint got nuthin to do with the gameplay loop
	void Awake()
	{
		Screen.SetResolution (480, 800, false);
	}

	void Start () 
	{
		GetComponent<Camera>().aspect = 3.0f / 5.0f;
	}
}
