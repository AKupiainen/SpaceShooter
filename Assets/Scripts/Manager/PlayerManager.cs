using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class PlayerManager : GenericSingleton<PlayerManager>
{
	private float maxHealth;
	private float currentHealth;

	private int playerLifes;

	private int playerScore;
	private int playerCoinCount;

	private float currentShieldCharge;
	private float maxShieldCharge;

	private bool isPlayerHit;
	private GameObject player;
	private GameObject shield;

	private PickUpType pickType;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);

		playerLifes = 3;

		isPlayerHit = false;
		pickType = PickUpType.None;

		float percent = (float)GameManager.Instance.PlayerData.Upgrades.Find(x => x.Title == "Shield Upgrade").Rank / 10;

		currentShieldCharge = GameManager.Instance.PlayerData.PlayerShieldCharge + (GameManager.Instance.PlayerData.PlayerShieldCharge * percent);
		maxShieldCharge = currentShieldCharge;

		playerCoinCount = GameManager.Instance.PlayerData.Coincount;
	}

	private void Update()
	{
		if(GameManager.Instance.CurrentState != GameState.GamePlay)
		{
			return;
		}

		Shield.SetActive(currentShieldCharge > 0);

		if(playerLifes == 0)
		{
			GameManager.Instance.ChangeGameState(GameState.GameOver);

			MasterAudio.StopBus("SFX");

			GameObject go = UIManager.Instance.Menus.Find(x => x.name == "GameOver");
			UIManager.Instance.ChangeUIState(go);
		}

		if(CurrentHealth <= 0)
		{
			CurrentShieldCharge = MaxShieldCharge;
			CurrentHealth = MaxHealth;
			playerLifes--;
			isPlayerHit = true;
		}

		if(CurrentShieldCharge > 0 && Shield.activeSelf)
		{
			if (CurrentShieldCharge < MaxShieldCharge * .3f)
			{
				Renderer rend = Shield.GetComponent<Renderer>();
				rend.material.SetColor("_MainColor", Color.red);
			}

			else
			{
				Renderer rend = Shield.GetComponent<Renderer>();
				rend.material.SetColor("_MainColor", new Color(0, 0.33f, 0.28f, 1f));
			}
		}

		if (isPlayerHit)
		{
			StartCoroutine(OnHit());
		}
	}

	IEnumerator OnHit()
	{
		isPlayerHit = false;

		Player.gameObject.GetComponent<BoxCollider2D>().enabled = false;

		yield return new WaitForSeconds(1f);

		Player.gameObject.GetComponent<BoxCollider2D>().enabled = true;

		yield return null;
	}


	public int PlayerScore
	{
		get
		{
			return playerScore;
		}

		set
		{
			playerScore = value;
		}
	}

	public int PlayerLifes
	{
		get
		{
			return playerLifes;
		}

		set
		{
			playerLifes = value;
		}
	}

	public float CurrentShieldCharge
	{
		get
		{
			return currentShieldCharge;
		}

		set
		{
			currentShieldCharge = value;
		}
	}

	public float MaxShieldCharge
	{
		get
		{
			return maxShieldCharge;
		}

		set
		{
			maxShieldCharge = value;
		}
	}

	public bool IsPlayerHit
	{
		get
		{
			return isPlayerHit;
		}

		set
		{
			isPlayerHit = value;
		}
	}

	public PickUpType PickType
	{
		get
		{
			return pickType;
		}

		set
		{
			pickType = value;
		}
	}

	public int PlayerCoinCount
	{
		get
		{
			return playerCoinCount;
		}

		set
		{
			playerCoinCount = value;
		}
	}

	public GameObject Player
	{
		get
		{
			if(player == null)
			{
				player = GameObject.Find("Player");
			}

			return player;
		}

		set
		{
			player = value;
		}
	}

	public GameObject Shield
	{
		get
		{
			if (shield == null)
			{
				shield = Player.transform.Find("Shield").gameObject;
			}

			return shield;
		}

		set
		{
			shield = value;
		}
	}

	public float MaxHealth
	{
		get
		{
			return maxHealth;
		}

		set
		{
			maxHealth = value;
		}
	}

	public float CurrentHealth
	{
		get
		{
			return currentHealth;
		}

		set
		{
			currentHealth = value;
		}
	}
}