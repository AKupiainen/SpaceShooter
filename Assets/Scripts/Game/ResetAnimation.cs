using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimation : MonoBehaviour
{
	private Animator animator;

	private void Awake()
	{
		animator = gameObject.GetComponent<Animator>();
	}

	void Update()
	{
		AnimatorStateInfo anim = animator.GetCurrentAnimatorStateInfo(0);

		if (anim.normalizedTime >= 1)
		{
			PoolManager.Instance.SpawnPools["Misc"].Despawn(gameObject);
		}
	}

	private void OnEnable()
	{
		animator.Play("Explosion", -1, 0f);
	}
}
