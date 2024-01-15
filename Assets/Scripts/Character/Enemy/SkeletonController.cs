using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : CharacterController
{
	[Header("Move info")]
	[SerializeField] private float moveSpeed = 2.0f;
	// Start is called before the first frame update
	void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	void Update()
	{
		base.Update();
	}

	protected override void Move()
	{
		rb.velocity = new Vector2(moveSpeed * (isFacingRight ? 1 : -1), rb.velocity.y);
		//TODO: Fix the weirdo move when is not on the ground.
		if ((!isGrounded || isFacingWall) && rb.velocity.y == 0) { Flip(!(transform.rotation.y == 0)); }
	}
}
