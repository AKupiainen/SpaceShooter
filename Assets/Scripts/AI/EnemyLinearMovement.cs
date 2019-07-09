using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyLinearMovement : Enemy
{
	public override void Awake()
	{
		base.Awake();

		Machine = new StateMachine();
		Machine.ChangeState(new MovementState(this));
	}

	void Update()
	{
		Machine.UpdateState();

		Vector3 position = Camera.main.WorldToViewportPoint(transform.position);

		if(position.y < -0.5f || Health <= 0)
		{
			OnDeath();
		}
	}

	public override void OnDeath()
	{
		base.OnDeath();
	}
}