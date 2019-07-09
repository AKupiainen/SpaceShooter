using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineMovement : MonoBehaviour {

	public SplinePath path;
	public float speed; 
	float t;
	Vector3 previousPosition;

	void Update()
	{
		if(t < 1)
		{
			Vector3 direction = (previousPosition - transform.position).normalized;
			float angle = Vector2.Angle(direction, Vector2.up);
			transform.eulerAngles = new Vector3(0f, 0f, angle);

			previousPosition = transform.position;
			transform.position = MathHelper.GetBezierCurve(path.bezierCurve.StartPoint,
											path.bezierCurve.FirstControlPoint,
											path.bezierCurve.SecondControlPoint,
											path.bezierCurve.EndPoint, t);

			t += speed * Time.deltaTime;
		}
	}
}
