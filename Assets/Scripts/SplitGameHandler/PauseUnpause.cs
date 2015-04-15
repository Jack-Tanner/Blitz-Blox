using UnityEngine;
using System.Collections;

public class PauseUnpause : MonoBehaviour {

	public CanvasGroup inputButtons;

	public void Pause()
	{
		Time.timeScale = 0.0f;

		GetComponent<MenuPanelHandler> ().DeactivatePanel (inputButtons);
	}

	public void UnPause()
	{
		Time.timeScale = 1.0f;
		
		GetComponent<MenuPanelHandler> ().ActivatePanel (inputButtons);
	}
}
