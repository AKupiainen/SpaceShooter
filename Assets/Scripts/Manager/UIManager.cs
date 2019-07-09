using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.SceneManagement;

public class UIManager : GenericSingleton<UIManager>
{
	private List<Button> buttons;
	private GameObject currentState;
	private List<GameObject> menus;
	private List<Sprite> uiSprites;

	void Awake ()
	{
		buttons = new List<Button>();
		menus = new List<GameObject>();
		UiSprites = new List<Sprite>();

		GetUISprites();
	}

	public void ChangeUIState(GameObject state)
	{
		currentState.gameObject.SetActive(false);
		currentState = state;
		currentState.gameObject.SetActive(true);
	}

	public void GetUISprites()
	{
		Sprite[] sprites = Resources.LoadAll<Sprite>("UI");

		foreach(Sprite spr in sprites)
		{
			UiSprites.Add(spr);
		}
	}

	public void SceneChanged()
	{
		Transform allMenus = GameObject.Find("Canvas").transform;

		foreach (Button button in Resources.FindObjectsOfTypeAll(typeof(Button)) as Button[])
		{
			if (button.hideFlags == HideFlags.NotEditable || button.hideFlags == HideFlags.HideAndDontSave)
				continue;

			buttons.Add(button);
		}

		foreach (Transform menu in allMenus)
		{
			menus.Add(menu.gameObject);
			menu.gameObject.SetActive(false);
		}

		menus[0].SetActive(true);
		currentState = menus[0];
	}

	public List<GameObject> Menus
	{
		get
		{
			return menus;
		}

		set
		{
			menus = value;
		}
	}

	public GameObject CurrentState
	{
		get
		{
			return currentState;
		}

		set
		{
			currentState = value;
		}
	}

	public List<Button> Buttons
	{
		get
		{
			return buttons;
		}

		set
		{
			buttons = value;
		}
	}

	public GameObject CurrentClickedButton
	{
		get
		{
			return EventSystem.current.currentSelectedGameObject;
		}

	}

	public List<Sprite> UiSprites
	{
		get
		{
			return uiSprites;
		}

		set
		{
			uiSprites = value;
		}
	}
}