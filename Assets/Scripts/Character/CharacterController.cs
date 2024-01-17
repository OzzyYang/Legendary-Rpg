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
	[SerializeField] protected LayerMask whatIsGround;
	[Space]
	[SerializeField] protected float checkWallDistance = 0.9f;
	[SerializeField] protected Transform whatToFaceWall;
	[SerializeField] protected Transform whatToCheckGround;
	protected bool isFacingWall = false;

	protected bool isGrounded = false;

	// Start is called before the first frame update
	protected virtual void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		ani = GetComponentInChildren<Animator>();
		if (whatToFaceWall == null) whatToFaceWall = transform;
		if (whatToCheckGround == null) whatToCheckGround = transform;
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		CollisionCheck();
		Move();
	}
	protected virtual void OnDrawGizmos()
	{
		//Draw check ground ray
		Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - checkGroundDistance));
		//Draw check wall ray
		Gizmos.DrawLine(whatToFaceWall.position, new Vector3(whatToFaceWall.position.x + checkWallDistance * (isFacingRight ? 1 : -1), whatToFaceWall.position.y));
	}
	protected virtual void CollisionCheck()
	{
		isGrounded = Physics2D.Raycast(whatToCheckGround.position, Vector2.down, checkGroundDistance, whatIsGround);
		isFacingWall = Physics2D.Raycast(whatToFaceWall.position, Vector2.right * (isFacingRight ? 1 : -1), checkWallDistance, whatIsGround);
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



}
