using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : IState
{
	private Enemy enemy;

	public MovementState(Enemy enemy)
	{
		this.enemy = enemy;
	}

	public void Enter() { }

	public void Execute()
	{
		enemy.transform.Translate(Vector3.down * enemy.Speed * Time.deltaTime);
	}

	public void Exit() { }
}
