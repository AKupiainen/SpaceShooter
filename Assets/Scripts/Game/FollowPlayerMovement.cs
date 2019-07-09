using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerMovement : MonoBehaviour
{
	public Vector3 centerPoint;
	public float radius;
	private Vector3 velocity;
	public float smoothtime;

	public void Start()
	{
		centerPoint = transform.position;
	}

	void Update()
	{
		Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		Vector3 newPos = transform.position + movement;

		Vector3 offset = newPos - centerPoint;
		transform.position = Vector3.SmoothDamp(transform.position, centerPoint + Vector3.ClampMagnitude(offset, radius), ref velocity, smoothtime);
	}
}
