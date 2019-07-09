using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
	[SerializeField]
	private int levelId;
	[SerializeField]
	private bool isUnlocked;
	[SerializeField]
	private byte starCount;

	public int LevelId
	{
		get
		{
			return levelId;
		}

		set
		{
			levelId = value;
		}
	}

	public bool IsUnlocked
	{
		get
		{
			return isUnlocked;
		}

		set
		{
			isUnlocked = value;
		}
	}

	public byte StarCount
	{
		get
		{
			return starCount;
		}

		set
		{
			starCount = value;
		}
	}
}
