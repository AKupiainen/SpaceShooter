using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class SpreadShot : BaseShot
{
	[SerializeField]
	public float endAngle;
	[SerializeField]
	public float startingAngle;
	[SerializeField]
	public int bulletCount;

	private void Start()
	{
		base.IsShooting = true;
	}

	public void Shot()
	{
		if (base.IsShooting)
		{
			StartCoroutine(ShotCoroutine());
		}
	}

	private void Update()
	{
		Shot();
	}

	private IEnumerator ShotCoroutine()
	{
		base.IsShooting = false;
		float angle = startingAngle;

		for (int i = 0; i <= bulletCount ; i++, angle += (Mathf.Abs(startingAngle - endAngle)) / bulletCount)
		{
			Vector2 dir = new Vector2(Mathf.Sin((angle - transform.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Cos((angle - transform.eulerAngles.z ) * Mathf.Deg2Rad)).normalized;
			float newAngle = -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

			PoolManager.Instance.SpawnPools[base.PoolName].Spawn(base.PrefabName, transform.position, new Vector3(0, 0, newAngle));
		}

		yield return new WaitForSeconds(base.DelayBetweenBullets);

		base.IsShooting = true;
	}

	void OnDrawGizmosSelected()
	{
		float length = 5f;
		float angle = startingAngle;

		Vector2 startDir = new Vector2(Mathf.Sin((startingAngle - transform.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Cos((startingAngle - transform.eulerAngles.z) * Mathf.Deg2Rad)).normalized;
		Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + (startDir * length));

		Vector2 endDir = new Vector2(Mathf.Sin((endAngle - transform.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Cos((endAngle - transform.eulerAngles.z) * Mathf.Deg2Rad)).normalized;
		Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + (endDir * length));

		for(int i = 0; i < bulletCount; i++, angle += (Mathf.Abs(startingAngle - endAngle)) / bulletCount)
		{
			Vector2 dir = new Vector2(Mathf.Sin((angle - transform.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Cos((angle - transform.eulerAngles.z) * Mathf.Deg2Rad)).normalized;
			Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + (dir * length));
		}
	}
}