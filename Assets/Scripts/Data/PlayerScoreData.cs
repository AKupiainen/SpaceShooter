using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerScore
{
	public PlayerScore(string name, int score)
	{
		this.name = name;
		this.score = score;
	}

	[SerializeField]
	private string name;
	[SerializeField]
	private int score;

	public string PlayerName
	{
		get
		{
			return name;
		}

		set
		{
			name = value;
		}
	}

	public int Score
	{
		get
		{
			return score;
		}

		set
		{
			score = value;
		}
	}
}
