using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
	private PickUpType type;
	[SerializeField]
	private float speed;

	private void Update()
	{
		transform.Translate(Vector2.down * speed * Time.deltaTime);

		Vector3 position = Camera.main.WorldToViewportPoint(transform.position);

		if (position.y < 0f)
		{
			PoolManager.Instance.SpawnPools["Misc"].Despawn(gameObject);
		}

		if (position.x > 1f)
		{
			PoolManager.Instance.SpawnPools["Misc"].Despawn(gameObject);
		}

		if (position.x < 0f)
		{
			PoolManager.Instance.SpawnPools["Misc"].Despawn(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			if (type == PickUpType.ExtraLife)
			{
				PlayerManager.Instance.PlayerLifes += 1;
			}

			if(type == PickUpType.RapidShot)
			{

			}

			PoolManager.Instance.SpawnPools["Misc"].Despawn(gameObject);
		}
	}

	public PickUpType Type
	{
		get
		{
			return type;
		}

		set
		{
			type = value;
		}
	}

}
