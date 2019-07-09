using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveItem
{
	[SerializeField]
	private Sprite waveSprite;
	[SerializeField]
	private string poolName;
	[SerializeField]
	private string prefabName;
	[SerializeField]
	private Vector2 spawnPos;

	public Sprite WaveSprite
	{
		get
		{
			return waveSprite;
		}

		set
		{
			waveSprite = value;
		}
	}

	public string PoolName
	{
		get
		{
			return poolName;
		}

		set
		{
			poolName = value;
		}
	}

	public string PrefabName
	{
		get
		{
			return prefabName;
		}

		set
		{
			prefabName = value;
		}
	}

	public Vector2 SpawnPos
	{
		get
		{
			return spawnPos;
		}

		set
		{
			spawnPos = value;
		}
	}
}

public enum WavePattern
{
	Line,
	Grid,
	Circle,
	Free
}