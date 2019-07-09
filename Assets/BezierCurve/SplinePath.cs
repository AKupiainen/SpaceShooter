using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SplinePath : MonoBehaviour 
{
	public CubicBezierCurve bezierCurve;
	public float padding;
}

[Serializable]
public class CubicBezierCurve
{
	public CubicBezierCurve(int segments, Vector2 startPoint, Vector2 endPoint, Vector2 firstControlPoint, Vector2 secondControlPoint)
	{
		this.segments = segments;
		this.startPoint = startPoint;
		this.endPoint = endPoint;
		this.firstControlPoint = firstControlPoint;
		this.secondControlPoint = secondControlPoint;

		points = new Vector2[segments];
	}
		
	[SerializeField]
	private int segments;
	[SerializeField]
	private Vector2 startPoint;
	[SerializeField]
	private Vector2 endPoint;
	[SerializeField]
	private Vector2 firstControlPoint;
	[SerializeField]
	private Vector2 secondControlPoint;
	[SerializeField]
	private Vector2[] points;

	public int Segments 
	{
		get { return segments; }
		set { segments = value; }
	}

	public Vector2 StartPoint
	{
		get {return startPoint; }
		set { startPoint = value; }
	}

	public Vector2 EndPoint
	{
		get { return endPoint; }
		set { endPoint = value; }
	}

	public Vector2 FirstControlPoint
	{
		get { return firstControlPoint; }
		set { firstControlPoint = value; }
	}

	public Vector2 SecondControlPoint
	{
		get { return secondControlPoint; }
		set { secondControlPoint = value; }
	}

	public Vector2[] Points
	{
		get
		{
			Array.Resize<Vector2>(ref points, segments);

			return points;
		}

		set
		{
			points = value;
		}
	}
}

public enum PathType
{
	CubicBezierCurve = 0
}

public class MathHelper
{
	public static Vector2 GetBezierCurve(Vector2 p01, Vector2 p02, Vector2 p03, Vector2 p04, float t)
	{
		return Mathf.Pow (1 - t, 3) * p01 + 3f * Mathf.Pow (1 - t, 2) * t * p02 + 3f * Mathf.Pow (1 - t, 2) * t * p03 + Mathf.Pow (t, 3) * p04;
	}
}