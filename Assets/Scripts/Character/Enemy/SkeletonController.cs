using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : CharacterController
{
	[Header("Move info")]
	[SerializeField] private float normalMoveSpeed = 2.0f;
	private float currentMoveSpeed = 2.0f;

	[Header("Attack info")]
	[SerializeField] private float attackArrange = 2.0f;

	[Header("Player Check Info")]
	[SerializeField] private LayerMask whatIsPlayer;
	[SerializeField] private RaycastHit2D isPlayerDetected;
	[SerializeField] private float checkPlayerDistance = 5.0f;
	[SerializeField] private bool isFacingPlayer = false;
	[SerializeField] private bool isAttacking = false;
	// Start is called before the first frame update
	void Start()
	{
		base.Start();
		currentMoveSpeed = normalMoveSpeed;
	}

	// Update is called once per frame
	void Update()
	{
		base.Update();
		PlayerCheck();
	}

	protected override void Move()
	{
		if (isGrounded) rb.velocity = new Vector2(currentMoveSpeed * (isFacingRight ? 1 : -1), rb.velocity.y);

		if ((!isGrounded || isFacingWall) && rb.velocity.y == 0) { Flip(!(transform.rotation.y == 0)); }
	}

	protected override void Attack()
	{
	}

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		//Draw check player ray
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + checkPlayerDistance * (isFacingRight ? 1 : -1), transform.position.y));
	}

	private void PlayerCheck()
	{
		isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right * (isFacingRight ? 1 : -1), checkPlayerDistance, whatIsPlayer);
		if (isPlayerDetected)
		{
			currentMoveSpeed = normalMoveSpeed * 2;
			if (isPlayerDetected.distance <= attackArrange)
			{
				currentMoveSpeed = 0;
				isAttacking = true;
			}
			else
			{
				currentMoveSpeed = normalMoveSpeed * 2;
				isAttacking = false;
			}
		}
		else
		{
			currentMoveSpeed = normalMoveSpeed;
			isAttacking = false;
		}
	}
	protected override void CollisionCheck()
	{
		base.CollisionCheck();
		isFacingPlayer = Physics2D.Raycast(transform.position, Vector2.right * (isFacingRight ? 1 : -1), attackArrange, whatIsPlayer);

	}
}

