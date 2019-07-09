using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
	private Enemy enemy;
	private List<Transform> turrets;
	private string bulletName;

	public AttackState(Enemy enemy, List<Transform> turrets, string bulletName)
	{
		this.enemy = enemy;
		this.turrets = turrets;
		this.bulletName = bulletName;
	}

	public void Enter() { }

	public void Execute()
	{
		foreach (Transform trans in turrets)
		{
			GameObject go = PoolManager.Instance.SpawnPools["Misc"].Spawn(bulletName, trans.transform.position);
		}

		enemy.Machine.ChangeState(new MovementState(enemy));
	}

	public void Exit() {}
}
