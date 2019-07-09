using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Armory : MonoBehaviour
{
	private const int itemsPerTab = 3;
	private int armoryTabIndex;
	private Text coinCountText;
	private List<Image> upgradeLogo;
	private List<Image> upgradeRankings;
	private List<Text> upgradeTitles;
	private List<Text> upgradeDescriptions;
	private List<Image> upgradeButtonIcons;
	private List<Text> upgradeButtonText;

	private void Awake()
	{
		armoryTabIndex = 0;
		coinCountText = GameObject.Find("Canvas/Menus/Armory/CoinCount").GetComponent<Text>();

		upgradeLogo = new List<Image>();
		FindUpgradeLogos();

		upgradeRankings = new List<Image>();
		FindUpgradeRankings();

		upgradeTitles = new List<Text>();
		FindUpgradeTitles();

		upgradeDescriptions = new List<Text>();
		FindUpgradeDescriptons();

		upgradeButtonIcons = new List<Image>();
		FindUpgradeIcons();

		upgradeButtonText = new List<Text>();
		FindUpgradeButtonTexts();
	}

	private void Start()
	{
		foreach (Button button in UIManager.Instance.Buttons)
		{
			if ((button.transform.parent == transform || button.transform.name == "BuyUpgradeBtn1"
				|| button.transform.name == "BuyUpgradeBtn2" || button.transform.name == "BuyUpgradeBtn3") && button.IsInteractable())
			{
				button.onClick.AddListener(() => OnClickButton());
			}
		}
	}

	private void OnEnable()
	{
		coinCountText.text = PlayerManager.Instance.PlayerCoinCount.ToString();

		ChangeUpgradeLogos();
		ChangeUpgradeRankings();
		ChangeUpgradeTitles();
		ChangeUpgradeDescriptions();
		DisableUpgradeButtons();
		ChangeUpgradePrices();
	}

	void OnClickButton()
	{
		if(UIManager.Instance.CurrentClickedButton.name == "ArmoryBackBtn")
		{
			GameObject go = UIManager.Instance.Menus.Find(x => x.name == "MainMenu");
			UIManager.Instance.ChangeUIState(go);
		}

		if (UIManager.Instance.CurrentClickedButton.name == "ArmoryNextBtn")
		{
			GameObject go = UIManager.Instance.Menus.Find(x => x.name == "ShipSelection");
			UIManager.Instance.ChangeUIState(go);
		}

		if(UIManager.Instance.CurrentClickedButton.gameObject.name == "ArmoryItemBackBtn")
		{
			if(armoryTabIndex > 0)
			{
				armoryTabIndex--;
				ChangeUpgradeLogos();
				ChangeUpgradeRankings();
				ChangeUpgradeTitles();
				ChangeUpgradeDescriptions();
				DisableUpgradeButtons();
			}
		}

		if (UIManager.Instance.CurrentClickedButton.gameObject.name == "ArmoryItemNextButton")
		{
			if(GameManager.Instance.PlayerData.Upgrades.Count >= itemsPerTab * (armoryTabIndex + 1) + 1)
			{
				armoryTabIndex++;
				ChangeUpgradeLogos();
				ChangeUpgradeRankings();
				ChangeUpgradeTitles();
				ChangeUpgradeDescriptions();
				DisableUpgradeButtons();
			}
		}

		if(UIManager.Instance.CurrentClickedButton.gameObject.name == "BuyUpgradeBtn1")
		{
			int id = (itemsPerTab * armoryTabIndex);

			if (GameManager.Instance.PlayerData.Upgrades[id].Rank < 5)
			{
				int currentRank = GameManager.Instance.PlayerData.Upgrades[id].Rank;
				int currency = PlayerManager.Instance.PlayerCoinCount;

				if (currency >= GameManager.Instance.PlayerData.Upgrades[id].Ranks[currentRank])
				{
					BuyUpgrade(id);
				}
			}
		}

		if (UIManager.Instance.CurrentClickedButton.gameObject.name == "BuyUpgradeBtn2")
		{
			int id = (itemsPerTab * armoryTabIndex) + 1;

			if (GameManager.Instance.PlayerData.Upgrades[id].Rank < 5)
			{
				int currentRank = GameManager.Instance.PlayerData.Upgrades[id].Rank;
				int currency = PlayerManager.Instance.PlayerCoinCount;

				if (currency >= GameManager.Instance.PlayerData.Upgrades[id].Ranks[currentRank])
				{
					BuyUpgrade(id);
				}
			}
		}

		if (UIManager.Instance.CurrentClickedButton.gameObject.name == "BuyUpgradeBtn3")
		{
			int id = (itemsPerTab * armoryTabIndex) + 2;

			if (GameManager.Instance.PlayerData.Upgrades[id].Rank < 5)
			{
				int currentRank = GameManager.Instance.PlayerData.Upgrades[id].Rank;
				int currency = PlayerManager.Instance.PlayerCoinCount;

				if (currency >= GameManager.Instance.PlayerData.Upgrades[id].Ranks[currentRank])
				{
					BuyUpgrade(id);
				}
			}
		}
	}

	void FindUpgradeLogos()
	{
		RectTransform parent = GameObject.Find("Canvas/Menus/Armory/UpgradeContent").GetComponent<RectTransform>();

		foreach(RectTransform child in parent)
		{
			upgradeLogo.Add(child.Find("Logo").GetComponent<Image>());
		}
	}

	void ChangeUpgradeLogos()
	{
		int startingIdex = itemsPerTab * armoryTabIndex + 1;

		foreach (Image logo in upgradeLogo)
		{
			if(startingIdex <= GameManager.Instance.PlayerData.Upgrades.Count)
			{
				logo.sprite = GameManager.Instance.PlayerData.Upgrades[startingIdex - 1].Icon;
			}

			startingIdex++;
		}
	}

	void FindUpgradeRankings()
	{
		RectTransform parent = GameObject.Find("Canvas/Menus/Armory/UpgradeContent").GetComponent<RectTransform>();

		foreach (RectTransform child in parent)
		{
			upgradeRankings.Add(child.Find("Ranking").GetComponent<Image>());
		}
	}

	void ChangeUpgradeRankings()
	{
		int startingIdex = itemsPerTab * armoryTabIndex + 1;

		for(int i = 0; i < upgradeRankings.Count; i++)
		{
			if (startingIdex <= GameManager.Instance.PlayerData.Upgrades.Count)
			{
				upgradeRankings[i].sprite = UIManager.Instance.UiSprites.Find(x => x.name == "rank" + GameManager.Instance.PlayerData.Upgrades[startingIdex - 1].Rank.ToString());
			}

			startingIdex++;
		}
	}

	void FindUpgradeTitles()
	{
		RectTransform parent = GameObject.Find("Canvas/Menus/Armory/UpgradeContent").GetComponent<RectTransform>();

		foreach (RectTransform child in parent)
		{
			upgradeTitles.Add(child.Find("DescriptionTitle").GetComponent<Text>());
		}
	}

	void ChangeUpgradeTitles()
	{
		int startingIdex = itemsPerTab * armoryTabIndex + 1;

		for (int i = 0; i < upgradeTitles.Count; i++)
		{
			if (startingIdex <= GameManager.Instance.PlayerData.Upgrades.Count)
			{
				upgradeTitles[i].text = GameManager.Instance.PlayerData.Upgrades[startingIdex - 1].Title;
			}

			startingIdex++;
		}
	}

	void FindUpgradeDescriptons()
	{
		RectTransform parent = GameObject.Find("Canvas/Menus/Armory/UpgradeContent").GetComponent<RectTransform>();

		foreach (RectTransform child in parent)
		{
			upgradeDescriptions.Add(child.Find("Description").GetComponent<Text>());
		}
	}

	void ChangeUpgradeDescriptions()
	{
		int startingIdex = itemsPerTab * armoryTabIndex + 1;

		for (int i = 0; i < upgradeDescriptions.Count; i++)
		{
			if (startingIdex <= GameManager.Instance.PlayerData.Upgrades.Count)
			{
				upgradeDescriptions[i].text = GameManager.Instance.PlayerData.Upgrades[startingIdex - 1].Desc;
			}

			startingIdex++;
		}
	}

	void FindUpgradeIcons()
	{
		RectTransform parent = GameObject.Find("Canvas/Menus/Armory/UpgradeContent").GetComponent<RectTransform>();
		int index = 1;

		foreach (RectTransform child in parent)
		{
			upgradeButtonIcons.Add(child.Find("BuyUpgradeBtn" + index.ToString() + "/Image").GetComponent<Image>());
			index++;
		}
	}

	void FindUpgradeButtonTexts()
	{
		RectTransform parent = GameObject.Find("Canvas/Menus/Armory/UpgradeContent").GetComponent<RectTransform>();
		int index = 1;

		foreach (RectTransform child in parent)
		{
			upgradeButtonText.Add(child.Find("BuyUpgradeBtn" + index.ToString() + "/Text").GetComponent<Text>());
			index++;
		}
	}

	void DisableUpgradeButtons()
	{
		int startingIdex = itemsPerTab * armoryTabIndex;

		for (int i = 0; i < 3; i++, startingIdex++)
		{
			if(startingIdex >= GameManager.Instance.PlayerData.Upgrades.Count)
			{
				break;
			}

			if (GameManager.Instance.PlayerData.Upgrades[startingIdex].Rank == 5)
			{
				DisableUpgradeButton(i);
			}
		}
	}

	void BuyUpgrade(int id)
	{
		PlayerManager.Instance.PlayerCoinCount -= GameManager.Instance.PlayerData.Upgrades[id].Ranks[GameManager.Instance.PlayerData.Upgrades[id].Rank];
		GameManager.Instance.PlayerData.Upgrades[id].Rank += 1;

		if (GameManager.Instance.PlayerData.Upgrades[id].Rank < 5)
		{
			ChangeUpgradePrice(id);
		}

		if (GameManager.Instance.PlayerData.Upgrades[id].Rank == 5)
		{
			DisableUpgradeButton(id);
		}

		ChangeUpgradeRankings();
	}

	void DisableUpgradeButton(int index)
	{
		upgradeButtonIcons[index].sprite = UIManager.Instance.UiSprites.Find(x => x.name == "lock");
	}
	
	void ChangeUpgradePrice(int index)
	{
		int rank = GameManager.Instance.PlayerData.Upgrades[index].Rank;
		upgradeButtonText[index].text = GameManager.Instance.PlayerData.Upgrades[index].Ranks[rank].ToString();
	}

	void ChangeUpgradePrices()
	{
		int startingIdex = itemsPerTab * armoryTabIndex;

		for (int i = 0; i < upgradeButtonText.Count; i++, startingIdex++)
		{
			if(GameManager.Instance.PlayerData.Upgrades[startingIdex].Rank > 0)
			{
				upgradeButtonText[i].text = GameManager.Instance.PlayerData.Upgrades[startingIdex].Ranks[GameManager.Instance.PlayerData.Upgrades[startingIdex].Rank - 1].ToString();
			}

			else
			{
				upgradeButtonText[i].text = "1000";
			}
		}
	}
}