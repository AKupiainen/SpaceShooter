using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : GenericSingleton<LevelManager>
{
	private int enemiesDestroyed;
	private float gameTime;
	private byte starCount;
	private int overAllEnemies;

	private void Update()
	{
		if(GameManager.Instance.CurrentState != GameState.GamePlay)
		{
			return;
		}

		gameTime += Time.deltaTime;
	}

	public int EnemiesDestroyed
	{
		get
		{
			return enemiesDestroyed;
		}

		set
		{
			enemiesDestroyed = value;
		}
	}

	public float GameTime
	{
		get
		{
			return gameTime;
		}

		set
		{
			gameTime = value;
		}
	}

	public byte StarCount
	{
		get
		{
			float percent = overAllEnemies / enemiesDestroyed * 100f;

			if (percent < 33)
			{
				return 1;
			}

			else if (percent >= 33 && percent < 66)
			{
				return 2;
			}

			else
			{
				return 3;
			}
		}

		set
		{
			starCount = value;
		}
	}

	public int OverAllEnemies
	{
		get
		{
			return overAllEnemies;
		}

		set
		{
			overAllEnemies = value;
		}
	}
}
