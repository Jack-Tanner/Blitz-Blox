using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayMessage : MonoBehaviour {

	public Text messageDisplay;
	float messageDisplayLength;

	void Awake()
	{
		EmptyMessage();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//if there is a timer then tick it down once per tick
		if(messageDisplayLength > 0)
		{
			messageDisplayLength--;
		}

		//if the timer reaches zero then nullify the message and set timer to -1
		if(messageDisplayLength == 0)
		{
			EmptyMessage();
			messageDisplayLength--;
		}
	}

	public void EmptyMessage()
	{
		messageDisplay.text = "";
	}

	public void SetMessage(string newMessageText, float displayTime)
	{
		messageDisplay.text = newMessageText;
		messageDisplayLength = displayTime;
	}
}
