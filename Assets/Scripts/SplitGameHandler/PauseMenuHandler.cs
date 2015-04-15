using UnityEngine;
using System.Collections;

public class PauseMenuHandler : MonoBehaviour {

	public CanvasGroup menuPanel;

	void Update () 
	{
		if(Input.GetKeyDown (KeyCode.Menu) && GetComponent<GameHandlerScript>().gameRunning)
		{
			if(Time.timeScale == 0.0f)
			{
				GetComponent<PauseUnpause>().UnPause();
				GetComponent<MenuPanelHandler>().DeactivatePanel(menuPanel);
			}
			else
			{
				GetComponent<PauseUnpause>().Pause();
				GetComponent<MenuPanelHandler>().ActivatePanel(menuPanel);
			}
		}
	}
}
