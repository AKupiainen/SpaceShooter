using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrade
{
	[SerializeField]
	private Sprite icon;
	[SerializeField]
	private byte rank;
	[SerializeField]
	private string title;
	[SerializeField]
	private string desc;
	[SerializeField]
	private int[] ranks = new int[5];

	public Sprite Icon
	{
		get
		{
			return icon;
		}

		set
		{
			icon = value;
		}
	}

	public byte Rank
	{
		get
		{
			return rank;
		}

		set
		{
			rank = value;
		}
	}

	public string Title
	{
		get
		{
			return title;
		}

		set
		{
			title = value;
		}
	}

	public string Desc
	{
		get
		{
			return desc;
		}

		set
		{
			desc = value;
		}
	}

	public int[] Ranks
	{
		get
		{
			return ranks;
		}

		set
		{
			ranks = value;
		}
	}
}