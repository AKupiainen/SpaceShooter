using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using DarkTonic.MasterAudio;

public class Spawner : MonoBehaviour
{
	[NonSerialized]
	public bool canSpawnWave;
	[NonSerialized]
	public int waveId;
	public List<Wave> waves;
	[SerializeField]
	private List<Sprite> brushSprites;
	public UnityEvent complete;

	private void Start()
	{
		canSpawnWave = true;
		waveId = 0;
	}

	public IEnumerator CountDown()
	{
		yield return new WaitForSeconds(20f);
		GameManager.Instance.ChangeGameState(GameState.Victory);
		GameObject go = UIManager.Instance.Menus.Find(x => x.name == "Victory");
		UIManager.Instance.ChangeUIState(go);

		int id = int.Parse(Regex.Match(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, @"\d+").Value);

		GameManager.Instance.PlayerData.LevelData[id - 1].StarCount = LevelManager.Instance.StarCount;
		GameManager.Instance.PlayerData.LevelData[id].IsUnlocked = true;
		MasterAudio.StopBus("SFX");

		string name = GameManager.Instance.PlayerData.PlayerName;
		int score = PlayerManager.Instance.PlayerScore;
		GameManager.Instance.PlayerData.PlayerScore.Add(new PlayerScore(name, score));

	}

	public void StartCountDown()
	{
		StartCoroutine(CountDown());
	}

	void Update()
	{
		if (canSpawnWave && waveId <= waves.Count)
		{
			StartCoroutine(SpawmWave());
		}

		if (waveId == waves.Count - 1 && complete != null)
		{
			canSpawnWave = false;
			complete.Invoke();
		}
	}

	/// <summary>
	/// Spawns wave
	/// </summary>
	/// <returns></returns>
	private IEnumerator SpawmWave()
	{
		canSpawnWave = false;
		Wave currentWave = waves[waveId];

		int count = currentWave.Pattern != WavePattern.Grid ? currentWave.WaveCount : currentWave.WaveCount * currentWave.WaveCount;

		for (int i = 0; i < count; i++)
		{
			string poolName = currentWave.WaveItems[i].PoolName;
			string prefabName = currentWave.WaveItems[i].PrefabName;

			currentWave.RemainingItems++;

			GameObject go = PoolManager.Instance.SpawnPools[poolName].Spawn(prefabName, currentWave.WaveItems[i].SpawnPos);
			go.GetComponent<SpriteRenderer>().sprite = currentWave.WaveItems[i].WaveSprite;

			Animator anim = go.GetComponent<Animator>();
			anim.runtimeAnimatorController = GameManager.Instance.PlayerData.EnemyShipControllers.Find(x => x.name == go.GetComponent<SpriteRenderer>().sprite.name);
		}
		
		yield return new WaitForSeconds(waves[waveId].Interval);

		if(currentWave.waveComplete != null && currentWave.RemainingItems == 0)
		{
			currentWave.waveComplete.Invoke();
		}

		waveId++;
		canSpawnWave = true;
	}

	public List<Wave> Waves
	{
		get
		{
			return waves;
		}

		set
		{
			waves = value;
		}
	}

	public List<Sprite> BrushSprites
	{
		get
		{
			return brushSprites;
		}

		set
		{
			brushSprites = value;
		}
	}
}