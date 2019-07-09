using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections;

public class SpawnPool : MonoBehaviour
{
	// Prefabs which are used to load objects to the pool
	public List<PoolItem> prefabSpawns;
	// Pool objects
	public Dictionary<string, List<GameObject>> activeSpawns;

	public void Awake()
	{
		AddSpawnsToPool();
	}

	/// <summary>
	/// Loads all the prefabs to the memory
	/// </summary>
	private void AddSpawnsToPool()
	{
		activeSpawns = new Dictionary<string, List<GameObject>>();

		try
		{
			foreach (PoolItem item in prefabSpawns)
			{
				// Loads prefabs one by one, and instantiates them to the pool which is done by preload amount
				List<GameObject> tmpSpawns = new List<GameObject>();
				GameObject parent = new GameObject();
				parent.gameObject.name = item.Name;

				for (int i = 0; i < item.PreloadAmount; i++)
				{
					GameObject go = Instantiate(item.Prefab, Vector3.zero, Quaternion.identity) as GameObject;
					go.gameObject.SetActive(false);
					go.name = item.Name + "" + i.ToString();
					go.transform.parent = parent.transform;
					item.Parent = parent.transform;

					tmpSpawns.Add(go);
				}

				activeSpawns.Add(item.Name, tmpSpawns);
			}
		}

		// Catch the exception
		catch (Exception ex)
		{
			Debug.Log("There was error loading items to the pool " + ex.ToString());
		}
	}

	/// <summary>
	/// Spawns new gameobject from the pool using name
	/// </summary>
	/// <param name="poolName"></param>
	public GameObject Spawn(string poolName)
	{
		GameObject result = null;

		foreach (KeyValuePair<string, List<GameObject>> list in activeSpawns)
		{
			if (list.Key == poolName)
			{
				result = list.Value.First(x => !x.gameObject.activeSelf);
			}
		}

		if (result != null)
		{
			result.gameObject.SetActive(true);
			result.transform.parent = null;
		}

		return result;
	}

	/// <summary>
	/// Spawns new gameobject from the pool using name
	/// </summary>
	/// <param name="poolName"></param>
	/// <param name="spawnPosition"></param>
	/// <returns></returns>
	public GameObject Spawn(string poolName, Vector3 spawnPosition)
	{
		GameObject result = Spawn(poolName);

		if (result != null)
		{
			result.gameObject.SetActive(true);
			result.gameObject.transform.position = spawnPosition;
			result.transform.parent = null;
		}

		return result;
	}

	/// <summary>
	/// Spawns new gameobject from the pool using name
	/// </summary>
	/// <param name="poolName"></param>
	/// <param name="spawnPosition"></param>
	/// <param name="spawnRotation"></param>
	/// <returns></returns>
	public GameObject Spawn(string poolName, Vector3 spawnPosition, Vector3 spawnRotation)
	{
		GameObject result = Spawn(poolName);

		if (result != null)
		{
			result.gameObject.SetActive(true);
			result.gameObject.transform.position = spawnPosition;
			result.transform.parent = null;
			result.transform.eulerAngles = spawnRotation;
		}

		return result;
	}

	/// <summary>
	/// Despawns object using Gameobject
	/// </summary>
	/// <param name="go"></param>
	public void Despawn(GameObject go)
	{
		foreach (KeyValuePair<string, List<GameObject>> list in activeSpawns)
		{
			if (list.Value.Contains(go))
			{
				Regex regex = new Regex("[^a-z]", RegexOptions.IgnoreCase);
				string prefabName = regex.Replace(go.gameObject.name, @"");

				PoolItem item = prefabSpawns.Find(x => x.Prefab.name == prefabName);

				if (item.Reparent)
				{
					go.transform.parent = item.Parent;
				}

				go.gameObject.SetActive(false);
			}
		}
	}

	/// <summary>
	/// Despawns all objects from the pool
	/// </summary>
	/// <param name="spawnPoolName"></param>
	public void DespawnAll(string spawnPoolName)
	{
		foreach (KeyValuePair<string, List<GameObject>> list in activeSpawns)
		{
			if (list.Key.Contains(spawnPoolName))
			{
				for (int i = 0; i < list.Value.Count; i++)
				{
					Regex regex = new Regex("[^a-z]", RegexOptions.IgnoreCase);
					string prefabName = regex.Replace(list.Value[i].gameObject.name, @"");
					PoolItem item = prefabSpawns.Find(x => x.Prefab.name == prefabName);

					if (item.Reparent)
					{
						list.Value[i].gameObject.transform.parent = item.Parent;
					}

					list.Value[i].gameObject.SetActive(false);
				}

				break;
			}
		}
	}

