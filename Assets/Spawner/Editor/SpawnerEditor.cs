using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditor.SceneManagement;
using System;


[CustomEditor(typeof(Spawner))]
public class SpawnerEditor : Editor
{
	private List<string> poolNames;
	private List<string> prefabNames;
	private int selectedPoolId;
	private int selectedPrefabId;
	private int selectedSpawnId;
	private int oldSpawnId;
	private int oldPreviewCount;
	private SpawnPool[] spawnPools;
	private List<GameObject> previewObjects;
	private int selectedPreviewObject;
	private Vector2 scrollPosition;
	private List<Texture2D> brushSpriteTextures;
	private int selectedBrushSprite;
	private Spawner data;
	private WavePattern pattern;
	private bool isOpen;

	private void OnEnable()
	{
		data = (Spawner)target;

		if(data.Waves == null || data.BrushSprites == null)
		{
			data.Waves = new List<Wave>();
			data.BrushSprites = new List<Sprite>();
		}

		selectedSpawnId = 0;
		oldSpawnId = -1;
		selectedBrushSprite = 0;
		selectedPreviewObject = 0;
		selectedPoolId = 0;
		selectedPrefabId = 0;
		scrollPosition = Vector2.zero;

		spawnPools = Resources.FindObjectsOfTypeAll(typeof(SpawnPool)) as SpawnPool[];
		poolNames = new List<string>();
		previewObjects = new List<GameObject>();
		prefabNames = new List<string>();
		brushSpriteTextures = new List<Texture2D>();

		for (int i = 0; i < spawnPools.Length; i++)
		{
			poolNames.Add(spawnPools[i].name);
		}

		foreach (Sprite spr in data.BrushSprites)
		{
			Texture2D tex = GenerateTextureFromSprite(spr);
			brushSpriteTextures.Add(tex);
		}
	}

	private void OnDisable()
	{
		DeletePreviewSprites();

		selectedSpawnId = 0;
		oldSpawnId = -1;
		selectedBrushSprite = 0;
		selectedPreviewObject = 0;
		selectedPoolId = 0;
		selectedPrefabId = 0;
		scrollPosition = Vector2.zero;

		spawnPools = null;
		poolNames = null;
		previewObjects = null;
		prefabNames = null;
		brushSpriteTextures = null;
	}

	public override void OnInspectorGUI()
	{
		DrawBasicSettings();

		GUILayout.Space(20f);

		DrawSpriteDropArea();

		GUILayout.Space(20f);

		DrawSpawnerToolbar();

		if(isOpen)
		{
			for (int i = 0; i < data.Waves.Count; i++)
			{
				data.Waves[i].DrawGUI(i, data.Waves);

				if(data.Waves[i].subMenuOpen)
				{
					SerializedProperty waves = serializedObject.FindProperty("waves");
					EditorGUILayout.PropertyField(waves.GetArrayElementAtIndex(i).FindPropertyRelative("waveComplete"));

					serializedObject.ApplyModifiedProperties();
				}
			}
		}

		GUILayout.BeginHorizontal();

		if(GUILayout.Button(!ActiveEditorTracker.sharedTracker.isLocked ? "Lock Window" : "Unlock Window"))
		{
			ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
		}

		GUILayout.EndHorizontal();

		EditorUtility.SetDirty(data);
	}

