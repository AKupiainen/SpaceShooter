using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	private void Awake()
	{
		foreach (Button button in UIManager.Instance.Buttons)
		{
			if (button.transform.parent == transform)
			{
				button.onClick.AddListener(() => OnClickButton());
			}
		}
	}

	void OnClickButton()
	{
		if (UIManager.Instance.CurrentClickedButton.name == "SettingsBtn")
		{
			GameObject go = UIManager.Instance.Menus.Find(x => x.name == "Settings");
			UIManager.Instance.ChangeUIState(go);
		}

		if (UIManager.Instance.CurrentClickedButton.name == "QuitBtn")
		{
			SceneManager.LoadScene("Menus");
		}

		if (UIManager.Instance.CurrentClickedButton.name == "ReplayBtn")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		if(UIManager.Instance.CurrentClickedButton.name == "BackBtn")
		{
			GameObject go = UIManager.Instance.Menus.Find(x => x.name == "GameUI");
			UIManager.Instance.ChangeUIState(go);
			Time.timeScale = 1f;
		}
	}
}
