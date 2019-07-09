using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralShot : BaseShot
{
	private void Start()
	{
		IsShooting = true;
	}

	public void Shot()
	{
		if (IsShooting)
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
		IsShooting = false;

		float time = 0;
		float rotateAngle = 0;
		float angle = 0;

		while(time < rotateTime)
		{
			if(rotateAngle >= 360)
			{
				rotateAngle = 0;
			}

			for(int i = 0; i < spiralPointCount; i++, angle += 360 / spiralPointCount)
			{
				Vector2 dir = new Vector2(Mathf.Sin((angle - transform.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Cos((angle - transform.eulerAngles.z) * Mathf.Deg2Rad)).normalized;
				float newAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

				PoolManager.Instance.SpawnPools[PoolName].Spawn(PrefabName, transform.position, new Vector3(transform.rotation.x, transform.rotation.y, newAngle + rotateAngle + transform.eulerAngles.z));
			}

			rotateAngle += Time.deltaTime * rotateSpeed;
			time += Time.deltaTime;

			yield return new WaitForSeconds(timeBetweenShots);
		}


		yield return new WaitForSeconds(DelayBetweenBullets);
		IsShooting = true;
	}

	void OnDrawGizmosSelected()
	{
		float angle = 0f;
		float length = 5f;

		for (int i = 0; i < spiralPointCount; i++, angle += 360f / spiralPointCount)
		{
			Vector2 dir = new Vector2(Mathf.Sin((angle - transform.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Cos((angle - transform.eulerAngles.z) * Mathf.Deg2Rad)).normalized;
			Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + (dir * length));
		}
	}

	[SerializeField]
	private int spiralPointCount;
	[SerializeField]
	private float rotateTime;
	[SerializeField]
	private float rotateSpeed;
	[SerializeField]
	private float timeBetweenShots;

	public int SpiralPointCount
	{
		get
		{
			return spiralPointCount;
		}

		set
		{
			spiralPointCount = value;
		}
	}

	public float RotateTime
	{
		get
		{
			return rotateTime;
		}

		set
		{
			rotateTime = value;
		}
	}

	public float RotateSpeed
	{
		get
		{
			return rotateSpeed;
		}

		set
		{
			rotateSpeed = value;
		}
	}

	public float TimeBetweenShots
	{
		get
		{
			return timeBetweenShots;
		}

		set
		{
			timeBetweenShots = value;
		}
	}
}