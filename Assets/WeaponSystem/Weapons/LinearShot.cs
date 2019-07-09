using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class LinearShot : BaseShot
{
	[SerializeField]
	private float angle;

	private void Start()
	{
		IsShooting = true;
	}

	public void Shot()
	{
		if(IsShooting)
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

		PoolManager.Instance.SpawnPools[PoolName].Spawn(PrefabName, transform.position, new Vector3(transform.rotation.x, transform.rotation.y, -angle + transform.eulerAngles.z));
		yield return new WaitForSeconds(DelayBetweenBullets);

		IsShooting = true;
	}

	void OnDrawGizmosSelected()
	{
		float length = 5f;
		Vector2 dir = new Vector2(Mathf.Sin((Angle - transform.eulerAngles.z) * Mathf.Deg2Rad), Mathf.Cos((Angle - transform.eulerAngles.z) * Mathf.Deg2Rad)).normalized;

		Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + (dir * length));
	}

	public float Angle
	{
		get
		{
			return angle;
		}

		set
		{
			angle = value;
		}
	}
}
