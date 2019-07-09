using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.Events;

[System.Serializable]
public class Wave
{
	[SerializeField]
	public int waveCount = 1;
	[SerializeField]
	private float spacing;
	[SerializeField]
	private float interval;
	[SerializeField]
	private Vector2 startPoint;
	[SerializeField]
	private Vector2 endPoint;
	[SerializeField]
	private WaveItem[] waveItems;
	[SerializeField]
	private WavePattern pattern;
	[SerializeField]
	public UnityEvent waveComplete;
	private int remainingItems;

#if UNITY_EDITOR
	public bool subMenuOpen;
#endif

	public void DrawGUI(int index, List<Wave> waves)
	{
		GUILayout.BeginVertical();

		GUILayout.BeginHorizontal(EditorStyles.toolbar);

		subMenuOpen = GUILayout.Toggle(subMenuOpen, "Options", EditorStyles.toolbarButton, GUILayout.Width((EditorGUIUtility.currentViewWidth * 0.2f)));

		if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width((EditorGUIUtility.currentViewWidth * 0.2f))))
		{
			waves.Add(new Wave());
		}

		if (GUILayout.Button("-", EditorStyles.toolbarButton, GUILayout.Width((EditorGUIUtility.currentViewWidth * 0.2f))))
		{
			waves.RemoveAt(index);
		}

		if (GUILayout.Button("▼", EditorStyles.toolbarButton, GUILayout.Width((EditorGUIUtility.currentViewWidth * 0.2f))) && index < waves.Count - 1)
		{
			SwapItems(index, index + 1, waves);
		}

		if (GUILayout.Button("▲", EditorStyles.toolbarButton, GUILayout.Width((EditorGUIUtility.currentViewWidth * 0.2f))) && index > 0)
		{
			SwapItems(index, index - 1, waves);
		}
	
		GUILayout.EndHorizontal();

		if (subMenuOpen)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Wave Count");
			WaveCount = EditorGUILayout.IntSlider(WaveCount, 1, 100);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Time Between Spawns");
			interval = EditorGUILayout.Slider(interval, 1f, 100f);
			GUILayout.EndHorizontal();

			if (pattern != WavePattern.Line)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label("Spacing");
				spacing = EditorGUILayout.Slider(spacing, 1, 100);
				GUILayout.EndHorizontal();
			}
		}

		GUILayout.EndVertical();
		SceneView.RepaintAll();
	}

	public void SwapItems(int oldIndex, int newIndex, List<Wave> waves)
	{
		Wave tempWave = waves[oldIndex];
		waves[oldIndex] = waves[newIndex];
		waves[newIndex] = tempWave;
	}

	public int WaveCount
	{
		get
		{
			return waveCount;
		}

		set
		{
			waveCount = value;
		}
	}

	public float Spacing
	{
		get
		{
			return spacing;
		}

		set
		{
			spacing = value;
		}
	}

	public float Interval
	{
		get
		{
			return interval;
		}

		set
		{
			interval = value;
		}
	}

	public Vector2 StartPoint
	{
		get
		{
			return startPoint;
		}

		set
		{
			startPoint = value;
		}
	}

	public Vector2 EndPoint
	{
		get
		{
			return endPoint;
		}

		set
		{
			endPoint = value;
		}
	}

	public WaveItem[] WaveItems
	{
		get
		{
			if(waveItems == null)
			{
				waveItems = new WaveItem[waveCount];
				waveItems[0] = new WaveItem();
			}

			if (Pattern != WavePattern.Grid)
			{
				Array.Resize<WaveItem>(ref waveItems, waveCount);
			}

			else
			{
				Array.Resize<WaveItem>(ref waveItems, waveCount * WaveCount);
			}

			for (int i = 0; i < waveItems.Length; i++)
			{
				if (waveItems[i] == null)
				{
					WaveItem item = new WaveItem();
					waveItems[i] = item;
				}
			}
			
			return waveItems;
		}

		set
		{
			waveItems = value;
		}
	}

	public WavePattern Pattern
	{
		get
		{
			return pattern;
		}

		set
		{
			pattern = value;
		}
	}

	public UnityEvent WaveComplete
	{
		get
		{
			return waveComplete;
		}

		set
		{
			waveComplete = value;
		}
	}

	public int RemainingItems
	{
		get
		{
			return remainingItems;
		}

		set
		{
			remainingItems = value;
		}
	}
}