	private void DrawSpawnerToolbar()
	{
		GUILayout.BeginHorizontal(EditorStyles.toolbar);

		GUILayout.Label("Waves", GUILayout.Width((EditorGUIUtility.currentViewWidth * 0.5f)));

		if (GUILayout.Button(isOpen ? "▼" : "▶", EditorStyles.toolbarButton, GUILayout.Width((EditorGUIUtility.currentViewWidth * 0.25f))))
		{
			isOpen = !isOpen;
		}

		if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width((EditorGUIUtility.currentViewWidth * 0.25f))))
		{
			data.Waves.Add(new Wave());
		}

		GUILayout.EndHorizontal();
	}

	private void DrawBasicSettings()
	{
		if (data.Waves.Count == 0)
		{
			GUILayout.BeginHorizontal();

			EditorGUILayout.HelpBox("You need to add spawn!", MessageType.Warning);

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Add Spawn"))
			{
				data.Waves.Add(new Wave());
			}

			GUILayout.EndHorizontal();

			return;
		}

		GUILayout.Space(20f);

		GUILayout.BeginVertical("box");

		GUILayout.BeginHorizontal();
		GUILayout.Label("Basic Settings", EditorStyles.boldLabel);
		GUILayout.EndHorizontal();

		GUILayout.Space(10f);

		GUILayout.BeginHorizontal();
		GUILayout.Label("Selected spawn");
		selectedSpawnId = EditorGUILayout.IntSlider(selectedSpawnId, 0, data.Waves.Count - 1);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		pattern = (WavePattern)EditorGUILayout.EnumPopup("Pattern Type", data.Waves[selectedSpawnId].Pattern);
		GUILayout.EndHorizontal();

		data.Waves[selectedSpawnId].Pattern = pattern;

		selectedPreviewObject = Mathf.Clamp(selectedPreviewObject, 0, previewObjects.Count);

		GUILayout.BeginHorizontal();
		GUILayout.Label("Pool Name");
		selectedPoolId = EditorGUILayout.Popup(selectedPoolId, poolNames.ToArray());
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Prefabs");
		selectedPrefabId = EditorGUILayout.Popup(selectedPrefabId, GetPrefabNamesFromSpawnPool(selectedPoolId).ToArray());
		GUILayout.EndHorizontal();

		EditorGUILayout.PropertyField(serializedObject.FindProperty("complete"));
		serializedObject.ApplyModifiedProperties();

		GUILayout.Space(10f);

		GUILayout.EndVertical();
	}

	private List<string> GetPrefabNamesFromSpawnPool(int id)
	{
		List<string> prefabs = new List<string>();

		for (int i = 0; i < spawnPools[id].prefabSpawns.Count; i++)
		{
			if (spawnPools[id].prefabSpawns[i].Prefab.GetComponent<SpriteRenderer>() != null)
			{
				prefabs.Add(spawnPools[id].prefabSpawns[i].Prefab.name);
			}
		}

		return prefabs;
	}

	private void DrawSpriteDropArea()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Sprites", EditorStyles.boldLabel);
		GUILayout.EndHorizontal();

		GUILayout.BeginVertical("box");

		DrawDropAreaButtons();
		HandleSpriteDrop();

		GUILayout.EndVertical();
	}

	private void DrawDropAreaButtons()
	{
		GUILayout.BeginVertical();
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width), GUILayout.Height(200f));

		int rowCount = 4;
		int spriteCount = data.BrushSprites.Count;
		int index = 0;
		int golumnCount = spriteCount / rowCount + 1;

		if (spriteCount > 0)
		{
			for (int i = 0; i < golumnCount; i++)
			{
				GUILayout.BeginHorizontal();

				for (int j = 0; j < rowCount; j++, index++)
				{
					if (index < spriteCount)
					{
						float width = Screen.width / rowCount;
						Sprite currentSprite = data.BrushSprites[index];

						if (currentSprite)
						{
							if (index == selectedBrushSprite)
							{
								GUILayout.BeginHorizontal("Box");

								if (GUILayout.Button(brushSpriteTextures[index], GUIStyle.none, GUILayout.Width(width), GUILayout.MaxHeight(50f)))
								{
									selectedBrushSprite = index;
								}

								GUILayout.EndHorizontal();
							}

							else
							{
								if (GUILayout.Button(brushSpriteTextures[index], GUIStyle.none, GUILayout.Width(width), GUILayout.MaxHeight(50f)))
								{
									selectedBrushSprite = index;
								}
							}
						}
					}
				}

				GUILayout.EndHorizontal();
				GUILayout.Space(5f);
			}
		}

		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void HandleSpriteDrop()
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

					foreach (UnityEngine.Object obj in DragAndDrop.objectReferences)
					{
						if (obj.GetType() == typeof(Texture2D))
						{
							Texture2D tex = obj as Texture2D;

							string spriteSheet = AssetDatabase.GetAssetPath(tex);
							Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToArray();

							for (int i = 0; i < sprites.Length; i++)
							{
								Texture2D spriteTex = GenerateTextureFromSprite(sprites[i]);

								brushSpriteTextures.Add(spriteTex);
								data.BrushSprites.Add(sprites[i]);
							}
						}
					}
				}

				break;
		}

		GUILayout.BeginHorizontal();

		if (GUILayout.Button("Delete Sprite") && data.BrushSprites.Count > 0)
		{
			data.BrushSprites.RemoveAt(selectedBrushSprite);
			brushSpriteTextures.RemoveAt(selectedBrushSprite);

			if(selectedBrushSprite > 0)
			{
				selectedBrushSprite--;
			}
		}

		GUILayout.EndHorizontal();
	}

	private void OnSceneGUI()
	{
		if(Application.isPlaying || data.Waves.Count == 0)
		{
			return;
		}

		Wave wave = data.Waves[selectedSpawnId];
		int count = 0;

		if(wave.Pattern == WavePattern.Grid)
		{
			count = wave.WaveCount * wave.WaveCount;
		}

		else
		{
			count = wave.WaveCount;
		}

		if (oldSpawnId != selectedSpawnId || count != previewObjects.Count)
		{
			DeletePreviewSprites();

			for (int i = 0; i < count; i++)
			{
				if (i < wave.WaveItems.Length && data.BrushSprites.Count > 0)
				{
					if(wave.WaveItems[i].WaveSprite == null)
					{
						wave.WaveItems[i].WaveSprite = data.BrushSprites[selectedBrushSprite];
					}
	
					GameObject previewSprite = CreatePreviewSprite(wave, wave.WaveItems[i].WaveSprite);
					previewObjects.Add(previewSprite);
				}
			}

			oldSpawnId = selectedSpawnId;
		}

		float size = HandleUtility.GetHandleSize(wave.StartPoint) * 0.25f;
		Vector3 snap = Vector3.one * 0.5f;

		if (wave.Pattern == WavePattern.Line)
		{
			DrawPreviewLine(wave, size, snap);
		}

		if(wave.Pattern == WavePattern.Circle)
		{
			DrawPreviewCircle(wave, size, snap);
		}

		if(wave.Pattern == WavePattern.Grid)
		{
			DrawPreviewGrid(wave, size, snap);
		}

		if (wave.Pattern == WavePattern.Free)
		{
			DrawFreePreview(wave, size, snap);
		}

		HandleInputs(wave);
		DrawPreviewObjectOutLines(selectedPreviewObject);

		if(GUI.changed)
		{
			EditorUtility.SetDirty(target);
		}

		data.Waves[selectedSpawnId] = wave;
	}

	private GameObject CreatePreviewSprite(Wave wave, Sprite rend)
	{
		GameObject created = EditorUtility.CreateGameObjectWithHideFlags(rend.name, HideFlags.HideAndDontSave, new System.Type[] { typeof(SpriteRenderer) });
		created.GetComponent<SpriteRenderer>().sprite = rend;

		return created;
	}

	private void DrawPreviewCircle(Wave wave, float size, Vector2 snap)
	{
		wave.StartPoint = Handles.FreeMoveHandle(wave.StartPoint, Quaternion.identity, size, snap, Handles.ConeHandleCap);
		float angle = 0;

		for (int i = 0; i < wave.WaveCount; i++)
		{
			float x = wave.StartPoint.x + wave.Spacing * Mathf.Cos(angle * Mathf.Deg2Rad);
			float y = wave.StartPoint.y + wave.Spacing * Mathf.Sin(angle * Mathf.Deg2Rad);
			 
			previewObjects[i].transform.position = new Vector2(x, y);
			wave.WaveItems[i].SpawnPos = new Vector2(x, y);
			angle += 360f / previewObjects.Count;
		}
	}

	private void DrawPreviewLine(Wave wave, float size, Vector2 snap)
	{
		Handles.DrawLine(wave.StartPoint, wave.EndPoint);
		wave.StartPoint = Handles.FreeMoveHandle(wave.StartPoint, Quaternion.identity, size, snap, Handles.ConeHandleCap);
		wave.EndPoint = Handles.FreeMoveHandle(wave.EndPoint, Quaternion.identity, size, snap, Handles.ConeHandleCap);

		float yDis = Vector2.Distance(new Vector2(0, wave.StartPoint.y), new Vector2(0, wave.EndPoint.y));
		float xDis = Vector2.Distance(new Vector2(wave.StartPoint.x, 0f), new Vector2(wave.EndPoint.x, 0f));

		float y = 0;
		float x = 0;

		for (int i = 0; i < wave.WaveCount; i++)
		{
			previewObjects[i].transform.position = new Vector2(wave.StartPoint.x + x, wave.StartPoint.y + y);
			wave.WaveItems[i].SpawnPos = new Vector2(wave.StartPoint.x + x, wave.StartPoint.y + y);

			if (wave.StartPoint.x < wave.EndPoint.x)
			{
				x += xDis / (wave.WaveCount - 1);
			}

			else
			{
				x -= xDis / (wave.WaveCount - 1);
			}

			if (wave.StartPoint.y < wave.EndPoint.y)
			{
				y += yDis / (wave.WaveCount - 1);
			}

			else
			{
				y -= yDis / (wave.WaveCount - 1);
			}
		}
	}

	private void DrawPreviewGrid(Wave wave, float size, Vector2 snap)
	{
		wave.StartPoint = Handles.FreeMoveHandle(wave.StartPoint, Quaternion.identity, size, snap, Handles.ConeHandleCap);
		float ySpacing = 0;
		float gridSize = ((wave.WaveCount - 1) * wave.Spacing) / 2;
		int index = 0;

		for(int i = 0; i < wave.WaveCount; i++, ySpacing += wave.Spacing)
		{
			for (int j = 0; j < wave.WaveCount; j++, index++)
			{
				float x = (wave.StartPoint.x - gridSize) + wave.Spacing * j;
				float y = (wave.StartPoint.y - gridSize) + ySpacing;

				previewObjects[index].transform.position = new Vector2(x, y);
				wave.WaveItems[index].SpawnPos = new Vector2(x, y);
			}			
		}
	}

	private void DrawFreePreview(Wave wave, float size, Vector2 snap)
	{
		for (int i = 0; i < wave.WaveCount; i++)
		{
			wave.WaveItems[i].SpawnPos = Handles.FreeMoveHandle(wave.WaveItems[i].SpawnPos, Quaternion.identity, size, snap, Handles.ConeHandleCap);
			previewObjects[i].transform.position = wave.WaveItems[i].SpawnPos;
		}
	}

	private void DeletePreviewSprites()
	{
		if(previewObjects.Count == 0)
		{
			return;
		}
		
		foreach(GameObject go in previewObjects)
		{
			DestroyImmediate(go);
		}

		previewObjects = new List<GameObject>();
	}

	private void DrawPreviewObjectOutLines(int index)
	{
		if(previewObjects.Count == 0 || previewObjects[index] == null)
		{
			return;
		}

		SpriteRenderer rend = previewObjects[index].GetComponent<SpriteRenderer>();

		Handles.DrawLine(new Vector2(rend.transform.position.x - rend.bounds.extents.x, rend.transform.position.y + rend.bounds.extents.y), new Vector2(rend.transform.position.x + rend.bounds.extents.x, rend.transform.position.y + rend.bounds.extents.y));
		Handles.DrawLine(new Vector2(rend.transform.position.x - rend.bounds.extents.x, rend.transform.position.y - rend.bounds.extents.y), new Vector2(rend.transform.position.x + rend.bounds.extents.x, rend.transform.position.y - rend.bounds.extents.y));

		Handles.DrawLine(new Vector2(rend.transform.position.x - rend.bounds.extents.x, rend.transform.position.y + rend.bounds.extents.y), new Vector2(rend.transform.position.x - rend.bounds.extents.x, rend.transform.position.y - rend.bounds.extents.y));
		Handles.DrawLine(new Vector2(rend.transform.position.x + rend.bounds.extents.x, rend.transform.position.y + rend.bounds.extents.y), new Vector2(rend.transform.position.x + rend.bounds.extents.x, rend.transform.position.y - rend.bounds.extents.y));
	}

	private void HandleInputs(Wave wave)
	{
		Event e = Event.current;
		int controlID = GUIUtility.GetControlID(FocusType.Passive);

		if (e.GetTypeForControl(controlID) == EventType.KeyDown)
		{
			if (e.keyCode == KeyCode.Tab)
			{
				selectedPreviewObject++;

				if (selectedPreviewObject == wave.WaveItems.Length)
				{
					selectedPreviewObject = 0;
				}
			}

			if(e.keyCode == KeyCode.R)
			{
				Sprite spr = data.BrushSprites[selectedBrushSprite];
				wave.WaveItems[selectedPreviewObject].WaveSprite = spr;

				DeletePreviewSprites();

				int count = wave.Pattern != WavePattern.Grid ? wave.WaveCount : wave.WaveCount * wave.WaveCount;
				
				for (int i = 0; i < count; i++)
				{
					if (i < wave.WaveItems.Length && data.BrushSprites.Count > 0)
					{
						if (wave.WaveItems[i].WaveSprite == null)
						{
							wave.WaveItems[i].WaveSprite = data.BrushSprites[selectedBrushSprite];
						}
						
						GameObject previewSprite = CreatePreviewSprite(wave, wave.WaveItems[i].WaveSprite);
						previewObjects.Add(previewSprite);
						
						data.Waves[selectedSpawnId].WaveItems[selectedPreviewObject].PoolName = poolNames[selectedPoolId];
						data.Waves[selectedSpawnId].WaveItems[selectedPreviewObject].PrefabName = GetPrefabNamesFromSpawnPool(selectedPoolId)[selectedPrefabId];
					}
				}
			}

			Event.current.Use();
		}
	}

	private Texture2D GenerateTextureFromSprite(Sprite sprite)
	{
		Rect rect = sprite.rect;
		Texture2D tex = new Texture2D((int)rect.width, (int)rect.height);

		Color[] data = sprite.texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
		tex.SetPixels(data);
		tex.Apply(true);

		return tex;
	}
}