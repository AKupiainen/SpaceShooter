using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearBullet : MonoBehaviour
{
	private Vector3 foward;
	[SerializeField]
	private float speed;

	void Update ()
	{
		if(transform.eulerAngles.z > 0)
		{
			foward = (Vector2.up * transform.eulerAngles.z).normalized;
		}

		else
		{
			foward = Vector2.up;
		}

		transform.Translate(foward * speed * Time.deltaTime);

		Vector3 position = Camera.main.WorldToViewportPoint(transform.position);

		if(OutSideBounds(position))
		{
			PoolManager.Instance.SpawnPools["Misc"].Despawn(gameObject);
		}

	}

	private bool OutSideBounds(Vector2 position)
	{
		return position.y > 1f || position.y < 0f || position.x > 1f || position.x < 0f;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Enemy")
		{
			Enemy enemy = col.gameObject.GetComponent<Enemy>();
			enemy.Health -= 2;

			PoolManager.Instance.SpawnPools["Misc"].Spawn("Explosion", col.transform.position);
		}
	}
}