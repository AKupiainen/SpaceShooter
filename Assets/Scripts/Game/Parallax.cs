using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
	[SerializeField]
	private float[] parallaxSpeed;
	[SerializeField]
	private SpriteRenderer[,] parallaxLayers;
	[SerializeField]
	private int parallaxLayerCount;

	private void Start()
	{
		parallaxLayers = new SpriteRenderer[parallaxLayerCount, 3];

		FindParallaxLayers();
		InstantiateParallaxLayers();
	}

	void Update()
	{
		UpdateParallaxPositions();
	}

	void FindParallaxLayers()
	{
		for(int i = 0; i < parallaxLayerCount; i++)
		{
			parallaxLayers[i, 0] = GameObject.Find("Parallax/Layer" + (i + 1).ToString()).GetComponentInChildren<SpriteRenderer>();
		}
	}

	void InstantiateParallaxLayers()
	{
		for (int i = 0; i < parallaxLayerCount; i++)
		{
			for(int j = 0; j < 2; j++)
			{
				if(j == 0)
				{
					SpriteRenderer layer = Instantiate<SpriteRenderer>(parallaxLayers[i, 0]);
					layer.transform.position = new Vector2(parallaxLayers[i, 0].transform.position.x, parallaxLayers[i, 0].transform.position.y + parallaxLayers[i, 0].sprite.bounds.size.y * 2);
					layer.transform.parent = parallaxLayers[i, 0].transform.parent;
					layer.name = parallaxLayers[i, 0].name;
					parallaxLayers[i, j + 1] = layer;
				}

				else
				{
					SpriteRenderer layer = Instantiate<SpriteRenderer>(parallaxLayers[i, 0]);
					layer.transform.position = new Vector2(parallaxLayers[i, j].transform.position.x, parallaxLayers[i, j].transform.position.y + parallaxLayers[i, 0].sprite.bounds.size.y * 2);
					layer.transform.parent = parallaxLayers[i, j].transform.parent;
					layer.name = parallaxLayers[i, j].name;
					parallaxLayers[i, j + 1] = layer;
				}
			}
		}
	}

	void UpdateParallaxPositions()
	{
		for(int i = 0; i < parallaxLayers.GetLength(0); i++)
		{
			for (int j = 0; j < parallaxLayers.GetLength(1); j++)
			{
				Vector3 positionToViewPort = Camera.main.WorldToViewportPoint(new Vector2(0, parallaxLayers[i, j].transform.position.y + parallaxLayers[i, j].sprite.bounds.extents.y * 2));

				if (positionToViewPort.y < -0.5f)
				{
					parallaxLayers[i, 0].transform.position = new Vector2(parallaxLayers[i, 0].transform.position.x, parallaxLayers[i, 2].transform.position.y + parallaxLayers[i, 0].sprite.bounds.size.y * 2);

					SpriteRenderer temp1 = parallaxLayers[i, 0];
					SpriteRenderer temp2 = parallaxLayers[i, 1];
					SpriteRenderer temp3 = parallaxLayers[i, 2];

					parallaxLayers[i, 0] = temp3;
					parallaxLayers[i, 1] = temp1;
					parallaxLayers[i, 2] = temp2;
				}

				parallaxLayers[i, j].transform.Translate(Vector3.down * parallaxSpeed[i] * Time.deltaTime);
			}
		}
	}
}
