using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
	[SerializeField]
	private List<PickUpItem> pickUps;
	private Spawner spawner;
	private List<Sprite> pickUpsprites;

	private static PickUpManager instance;
	private PickUpType currentPickUp;

	private void Awake()
	{
		spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
		GetPickUpSprites();
		currentPickUp = PickUpType.None;
	}

	public void SpawnPickUp()
	{
		PickUpType type = PickUps[spawner.waveId].PickUpType;
		Vector2 spawnPos = PickUps[spawner.waveId].SpawnPos;

		GameObject pickUp = PoolManager.Instance.SpawnPools["Misc"].Spawn("PickUp", spawnPos);
		pickUp.transform.position = spawnPos;
		pickUp.GetComponent<SpriteRenderer>().sprite = pickUpsprites.Find(x => x.name == type.ToString());
		pickUp.GetComponent<PickUp>().Type = type;
	}

	private void GetPickUpSprites()
	{
		pickUpsprites = new List<Sprite>();
		Sprite[] sprites = Resources.LoadAll<Sprite>("PickUps");

		foreach (Sprite spr in sprites)
		{
			pickUpsprites.Add(spr);
		}	
	}

	public List<PickUpItem> PickUps
	{
		get
		{
			if(pickUps == null)
			{
				pickUps = new List<PickUpItem>();
			}

			return pickUps;
		}

		set
		{
			pickUps = value;
		}
	}

	public List<Sprite> PickUpsprites
	{
		get
		{
			if(pickUpsprites == null)
			{
				GetPickUpSprites();
			}

			return pickUpsprites;
		}

		set
		{
			pickUpsprites = value;
		}
	}

	public static PickUpManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<PickUpManager>();
			}

			return instance;
		}
	}

	public PickUpType CurrentPickUp
	{
		get
		{
			return currentPickUp;
		}

		set
		{
			currentPickUp = value;
		}
	}
}

[System.Serializable]
public class PickUpItem
{
	[SerializeField]
	private Vector2 spawnPos;
	[SerializeField]
	private PickUpType pickUpType;

	public Vector2 SpawnPos
	{
		get
		{
			return spawnPos;
		}

		set
		{
			spawnPos = value;
		}
	}

	public PickUpType PickUpType
	{
		get
		{
			return pickUpType;
		}

		set
		{
			pickUpType = value;
		}
	}
}