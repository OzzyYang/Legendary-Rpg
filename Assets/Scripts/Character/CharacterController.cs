using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterController : MonoBehaviour
{

	public Animator animator { get; private set; }
	public Rigidbody2D rb { get; private set; }


	[Header("Collision Check Info")]
	[SerializeField] protected Transform groundCheck;
	[SerializeField] protected float groundCheckDistance = 0.1f;
	[SerializeField] protected Transform wallCheck;
	[SerializeField] protected float wallCheckDistance=0.1f;
	[SerializeField] protected LayerMask whatIsGround;


	// Start is called before the first frame update
	protected virtual void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		if (groundCheck == null) groundCheck = transform;
		if (wallCheck == null) wallCheck = transform;
	}

	// Update is called once per frame
	protected virtual void Update()
	{

	}

	public bool isGroundedDetected() { return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround) && (rb.velocity.y == 0); }
	protected virtual void OnDrawGizmos()
	{
		Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
		Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
	}
	protected virtual void CollisionCheck()
	{
	}

	protected void Flip(bool facingRight)
	{
	}

	protected virtual void Attack() { }
	public virtual void SetVelocity(float _xVelocity, float _yVelocity)
	{
		rb.velocity = new Vector2(_xVelocity, _yVelocity);
	}



}
