using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SplinePath))]
public class SplinePathEditor : Editor
{
	private SplinePath path;
	private bool isEditingCurve;

	[MenuItem("Hamza Tools/CreateSpline/Create New Spline")]
	static void CreateSpline()
	{
		GameObject go = new GameObject();
		go.AddComponent(typeof(SplinePath));
	}

	void OnEnable()
	{
		path = (SplinePath)target;
	}

	public override void OnInspectorGUI()
	{
		GUILayout.BeginVertical();

		GUILayout.BeginHorizontal();

		isEditingCurve = EditorGUILayout.Toggle("Editing curve", isEditingCurve);

		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();

		path.padding = EditorGUILayout.Slider("Padding", path.padding, 5f, 100f);

		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		path.bezierCurve.Segments = EditorGUILayout.IntSlider("Segments", path.bezierCurve.Segments, 0, 100);
		GUILayout.EndHorizontal();

		GUILayout.Space(20f);
		GUILayout.EndVertical();

		EditorUtility.SetDirty(target);
	}

	void OnSceneGUI()
	{
		// size and snap handles
		float size = HandleUtility.GetHandleSize(path.bezierCurve.StartPoint) * 0.25f;
		Vector3 snap = Vector3.one * 0.5f;

		Vector3 oldPoint = Vector3.zero;

		// lines between controlspoints
		Handles.color = Color.green;
		Handles.DrawLine(path.bezierCurve.StartPoint, path.bezierCurve.FirstControlPoint);
		Handles.DrawLine(path.bezierCurve.EndPoint, path.bezierCurve.SecondControlPoint);

		// Handles for changing positions
		Vector3 newTargetPosition = Handles.FreeMoveHandle(path.bezierCurve.StartPoint, Quaternion.identity, size, snap, Handles.ConeHandleCap);
		path.bezierCurve.StartPoint = newTargetPosition;

		newTargetPosition = Handles.FreeMoveHandle(path.bezierCurve.EndPoint, Quaternion.identity, size, snap, Handles.ConeHandleCap);
		path.bezierCurve.EndPoint = newTargetPosition;

		newTargetPosition = Handles.FreeMoveHandle(path.bezierCurve.FirstControlPoint, Quaternion.identity, size, snap, Handles.ConeHandleCap);
		path.bezierCurve.FirstControlPoint = newTargetPosition;

		newTargetPosition = Handles.FreeMoveHandle(path.bezierCurve.SecondControlPoint, Quaternion.identity, size, snap, Handles.ConeHandleCap);
		path.bezierCurve.SecondControlPoint = newTargetPosition;

		Handles.color = Color.blue;

		// Draw the bezier curve
		for (int j = 0; j < path.bezierCurve.Segments; j++)
		{

			float t = (float) j / path.bezierCurve.Segments;
			Vector3 bezierPoint = MathHelper.GetBezierCurve(path.bezierCurve.StartPoint,
										path.bezierCurve.FirstControlPoint,
										path.bezierCurve.SecondControlPoint,
										path.bezierCurve.EndPoint, t);


			path.bezierCurve.Points[j] = bezierPoint;

			if (j == 0)
			{
				Handles.DrawLine(path.bezierCurve.StartPoint, bezierPoint);
			}

			else if (j == path.bezierCurve.Segments - 1)
			{ 
				Handles.DrawLine(oldPoint, path.bezierCurve.EndPoint);
			}

			else
			{
				Handles.DrawLine(oldPoint, bezierPoint);
			}

			oldPoint = bezierPoint;
		}
	}
}