	/// <summary>
	/// Deletes all spawnpools
	/// </summary>
	/// <param name="name"></param>
	public void DeleteSpawnPool(string name)
	{
		foreach (KeyValuePair<string, List<GameObject>> list in activeSpawns)
		{
			if (list.Key.Contains(name))
			{
				for (int i = 0; i < list.Value.Count; i++)
				{
					Destroy(list.Value[i].gameObject);
				}

				break;
			}
		}

		activeSpawns.Remove(name);
	}

	/// <summary>
	/// Spawns object in delay
	/// </summary>
	/// <param name="poolname"></param>
	/// <param name="t"></param>
	public void DelayedSpawn(string poolname, float t)
	{
		StartCoroutine(StartDelayedSpawn(poolname, t));
	}

	/// <summary>
	/// Spawns object in delay with position
	/// </summary>
	/// <param name="poolname"></param>
	/// <param name="t"></param>
	/// <param name="spawnPosition"></param>
	public void DelayedSpawn(string poolname, float t, Vector3 spawnPosition)
	{
		StartCoroutine(StartDelayedSpawn(poolname, t, spawnPosition));
	}

	/// <summary>
	/// Spawns object in delay with position and rotation
	/// </summary>
	/// <param name="poolname"></param>
	/// <param name="t"></param>
	/// <param name="spawnPosition"></param>
	/// <param name="spawnRotation"></param>
	public void DelayedSpawn(string poolname, float t, Vector3 spawnPosition, Vector3 spawnRotation)
	{
		StartCoroutine(StartDelayedSpawn(poolname, t, spawnPosition, spawnRotation));
	}

	/// <summary>
	/// IEnumerator for DelayedSpawn
	/// </summary>
	/// <param name="name"></param>
	/// <param name="t"></param>
	/// <returns></returns>
	IEnumerator StartDelayedSpawn(string name, float t)
	{
		yield return new WaitForSeconds(t);
		Spawn(name);
	}

	/// <summary>
	/// IEnumerator for DelayedSpawn
	/// </summary>
	/// <param name="name"></param>
	/// <param name="t"></param>
	/// <returns></returns>
	IEnumerator StartDelayedSpawn(string name, float t, Vector3 spawnPosition)
	{
		yield return new WaitForSeconds(t);
		Spawn(name, spawnPosition);
	}

	/// <summary>
	/// IEnumerator for DelayedSpawn
	/// </summary>
	/// <param name="name"></param>
	/// <param name="t"></param>
	/// <returns></returns>
	IEnumerator StartDelayedSpawn(string name, float t, Vector3 spawnPosition, Vector3 spawnRotation)
	{
		yield return new WaitForSeconds(t);
		Spawn(name, spawnPosition, spawnRotation);
	}
}

[Serializable]
public class PoolItem
{
	[SerializeField]
	private string name;
	[SerializeField]
	private GameObject prefab;
	[SerializeField]
	private bool reparent;
	[SerializeField]
	private int preloadAmount;
	private Transform parent;

#if UNITY_EDITOR
	private bool subMenuOpen;
#endif
	public PoolItem(string name, int preloadAmount, GameObject prefab, bool reparent)
	{
		this.Name = name;
		this.PreloadAmount = preloadAmount;
		this.Prefab = prefab;
		this.Reparent = reparent;
	}

	public string Name
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

	public GameObject Prefab
	{
		get
		{
			return prefab;
		}

		set
		{
			prefab = value;
		}
	}

	public int PreloadAmount
	{
		get
		{
			return preloadAmount;
		}

		set
		{
			preloadAmount = value;
		}
	}

	public bool Reparent
	{
		get
		{
			return reparent;
		}

		set
		{
			reparent = value;
		}
	}

	public bool SubMenuOpen
	{
		get
		{
			return subMenuOpen;
		}

		set
		{
			subMenuOpen = value;
		}
	}

	public Transform Parent
	{
		get
		{
			return parent;
		}

		set
		{
			parent = value;
		}
	}
}