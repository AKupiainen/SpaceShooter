using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : Enemy
{
	public List<Transform> turrets;
	public string bulletName;
	public float movementTime;

	private float currentTime;

	void Start ()
	{
		Machine = new StateMachine();
		Machine.ChangeState(new MovementState(this));
	}
	
	void Update ()
	{
		currentTime += Time.deltaTime;

		if(currentTime >= movementTime)
		{
			currentTime = 0;
			Machine.ChangeState(new AttackState(this, turrets, bulletName));
		}

		Machine.UpdateState();
	}

	public override void OnDeath()
	{
		base.OnDeath();
	}
}
