using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PickUpManager))]
public class PickUpManagerEditor : Editor
{
	private PickUpManager manager;
	private int selectedPickUp;

	private void OnEnable()
	{
		manager = (PickUpManager)target;

		selectedPickUp = EditorPrefs.GetInt("selectedPickUp");
	}

	private void OnDisable()
	{
		EditorPrefs.SetInt("selectedPickUp", selectedPickUp);
	}

	public override void OnInspectorGUI()
	{
		GUILayout.BeginVertical();

		GUILayout.BeginHorizontal();

		GUILayout.Label("PickUp Spawns");

		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();

		selectedPickUp = EditorGUILayout.IntSlider("Selected PickUp", selectedPickUp, 0, manager.PickUps.Count - 1);

		GUILayout.EndHorizontal();

		for(int i = 0; i < manager.PickUps.Count; i++)
		{
			GUILayout.BeginHorizontal();

			GUILayout.Label((i + 1).ToString() + ".");
			manager.PickUps[i].PickUpType = (PickUpType)EditorGUILayout.EnumPopup(manager.PickUps[i].PickUpType);

			GUILayout.EndHorizontal();
		}

		GUILayout.BeginHorizontal();

		if(GUILayout.Button("Add new pickup"))
		{
			manager.PickUps.Add(new PickUpItem());
		}

		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();

		if (GUILayout.Button("Delete PickUp"))
		{
			manager.PickUps.RemoveAt(manager.PickUps.Count - 1);
		}

		GUILayout.EndHorizontal();

		GUILayout.EndVertical();

		if(GUI.changed)
		{
			EditorUtility.SetDirty(manager);
		}
	}

	private void OnSceneGUI()
	{
		if(manager.PickUps[selectedPickUp] == null)
		{
			return;
		}

		PickUpItem item = manager.PickUps[selectedPickUp];
		float size = HandleUtility.GetHandleSize(item.SpawnPos) * 0.25f;
		Vector3 snap = Vector3.one * 0.5f;

		item.SpawnPos = Handles.FreeMoveHandle(item.SpawnPos, Quaternion.identity, size, snap, Handles.ConeHandleCap);

		EditorUtility.SetDirty(manager);
	}
}