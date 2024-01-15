using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	protected Rigidbody2D rb;
	protected Animator ani;
	protected bool isFacingRight = true;

	[Header("Check Collision Info")]
	[SerializeField] protected float checkGroundDistance = 0.9f;
	[SerializeField] protected float checkWallDistance = 0.9f;
	[SerializeField] protected LayerMask whatIsGround;
	[SerializeField] protected bool isFacingWall = false;

	protected bool isGrounded = false;

	// Start is called before the first frame update
	protected virtual void Start()
    {
		rb = GetComponent<Rigidbody2D>();
		ani = GetComponentInChildren<Animator>();
	}

    // Update is called once per frame
    protected virtual void Update()
    {
		CollisionCheck();
		Move();
	}

	protected void CollisionCheck()
	{
		isGrounded = Physics2D.Raycast(new Vector2(transform.position.x + checkWallDistance*(isFacingRight? 1:-1) ,transform.position.y), Vector2.down, checkGroundDistance, whatIsGround);
		isFacingWall=Physics2D.Raycast(transform.position,Vector2.right* (isFacingRight ? 1 : -1), checkWallDistance,whatIsGround);
	}

	protected void Flip(bool facingRight)
	{
		if (facingRight == isFacingRight) return;
		else
		{
			isFacingRight = facingRight;
		}
		transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, facingRight ? 0 : 180, 0));
	}

	protected virtual void Attack() { }
	protected virtual void Move() { }


	protected virtual void OnDrawGizmos()
	{
		Gizmos.DrawLine(new Vector3(transform.position.x+checkWallDistance * (isFacingRight ? 1 : -1), transform.position.y), new Vector3(transform.position.x + checkWallDistance * (isFacingRight ? 1 : -1), transform.position.y - checkGroundDistance));
	}
}
