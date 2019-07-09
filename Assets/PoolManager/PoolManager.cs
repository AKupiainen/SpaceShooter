using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : GenericSingleton<PoolManager>
{
    private Dictionary<string, SpawnPool> spawnPools;

    void Awake()
    {
		// Find all pools from scene and add them to Dictionary
        SpawnPools = new Dictionary<string, SpawnPool>();
        SpawnPool[] tmpSpawnPools = GameObject.FindObjectsOfType<SpawnPool>();

		if (tmpSpawnPools.Length > 0)
		{
			for (int i = 0; i < tmpSpawnPools.Length; i++)
			{
				spawnPools.Add(tmpSpawnPools[i].name, tmpSpawnPools[i]);
			}
		}

		else
		{
			Debug.Log("Couldn´t add spawn to the Dictionary");
		}
    }

    public Dictionary<string, SpawnPool> SpawnPools
    {
        get
        {
            return spawnPools;
        }

        set
        {
            spawnPools = value;
        }
    }
}