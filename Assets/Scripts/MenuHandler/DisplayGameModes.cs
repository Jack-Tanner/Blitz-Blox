using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayGameModes : MonoBehaviour {

	public CanvasGroup[] panels;

	public void ShowGameModes()
	{
		ActivatePanel (1);
	}

	public void ShowMainMenu()
	{
		ActivatePanel (0);
	}

	public void Quit()
	{
		Application.Quit ();
	}

	void ActivatePanel(int panelID)
	{
		for(int i = 0; i < panels.Length; i++)
		{
			if(i == panelID)
			{
				panels[i].alpha = 1;
				panels[i].interactable = true;
				panels[i].blocksRaycasts = true;
			}
			else
			{
				panels[i].alpha = 0;
				panels[i].interactable = false;
				panels[i].blocksRaycasts = false;
			}
		}
	}
}
