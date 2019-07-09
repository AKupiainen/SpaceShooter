using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ShipSelection : MonoBehaviour
{
	private int selectedShipIndex;
	private Text model;
	private Image speedSpr;
	private Image enduranceSpr;
	private Image shipSprite;
	private Text coinCount;

	private void Awake()
	{
		selectedShipIndex = 0;
		model = GameObject.Find("Canvas/Menus/ShipSelection/ShipInformation/Model").GetComponent<Text>();
		speedSpr = GameObject.Find("Canvas/Menus/ShipSelection/ShipInformation/Speed").GetComponent<Image>();
		enduranceSpr = GameObject.Find("Canvas/Menus/ShipSelection/ShipInformation/Endurance").GetComponent<Image>();
		shipSprite = GameObject.Find("Canvas/Menus/ShipSelection/SelectedShip").GetComponent<Image>();
		coinCount = GameObject.Find("Canvas/Menus/ShipSelection/CoinCount").GetComponent<Text>();
	}

	private void Start()
	{
		foreach (Button button in UIManager.Instance.Buttons)
		{
			button.onClick.AddListener(() => OnClickButton());
		}
	}

	private void OnEnable()
	{
		coinCount.text = PlayerManager.Instance.PlayerCoinCount.ToString();
		ChangeShipDetails(selectedShipIndex);

		bool isOwned = GameManager.Instance.PlayerData.OwnedShips.Any(x => x.Type == GameManager.Instance.PlayerData.Shipdata[selectedShipIndex].Type);

		if (isOwned)
		{
			DisableBuyButton();
		}

		else
		{
			EnableBuyButton(GameManager.Instance.PlayerData.Shipdata[selectedShipIndex].Cost);
		}
	}

	private void OnDisable()
	{
		bool isOwned = GameManager.Instance.PlayerData.OwnedShips.Any(x => x.Type == GameManager.Instance.PlayerData.Shipdata[selectedShipIndex].Type);

		if(isOwned)
		{
			GameManager.Instance.CurrentShip = GameManager.Instance.PlayerData.Shipdata[selectedShipIndex];
		}

		else
		{
			GameManager.Instance.CurrentShip = GameManager.Instance.PlayerData.OwnedShips.First();
		}
	}

	void OnClickButton()
	{
		if (UIManager.Instance.CurrentClickedButton.name == "ShipSelectBackBtn")
		{
			GameObject go = UIManager.Instance.Menus.Find(x => x.name == "MainMenu");
			UIManager.Instance.ChangeUIState(go);
		}

		if (UIManager.Instance.CurrentClickedButton.name == "ShipSelectNextBtn")
		{
			GameObject go = UIManager.Instance.Menus.Find(x => x.name == "MissionSelection");
			UIManager.Instance.ChangeUIState(go);
		}

		if (UIManager.Instance.CurrentClickedButton.name == "ShipBackBtn")
		{
			if (selectedShipIndex > 0)
			{
				selectedShipIndex--;
				ChangeShipDetails(selectedShipIndex);

				bool isOwned = GameManager.Instance.PlayerData.OwnedShips.Any(x => x.Type == GameManager.Instance.PlayerData.Shipdata[selectedShipIndex].Type);

				if (isOwned)
				{
					DisableBuyButton();
				}

				else
				{
					EnableBuyButton(GameManager.Instance.PlayerData.Shipdata[selectedShipIndex].Cost);
				}
			}
		}

		if (UIManager.Instance.CurrentClickedButton.name == "ShipNextBtn")
		{
			if (selectedShipIndex < GameManager.Instance.PlayerData.Shipdata.Count - 1)
			{
				selectedShipIndex++;
				ChangeShipDetails(selectedShipIndex);

				bool isOwned = GameManager.Instance.PlayerData.OwnedShips.Any(x => x.Type == GameManager.Instance.PlayerData.Shipdata[selectedShipIndex].Type);

				if (isOwned)
				{
					DisableBuyButton();
				}

				else
				{
					EnableBuyButton(GameManager.Instance.PlayerData.Shipdata[selectedShipIndex].Cost);
				}
			}
		}

		if(UIManager.Instance.CurrentClickedButton.name == "BuyShipBtn")
		{
			BuyShip(selectedShipIndex);
		}
	}

	void ChangeShipDetails(int id)
	{
		model.text = GameManager.Instance.PlayerData.Shipdata[id].Type.ToString();
		speedSpr.sprite = UIManager.Instance.UiSprites.Find(x => x.name == "rank" + GameManager.Instance.PlayerData.Shipdata[id].Speed.ToString());
		enduranceSpr.sprite = UIManager.Instance.UiSprites.Find(x => x.name == "rank" + GameManager.Instance.PlayerData.Shipdata[id].Endurance.ToString());
		shipSprite.sprite = GameManager.Instance.PlayerData.Shipdata[id].ShipSprite;
	}

	void BuyShip(int index)
	{
		if(GameManager.Instance.PlayerData.Coincount >= GameManager.Instance.PlayerData.Shipdata[index].Cost)
		{
			GameManager.Instance.PlayerData.Coincount -= GameManager.Instance.PlayerData.Shipdata[index].Cost;
			GameManager.Instance.PlayerData.OwnedShips.Add(GameManager.Instance.PlayerData.Shipdata[index]);

			DisableBuyButton();
		}
	}

	void DisableBuyButton()
	{
		Button buyButton = UIManager.Instance.Buttons.Find(x => x.name == "BuyShipBtn");
		buyButton.interactable = false;

		buyButton.transform.GetChild(0).GetComponent<Text>().text = "Owned";
		buyButton.transform.GetChild(1).GetComponent<Image>().sprite = UIManager.Instance.UiSprites.Find(x => x.name == "lock");
	}

	void EnableBuyButton(int price)
	{
		Button buyButton = UIManager.Instance.Buttons.Find(x => x.name == "BuyShipBtn");
		buyButton.interactable = true;

		buyButton.transform.GetChild(0).GetComponent<Text>().text = price.ToString();
		buyButton.transform.GetChild(1).GetComponent<Image>().sprite = UIManager.Instance.UiSprites.Find(x => x.name == "ok");
	}
}