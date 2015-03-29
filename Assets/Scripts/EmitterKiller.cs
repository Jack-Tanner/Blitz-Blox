using UnityEngine;
using System.Collections;

public class EmitterKiller : MonoBehaviour {

	float startTime;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Time.time > startTime+0.5f)
		{
			Destroy(gameObject);
		}
	}

	public void SetColour(string colour)
	{
		Color32 blockColour = new Color32(255, 255, 255, 255);
		switch(colour)
		{
		case "Blue":
			blockColour.r = 73;
			blockColour.g = 94;
			blockColour.b = 255;
			break;
		case "Green":
			blockColour.r = 37;
			blockColour.g = 219;
			blockColour.b = 88;
			break;
		case "Grey":
			blockColour.r = 56;
			blockColour.g = 56;
			blockColour.b = 56;
			break;
		case "Cyan":
			blockColour.r = 73;
			blockColour.g = 206;
			blockColour.b = 255;
			break;
		case "Red":
			blockColour.r = 254;
			blockColour.g = 80;
			blockColour.b = 80;
			break;
		case "Yellow":
			blockColour.r = 253;
			blockColour.g = 255;
			blockColour.b = 73;
			break;
		}
		GetComponent<ParticleSystem>().startColor = blockColour;
	}
}

// Blue, Green, Cyan, Red, Yellow, Grey