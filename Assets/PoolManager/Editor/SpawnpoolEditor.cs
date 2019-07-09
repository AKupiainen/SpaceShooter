using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpawnPool))]
public class SpawnPoolEditor : UnityEditor.Editor
{
	private SpawnPool spawnPool;
	private int selectedSpawnItem;
	private bool isOpen;

	private void OnEnable()
	{
		spawnPool = (SpawnPool)target;

		if (spawnPool.prefabSpawns == null)
		{
			spawnPool.prefabSpawns = new List<PoolItem>();
		}
	}

	public void OnDisable()
	{
		spawnPool = null;
	}

	[MenuItem("ObjectyPool/Add SpawnPool")]
	static void AddPoolManager()
	{
		GameObject go = new GameObject("SpawnPool");
		go.AddComponent(typeof(SpawnPool));
	}

	public override void OnInspectorGUI()
	{
		GUILayout.Space(20f);

		GUILayout.BeginVertical();

		Toolbar();

		if (isOpen)
		{
			PoolOptions();
		}

		GUILayout.Space(20f);

		GUILayout.BeginHorizontal();

		GUILayout.Label("Drag 'n Drop", EditorStyles.boldLabel);

		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();

		GUILayout.Box("Drag GameObjects here", GUILayout.MinHeight(100f), GUILayout.MinWidth(Screen.width));

		HandleGameObjectDrop();

		GUILayout.EndHorizontal();

		GUILayout.EndVertical();
	}

	public void SwapItems(int oldIndex, int newIndex)
	{
		PoolItem tmp = spawnPool.prefabSpawns[oldIndex];
		spawnPool.prefabSpawns[oldIndex] = spawnPool.prefabSpawns[newIndex];
		spawnPool.prefabSpawns[newIndex] = tmp;
	}

	private void Toolbar()
	{
		GUILayout.BeginHorizontal(EditorStyles.toolbar);

		GUILayout.Label("Pool items", GUILayout.Width((EditorGUIUtility.currentViewWidth * 0.5f)));

		if (GUILayout.Button(isOpen ? "▼" : "▶", EditorStyles.toolbarButton, GUILayout.Width((EditorGUIUtility.currentViewWidth * 0.25f))))
		{
			isOpen = !isOpen;
		}

		if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width((EditorGUIUtility.currentViewWidth * 0.25f))))
		{
			spawnPool.prefabSpawns.Add(new PoolItem("", 0, null, false));
		}

		GUILayout.EndHorizontal();
	}

	private void PoolOptions()
	{
		if (spawnPool.prefabSpawns != null && spawnPool.prefabSpawns.Count > 0)
		{
			for (int i = 0; i < spawnPool.prefabSpawns.Count; i++)
			{
				GUILayout.BeginHorizontal(EditorStyles.toolbar);

				spawnPool.prefabSpawns[i].SubMenuOpen = GUILayout.Toggle(spawnPool.prefabSpawns[i].SubMenuOpen, "Options ", EditorStyles.toolbarButton, GUILayout.Width((EditorGUIUtility.currentViewWidth * 0.2f)));

				if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width((EditorGUIUtility.currentViewWidth * 0.2f))))
				{
					spawnPool.prefabSpawns.Add(new PoolItem("", 0, null, false));
				}

				if (GUILayout.Button("-", EditorStyles.toolbarButton, GUILayout.Width((EditorGUIUtility.currentViewWidth * 0.2f))))
				{
					spawnPool.prefabSpawns.RemoveAt(i);
					break;
				}

				if (GUILayout.Button("▼", EditorStyles.toolbarButton, GUILayout.Width((EditorGUIUtility.currentViewWidth * 0.2f))) && i < spawnPool.prefabSpawns.Count - 1)
				{
					SwapItems(i, i + 1);
					break;
				}

				if (GUILayout.Button("▲", EditorStyles.toolbarButton, GUILayout.Width((EditorGUIUtility.currentViewWidth * 0.2f))) && i > 0)
				{
					SwapItems(i, i - 1);
					break;
				}

				GUILayout.EndHorizontal();

				if (spawnPool.prefabSpawns[i].SubMenuOpen)
				{
					GUILayout.BeginHorizontal();

					GUILayout.Label("Name of the Spawn ");
					spawnPool.prefabSpawns[i].Name = GUILayout.TextField(spawnPool.prefabSpawns[i].Name);

					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();

					GUILayout.Label("Prefab ");
					spawnPool.prefabSpawns[i].Prefab = (GameObject)EditorGUILayout.ObjectField(spawnPool.prefabSpawns[i].Prefab, typeof(GameObject), true);

					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();

					GUILayout.Label("Prelod amount ");
					spawnPool.prefabSpawns[i].PreloadAmount = EditorGUILayout.IntSlider(spawnPool.prefabSpawns[i].PreloadAmount, 0, 1000);

					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();

					GUILayout.Label("Reparent ");
					spawnPool.prefabSpawns[i].Reparent = EditorGUILayout.Toggle(spawnPool.prefabSpawns[i].Reparent);

					GUILayout.EndHorizontal();
				}
			}
		}
	}

	private void HandleGameObjectDrop()
	{
		Event e = Event.current;
		Rect dropArea = GUILayoutUtility.GetLastRect();

		switch (e.type)
		{
			case EventType.DragUpdated:
			case EventType.DragPerform:

				if (!dropArea.Contains(e.mousePosition))
				{
					return;
				}

				DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

				if (e.type == EventType.DragPerform)
				{
					DragAndDrop.AcceptDrag();

					foreach (Object obj in DragAndDrop.objectReferences)
					{
						if (obj.GetType() == typeof(GameObject))
						{
							GameObject go = (GameObject)obj;
							spawnPool.prefabSpawns.Add(new PoolItem("", 0, go, false));
						}
					}
				}

				break;
		}
	}
}