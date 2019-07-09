using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class MissionSelection : MonoBehaviour
{
	public List<GameObject> missions;

	private void Start()
	{
		missions = new List<GameObject>();

		foreach (Button button in UIManager.Instance.Buttons)
		{
			if (button.transform.parent == transform)
			{
				button.onClick.AddListener(() => OnClickButton());

			}

			if (button.name.StartsWith("Frame"))
			{
				button.onClick.AddListener(() => OnClickButton());
				missions.Add(button.gameObject);
			}
		}

		missions = missions.OrderBy(x => int.Parse(Regex.Match(x.gameObject.name, @"\d+").Value)).ToList();
		ChangeMissionDetails();
	}

	private void OnEnable()
	{
		ChangeMissionDetails();
	}

	void OnClickButton()
	{
		if (UIManager.Instance.CurrentClickedButton.name == "MissionBackBtn")
		{
			GameObject go = UIManager.Instance.Menus.Find(x => x.name == "ShipSelection");
			UIManager.Instance.ChangeUIState(go);
		}

		if (missions.Contains(UIManager.Instance.CurrentClickedButton.gameObject))
		{
			string name = UIManager.Instance.CurrentClickedButton.transform.parent.name;
			SceneManager.LoadScene(name);

			PlayerManager.Instance.CurrentHealth = GameManager.Instance.PlayerData.PlayerHealth + (GameManager.Instance.PlayerData.PlayerHealth * ((float)GameManager.Instance.CurrentShip.Endurance / 10));
			PlayerManager.Instance.MaxHealth = PlayerManager.Instance.CurrentHealth;
		}
	}

	void ChangeMissionDetails()
	{
		for(int i = 0; i < missions.Count; i++)
		{
			if(GameManager.Instance.PlayerData.LevelData[i].IsUnlocked)
			{
				missions[i].transform.GetChild(0).gameObject.SetActive(true);
				missions[i].transform.GetChild(1).gameObject.SetActive(false);
				missions[i].GetComponent<Button>().interactable = true;
				missions[i].transform.GetChild(0).GetComponent<Image>().sprite = UIManager.Instance.UiSprites.Find(x => x.name == "stars" + GameManager.Instance.PlayerData.LevelData[i].StarCount.ToString());
			}

			else
			{
				missions[i].transform.GetChild(0).gameObject.SetActive(false);
				missions[i].transform.GetChild(1).gameObject.SetActive(true);
				missions[i].GetComponent<Button>().interactable = false;
			}
		}
	}
}
