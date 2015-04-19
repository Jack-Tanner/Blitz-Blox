using UnityEngine;
using System.Collections;

public class LevelHandler : MonoBehaviour {

	int _level;

    public float constant = 2.0f;

	// Use this for initialization
	void Awake () 
	{
		_level = 0;
	}

	void Start()
	{
		SetLevel (1);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//update level in here
		//currently increases the level at scores 11, 21, 31, etc
		if(GetComponent<HandleScore>().score > (_level * 10))
		{
			NextLevel();
		}
	}

	public void NextLevel()
	{
		SetLevel (_level + 1);
	} 

	public void SetLevel(int newLevel)
	{
		_level = newLevel;

        GetComponent<GameHandlerScript>().spawner.speed = GetComponent<GameHandlerScript>().spawner.baseSpeed + (_level - 1) + constant;

        if (GetComponent<GameHandlerScript>().spawner.speed > 70f)
        {
            GetComponent<GameHandlerScript>().spawner.speed = 70f;
        }

		GetComponent<DisplayMessage> ().SetMessage ("Level: " + _level.ToString (), 50);
	}
}
