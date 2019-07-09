using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	[SerializeField]
	private float orginalHealth;
	[SerializeField]
	private float speed;
	[SerializeField]
	private float health;
	[SerializeField]
	private int scoreAmount;
	[SerializeField]
	private float damage;
	private StateMachine machine;
	[SerializeField]
	private Spawner spawner;

	public virtual void Awake()
	{
		spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
		orginalHealth = health;
	}

	private void OnEnable()
	{
		LevelManager.Instance.OverAllEnemies++;
	}

	public virtual void OnDeath()
	{
		if (health <= 0)
		{
			LevelManager.Instance.EnemiesDestroyed += 1;
			spawner.Waves[spawner.waveId].RemainingItems -= 1;
			PlayerManager.Instance.PlayerScore += scoreAmount;
		}

		health = orginalHealth;
		PoolManager.Instance.SpawnPools["Misc"].Despawn(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			if (PlayerManager.Instance.CurrentShieldCharge > 0)
			{
				PlayerManager.Instance.CurrentShieldCharge -= Damage;
			}

			else
			{
				PlayerManager.Instance.CurrentHealth -= Damage;
			}

			PlayerManager.Instance.IsPlayerHit = true;
		}
	}

	public float OrginalHealth
	{
		get
		{
			return orginalHealth;
		}

		set
		{
			orginalHealth = value;
		}
	}

	public float Speed
	{
		get
		{
			return speed;
		}

		set
		{
			speed = value;
		}
	}

	public float Health
	{
		get
		{
			return health;
		}

		set
		{
			health = value;
		}
	}

	public int ScoreAmount
	{
		get
		{
			return scoreAmount;
		}

		set
		{
			scoreAmount = value;
		}
	}

	public StateMachine Machine
	{
		get
		{
			return machine;
		}

		set
		{
			machine = value;
		}
	}

	public float Damage
	{
		get
		{
			return damage;
		}

		set
		{
			damage = value;
		}
	}
}