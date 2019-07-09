using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatio : MonoBehaviour
{
	void Start ()
	{
		Camera.main.aspect = 2048f / 1536f;
	}
}
