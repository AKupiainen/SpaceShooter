using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	private void Awake()
	{
		UIManager.Instance.SceneChanged();

		foreach(Button button in UIManager.Instance.Buttons)
		{
			if(button.transform.parent == transform )
			{
				button.onClick.AddListener(() => OnClickButton());
			}
		}
	}

	private void OnEnable()
	{
		GameManager.Instance.ChangeGameState(GameState.InMenus);
	}

	void OnClickButton()
	{
		if (UIManager.Instance.CurrentClickedButton.name == "SingleGameBtn")
		{
			GameObject go = UIManager.Instance.Menus.Find(x => x.name == "ShipSelection");
			UIManager.Instance.ChangeUIState(go);
		}

		if (UIManager.Instance.CurrentClickedButton.name == "SettingsBtn")
		{
			GameObject go = UIManager.Instance.Menus.Find(x => x.name == "Settings");
			UIManager.Instance.ChangeUIState(go);
		}

		if (UIManager.Instance.CurrentClickedButton.name == "ArmoryBtn")
		{
			GameObject go = UIManager.Instance.Menus.Find(x => x.name == "Armory");
			UIManager.Instance.ChangeUIState(go);
		}

		if (UIManager.Instance.CurrentClickedButton.name == "CreditsBtn")
		{
			GameObject go = UIManager.Instance.Menus.Find(x => x.name == "Credits");
			UIManager.Instance.ChangeUIState(go);
		}

		if (UIManager.Instance.CurrentClickedButton.name == "HighScoreBtn")
		{
			GameObject go = UIManager.Instance.Menus.Find(x => x.name == "Scores");
			UIManager.Instance.ChangeUIState(go);
		}
	}
}
