using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShipData
{
	[SerializeField]
	private ShipType type;
	[SerializeField]
	private int speed;
	[SerializeField]
	private int endurance;
	[SerializeField]
	private int cost;
	[SerializeField]
	private Sprite shipSprite;
	[SerializeField]
	private RuntimeAnimatorController shipAnimation;

	public ShipType Type
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

	public int Speed
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

	public int Endurance
	{
		get
		{
			return endurance;
		}

		set
		{
			endurance = value;
		}
	}

	public int Cost
	{
		get
		{
			return cost;
		}

		set
		{
			cost = value;
		}
	}

	public Sprite ShipSprite
	{
		get
		{
			return shipSprite;
		}

		set
		{
			shipSprite = value;
		}
	}

	public RuntimeAnimatorController ShipAnimation
	{
		get
		{
			return shipAnimation;
		}

		set
		{
			shipAnimation = value;
		}
	}
}