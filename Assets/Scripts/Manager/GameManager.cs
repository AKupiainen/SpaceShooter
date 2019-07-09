using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GenericSingleton<GameManager>
{
	private PlayerData playerData;
	private GameDifficulty currentDifficulty;
	private ShipData currentShip;
	private GameState currentState;

	void Awake ()
	{
		DontDestroyOnLoad(gameObject);

		PlayerData = Resources.Load<PlayerData>("PlayerData");
		currentDifficulty = PlayerData.Difficulty;
	}

	public void ChangeGameState(GameState state)
	{
		currentState = state;

		switch(state)
		{
			case GameState.GamePlay:
				Time.timeScale = 1;
				Debug.Log(Time.timeScale);
				break;
			case GameState.Pause:
				Time.timeScale = 0;
				break;
			case GameState.Victory:
				Time.timeScale = 0;
				break;
			case GameState.InMenus:
				Time.timeScale = 1;
				break;
			case GameState.GameOver:
				Time.timeScale = 0;
				break;
		}
	}

	public GameDifficulty CurrentDifficulty
	{
		get
		{
			return currentDifficulty;
		}

		set
		{
			currentDifficulty = value;
		}
	}

	public PlayerData PlayerData
	{
		get
		{
			return playerData;
		}

		set
		{
			playerData = value;
		}
	}

	public ShipData CurrentShip
	{
		get
		{
			return currentShip;
		}

		set
		{
			currentShip = value;
		}
	}

	public GameState CurrentState
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
}

public enum GameDifficulty
{
	Easy = 0,
	Medium = 1,
	Hard = 2
}

public enum PickUpType
{
	None = 0,
	HomingMissile = 1,
	ExtraLife = 2,
	RapidShot = 3,
}

public enum GameState
{
	InMenus,
	GamePlay,
	Pause,
	GameOver,
	Victory
}