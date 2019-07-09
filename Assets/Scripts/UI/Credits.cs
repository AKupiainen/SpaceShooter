using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
	private void Start()
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
		if (UIManager.Instance.CurrentClickedButton.name == "CreditsBackBtn")
		{
			GameObject go = UIManager.Instance.Menus.Find(x => x.name == "MainMenu");
			UIManager.Instance.ChangeUIState(go);
		}
	}

}
