using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class PlayerControls : MonoBehaviour
{
	[Range(1, 10)]
	public float maxSpeed;
	private Bounds spriteBounds;
	private float overAllSpeed;

	private void Start()
	{
		gameObject.GetComponent<SpriteRenderer>().sprite = GameManager.Instance.CurrentShip.ShipSprite;
		gameObject.GetComponent<Animator>().runtimeAnimatorController = GameManager.Instance.CurrentShip.ShipAnimation;

		spriteBounds = GetComponent<SpriteRenderer>().bounds;
		gameObject.GetComponent<BoxCollider2D>().size = spriteBounds.size;

		GameManager.Instance.CurrentState = GameState.GamePlay;

		overAllSpeed = maxSpeed + (maxSpeed * (GameManager.Instance.CurrentShip.Speed / 10));

		MasterAudio.PlaySound("Weapon");
	}

	void Update()
	{
		HandleInput();
	}

	public void HandleInput()
	{
		Vector2 pos = transform.position;

		pos.x += Input.GetAxis("Horizontal") * overAllSpeed * Time.deltaTime;
		pos.y += Input.GetAxis("Vertical") * overAllSpeed * Time.deltaTime;

		Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(Vector3.zero);
		Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

		Vector3 movementRangeMin = bottomLeft + spriteBounds.extents;
		Vector3 movementRangeMax = topRight - spriteBounds.extents;

		pos.x = Mathf.Clamp(pos.x, movementRangeMin.x, movementRangeMax.x);
		pos.y = Mathf.Clamp(pos.y, movementRangeMin.y, movementRangeMax.y);

		transform.position = pos;
	}
}