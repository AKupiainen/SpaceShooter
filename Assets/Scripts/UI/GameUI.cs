using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class GameUI : MonoBehaviour
{
	private Image currentPickUp;
	private Text currentLife;
	private Image lifeBar;
	private Text playerScoreText;
	private Image playerShieldCharge;
	private Image currentPickUpIndicatorSpr;
	private Text currentPickUpText;
	private int currentLives;

	private void Awake()
	{
		UIManager.Instance.SceneChanged();
	}

	private void Start()
	{
		currentPickUp = GameObject.Find("Canvas/Menus/GameUI/Hud/LeftHudFrame/CurrentPickUp").GetComponent<Image>();
		currentLife = GameObject.Find("Canvas/Menus/GameUI/Hud/LeftHudFrame/CurrentLifeText").GetComponent<Text>();
		lifeBar = GameObject.Find("Canvas/Menus/GameUI/Hud/LeftHudFrame/Lifebar").GetComponent<Image>();
		playerScoreText = GameObject.Find("Canvas/Menus/GameUI/Hud/RightHudFrame/PlayerScore").GetComponent<Text>();
		playerShieldCharge = GameObject.Find("Canvas/Menus/GameUI/ShieldIndicator/Shieldbar/Foreground").GetComponent<Image>();
		currentPickUpIndicatorSpr = GameObject.Find("Canvas/Menus/GameUI/PickUPIndicator/CurrentPowerUp").GetComponent<Image>();
		currentPickUpText = GameObject.Find("Canvas/Menus/GameUI/PickUPIndicator/CurrentPickUpText").GetComponent<Text>();

		foreach (Button button in UIManager.Instance.Buttons)
		{
			if (button.transform.parent == transform)
			{
				button.onClick.AddListener(() => OnClickButton());
			}
		}

		RectTransform parent = GameObject.Find("Canvas/Menus/GameUI/Hud/RightHudFrame/Lives").GetComponent<RectTransform>();

		for (int i = 0; i <  PlayerManager.Instance.PlayerLifes - 1; i++)
		{
			GameObject go = PoolManager.Instance.SpawnPools["Misc"].Spawn("Heart");
			go.GetComponent<RectTransform>().SetParent(parent);
		}

		currentPickUp.sprite = UIManager.Instance.UiSprites.Find(x => x.name == PlayerManager.Instance.PickType.ToString());
		currentPickUpIndicatorSpr.sprite = currentPickUp.sprite;

		parent = GameObject.Find("Canvas/Menus/GameUI/PickUPIndicator").GetComponent<RectTransform>();
		parent.gameObject.SetActive(false);

		currentLives = PlayerManager.Instance.PlayerLifes;
	}

	private void Update()
	{
		currentLife.text = (PlayerManager.Instance.CurrentHealth / PlayerManager.Instance.MaxHealth * 100f).ToString() + "%";
		lifeBar.fillAmount = Mathf.Clamp((PlayerManager.Instance.CurrentHealth / PlayerManager.Instance.MaxHealth), 0.07f, 1f);
		playerShieldCharge.fillAmount = PlayerManager.Instance.CurrentShieldCharge / PlayerManager.Instance.MaxShieldCharge;
		playerScoreText.text = PlayerManager.Instance.PlayerScore.ToString();
		currentPickUpText.text = PlayerManager.Instance.PickType.ToString();
		currentPickUp.sprite = UIManager.Instance.UiSprites.Find(x => x.name == PickUpManager.Instance.CurrentPickUp.ToString());

		if(PlayerManager.Instance.PlayerLifes > currentLives)
		{
			RectTransform parent = GameObject.Find("Canvas/Menus/GameUI/Hud/RightHudFrame/Lives").GetComponent<RectTransform>();

			PoolManager.Instance.SpawnPools["Misc"].Spawn("Heart");
			GameObject go = PoolManager.Instance.SpawnPools["Misc"].Spawn("Heart");
			go.GetComponent<RectTransform>().SetParent(parent);

			currentLives = PlayerManager.Instance.PlayerLifes;
		}

		if (PlayerManager.Instance.PlayerLifes < currentLives)
		{
			RectTransform parent = GameObject.Find("Canvas/Menus/GameUI/Hud/RightHudFrame/Lives").GetComponent<RectTransform>();
			int index = PlayerManager.Instance.PlayerLifes;

			PoolManager.Instance.SpawnPools["Misc"].Despawn(parent.transform.GetChild(index).gameObject);
			currentLives = PlayerManager.Instance.PlayerLifes;
		}
	}

	private void OnClickButton()
	{
		if(UIManager.Instance.CurrentClickedButton.gameObject.name == "PauseBtn" && Time.timeScale == 1f)
		{
			GameObject go = UIManager.Instance.Menus.Find(x => x.name == "PauseMenu");
			GameManager.Instance.ChangeGameState(GameState.Pause);
			UIManager.Instance.ChangeUIState(go);
		}
	}
}