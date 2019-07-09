using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BaseShot))]
[CanEditMultipleObjects]
public class BaseWeaponSystemEditor : Editor
{
	public BaseShot shot;
	private SpawnPool[] spawnPools;

	private List<string> spawnpoolNames;
	private List<string> prefabNames;

	private int selectedSpawnPool;
	private int selectedPrefab;

	public void OnEnable()
	{
		shot = (BaseShot)target;
		prefabNames = new List<string>();
		spawnpoolNames = new List<string>();

		spawnPools = GetSpawnPools();
		prefabNames = GetPrefabNamesFromSpawnPool(selectedSpawnPool);

		selectedSpawnPool = EditorPrefs.GetInt("SelectedSpawnPool", selectedSpawnPool);
		selectedPrefab = EditorPrefs.GetInt("SelectedPrefab", selectedPrefab);
	}

	public void OnDisable()
	{
		EditorPrefs.SetInt("SelectedSpawnPool", selectedSpawnPool);
		EditorPrefs.SetInt("SelectedPrefab", selectedPrefab);
	}

	public override void OnInspectorGUI()
	{
		GUILayout.BeginVertical("box");

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Basic Settings", EditorStyles.boldLabel);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		shot.DelayBetweenBullets = EditorGUILayout.Slider("Delay:", shot.DelayBetweenBullets, 0, 100);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("SpawnPool:");
		selectedSpawnPool = EditorGUILayout.Popup(selectedSpawnPool, spawnpoolNames.ToArray());
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Prefab:");
		selectedPrefab = EditorGUILayout.Popup(selectedPrefab, prefabNames.ToArray());
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();

		shot.PrefabName = prefabNames[selectedPrefab];
		shot.PoolName = spawnpoolNames[selectedSpawnPool];

		EditorUtility.SetDirty(shot);
	}

	public SpawnPool[] GetSpawnPools()
	{
		SpawnPool[] pools = Resources.FindObjectsOfTypeAll(typeof(SpawnPool)) as SpawnPool[];

		foreach(SpawnPool pool in pools)
		{
			spawnpoolNames.Add(pool.name);
		}

		return pools;
	}

	List<string> GetPrefabNamesFromSpawnPool(int id)
	{
		List<string> prefabs = new List<string>();

		for (int i = 0; i < spawnPools[id].prefabSpawns.Count; i++)
		{
			if (spawnPools[id].prefabSpawns[i].Prefab.GetComponent<SpriteRenderer>() != null)
			{
				prefabs.Add(spawnPools[id].prefabSpawns[i].Name);
			}
		}

		return prefabs;
	}
}

[CanEditMultipleObjects]
[CustomEditor(typeof(LinearShot))]
public class LinearShotEditor : BaseWeaponSystemEditor
{
	private LinearShot linearShot;

	public new void OnEnable()
	{
		base.OnEnable();
		linearShot = (LinearShot)target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.BeginVertical("box");

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Advanced Settings", EditorStyles.boldLabel);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		linearShot.Angle = EditorGUILayout.Slider("Angle:", linearShot.Angle, 0f, 360f);
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();

		EditorUtility.SetDirty(linearShot);
	}
}

[CanEditMultipleObjects]
[CustomEditor(typeof(SpreadShot), true)]
public class SpreadShotEditor : BaseWeaponSystemEditor
{
	private SpreadShot spreadShot;

	private new void OnEnable()
	{
		base.OnEnable();
		spreadShot = (SpreadShot)target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.BeginVertical("box");

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Advanced Settings", EditorStyles.boldLabel);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		spreadShot.bulletCount = EditorGUILayout.IntSlider("Bullet Count:", spreadShot.bulletCount, 0, 100);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		spreadShot.startingAngle = EditorGUILayout.Slider("Starting Angle:", spreadShot.startingAngle, -360f, 360f);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		spreadShot.endAngle = EditorGUILayout.Slider("End Angle:", spreadShot.endAngle, 0, 360f);
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();

		EditorUtility.SetDirty(spreadShot);
	}
}

[CanEditMultipleObjects]
[CustomEditor(typeof(SpiralShot), true)]
public class SpriralShotEditor : BaseWeaponSystemEditor
{
	private SpiralShot spiralShot;

	private new void OnEnable()
	{
		base.OnEnable();
		spiralShot = (SpiralShot)target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.BeginVertical("box");

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Advanced Settings", EditorStyles.boldLabel);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		spiralShot.SpiralPointCount = EditorGUILayout.IntSlider("Spiral Point Count:", spiralShot.SpiralPointCount, 0, 20);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		spiralShot.RotateTime = EditorGUILayout.Slider("Rotate Time:", spiralShot.RotateTime, 0f, 20f);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		spiralShot.TimeBetweenShots = EditorGUILayout.Slider("Time Between Shots:", spiralShot.TimeBetweenShots, 0f, 20f);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		spiralShot.RotateSpeed = EditorGUILayout.Slider("Rotate Speed:", spiralShot.RotateSpeed, 0f, 720f);
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();

		EditorUtility.SetDirty(spiralShot);
	}
}