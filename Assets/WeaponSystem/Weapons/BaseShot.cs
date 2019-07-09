using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

[System.Serializable]
public class BaseShot : MonoBehaviour
{
	[SerializeField]
	private bool isShooting;
	[SerializeField]
	private float delayBetweenBullets;
	[SerializeField]
	private float bulletSpeed;
	[SerializeField]
	private string poolName;
	[SerializeField]
	private string prefabName;

	public bool IsShooting
	{
		get
		{	
			return isShooting;
		}

		set
		{
			isShooting = value;
		}
	}

	public float DelayBetweenBullets
	{
		get
		{
			return delayBetweenBullets;
		}

		set
		{
			delayBetweenBullets = value;
		}
	}

	public float BulletSpeed
	{
		get
		{
			return bulletSpeed;
		}

		set
		{
			bulletSpeed = value;
		}
	}

	public string PoolName
	{
		get
		{
			return poolName;
		}

		set
		{
			poolName = value;
		}
	}

	public string PrefabName
	{
		get
		{
			return prefabName;
		}

		set
		{
			prefabName = value;
		}
	}

	public virtual void Shoot(){ }
}