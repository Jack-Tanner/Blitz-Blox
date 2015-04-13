using UnityEngine;
using System.Collections;

public class MenuPanelHandler : MonoBehaviour {

	public void ActivatePanel(CanvasGroup panel)
	{
		panel.alpha = 1;
		panel.interactable = true;
		panel.blocksRaycasts = true;
	}
	
	public void DeactivatePanel(CanvasGroup panel)
	{
		panel.alpha = 0;
		panel.interactable = false;
		panel.blocksRaycasts = false;
	}

    public void QuitGame()
    {
        Application.Quit();
    }
}
