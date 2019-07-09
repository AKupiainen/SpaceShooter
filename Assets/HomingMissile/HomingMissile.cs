using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
	public Transform target;
	public Transform missileSprite;
	private Vector2 currentVelocity;

	[Range(0, 10f)]
	public float kProportionalConst;
	[Range(0, 100f)]
	public float maxSpeed;
	[Range(0, 720f)]
	public float rotationSpeed;
	
	void Update ()
	{
		Vector2 dir = target.transform.position - transform.position;
		dir.Normalize();

		Vector2 desiredVelocity = dir * maxSpeed;
		Vector2 error = desiredVelocity - currentVelocity;
		Vector2 sForce = error * kProportionalConst;

		currentVelocity += sForce * Time.deltaTime;
		transform.Translate(new Vector3(currentVelocity.x, currentVelocity.y));

		float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * 180f / Mathf.PI;
		missileSprite.rotation = Quaternion.Slerp(missileSprite.rotation, Quaternion.Euler(0f, 0f, angle - 90), rotationSpeed * Time.deltaTime);
	}
}
