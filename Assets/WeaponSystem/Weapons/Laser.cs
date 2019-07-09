using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
	public SpriteRenderer laser;
	
	void Update ()
	{
		Vector2 screenEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 1f));
		float distance = Vector2.Distance(new Vector2(0f, screenEdge.y), new Vector2(0f, transform.position.y));

		Vector2 laserDirection = Vector2.up;
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, laserDirection, distance);

		if (hit.collider != null)
		{
			if(hit.collider.tag == "Enemy")
			{
				distance = Vector2.Distance(hit.point, this.transform.position);
				laser.size = new Vector2(laser.size.x, distance);

				//PoolManager.Instance.SpawnPools["Misc"].Despawn(hit.collider.gameObject);
				//PoolManager.Instance.SpawnPools["Misc"].Spawn("Explosion", hit.collider.transform.position);
			}
		}

		else
		{
			screenEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 1f));
			distance = Vector2.Distance(new Vector2(0f, screenEdge.y), new Vector2(0f, transform.position.y));
			laser.size = new Vector2(laser.size.x, distance);
		}
	}
}