using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
	[SerializeField]
	private string playerName;
	[SerializeField]
	private List<PlayerScore> playerScore;
	[SerializeField]
	private float volume;
	[SerializeField]
	private GameDifficulty difficulty;
	[SerializeField]
	private bool isMuted;
	[SerializeField]
	private bool isViberated;
	[SerializeField]
	private List<LevelData> levelData;
	[SerializeField]
	private List<Upgrade> upgrades;
	[SerializeField]
	private int coincount;
	[SerializeField]
	private List<ShipData> shipdata;
	[SerializeField]
	private List<ShipData> ownedShips;
	[SerializeField]
	private float playerHealth;
	[SerializeField]
	private float playerShieldCharge;
	[SerializeField]
	private List<RuntimeAnimatorController> enemyShipControllers;

	public string PlayerName
	{
		get
		{
			return playerName;
		}
		
		set
		{
			playerName = value;
		}
	}

	public List<PlayerScore> PlayerScore
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

	public float Volume
	{
		get
		{
			return volume;
		}

		set
		{
			volume = value;
		}
	}

	public GameDifficulty Difficulty
	{
		get
		{
			return difficulty;
		}

		set
		{
			difficulty = value;
		}
	}

	public bool IsMuted
	{
		get
		{
			return isMuted;
		}

		set
		{
			isMuted = value;
		}
	}

	public bool IsViberated
	{
		get
		{
			return isViberated;
		}

		set
		{
			isViberated = value;
		}
	}

	public List<LevelData> LevelData
	{
		get
		{
			return levelData;
		}

		set
		{
			levelData = value;
		}
	}

	public List<Upgrade> Upgrades
	{
		get
		{
			return upgrades;
		}

		set
		{
			upgrades = value;
		}
	}

	public int Coincount
	{
		get
		{
			return coincount;
		}

		set
		{
			coincount = value;
		}
	}

	public List<ShipData> Shipdata
	{
		get
		{
			return shipdata;
		}

		set
		{
			shipdata = value;
		}
	}

	public List<ShipData> OwnedShips
	{
		get
		{
			return ownedShips;
		}

		set
		{
			ownedShips = value;
		}
	}

	public float PlayerHealth
	{
		get
		{
			return playerHealth;
		}

		set
		{
			playerHealth = value;
		}
	}

	public float PlayerShieldCharge
	{
		get
		{
			return playerShieldCharge;
		}

		set
		{
			playerShieldCharge = value;
		}
	}

	public List<RuntimeAnimatorController> EnemyShipControllers
	{
		get
		{
			return enemyShipControllers;
		}

		set
		{
			enemyShipControllers = value;
		}
	}
}

public enum ShipType
{
	Elysium,
	Omen,
	Ravager,
	Gladious,
	Avenger,
	Spitfire
}