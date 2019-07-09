using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : GenericSingleton<HighScoreManager>
{
	private const int maxHighScores = 24;
	private PlayerData playerData;
	public List<PlayerScore> playerScores;

	private void Awake()
	{
		PlayerData = Resources.Load<PlayerData>("PlayerData");
		PlayerScores = PlayerData.PlayerScore;
	}

	public void GetHighScores()
	{
		SortHighScores();
	}

	public void SortHighScores()
	{
		for (int i = 0; i < PlayerScores.Count; i++)
		{
			for (int j = 0; j < PlayerScores.Count - 1; j++)
			{
				if (PlayerScores[j].Score < PlayerScores[j + 1].Score)
				{
					PlayerScore temp = PlayerScores[j + 1];
					PlayerScores[j + 1] = PlayerScores[j];
					PlayerScores[j] = temp;
				}
			}
		}
	}

	public static int MaxHighScores
	{
		get
		{
			return maxHighScores;
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

	public List<PlayerScore> PlayerScores
	{
		get
		{
			return playerScores;
		}

		set
		{
			playerScores = value;
		}
	}
}