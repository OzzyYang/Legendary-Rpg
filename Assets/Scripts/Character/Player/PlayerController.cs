using System;
using UnityEngine;

public class PlayerController :CharacterController
{
	private float currentMoveSpeed;

	[Header("Movement Info")]
	[SerializeField] private float jumpForce = 10f;
	[SerializeField] private float moveSpeed = 5f;

	[Header("Dash Info")]
	[SerializeField] private float dashDuration = 0.2f;
	[SerializeField] private float dashCooldownTime = 2f;
	[SerializeField] private float dashSpeed = 19.0f;
	private float dashTime;
	private float dashCooldownTimer = 0;
	private int dashDiretion = 1;

	[Header("Attack Info")]
	[SerializeField] private bool isAttacking = false;
	[SerializeField] private int attackCounter = 0;
	[SerializeField] private float attackComboTimer = 0f;
	[SerializeField] private float attackComboTime = 0.3f;

	//State machine info
	public PlayerStateMachine stateMachine {  get; private set; }
	public PlayerIdleState idleState { get; private set; }
	public PlayerMoveState moveState { get; private set; }

	// Start is called before the first frame update
	void Start()
	{
		base.Start();
		stateMachine.Initialize(idleState);
	}

	private void Awake()
	{
		stateMachine=new PlayerStateMachine();
		idleState = new PlayerIdleState(this,stateMachine,"Idle");
		moveState = new PlayerMoveState(this, stateMachine, "Move");
	}

	// Update is called once per frame
	void Update()
	{
		base.Update();
		CheckInput();
		CheckTimer();
		Move();
		Dash();
		CheckAnimation();
		stateMachine.currentState.Update();
		if (Input.GetKeyDown(KeyCode.N))
		{
			stateMachine.ChangeState(stateMachine.currentState == idleState ? moveState : idleState);
		}
	}
	protected override void Move()
	{
		currentMoveSpeed = isAttacking ? 0 : Input.GetAxisRaw("Horizontal") * moveSpeed;
		switch (Input.GetAxisRaw("Horizontal"))
		{
			case 1:
				{
					Flip(true);
					break;
				}
			case -1:
				{
					Flip(false);
					break;
				}
			default:
				{
					break;
				}
		}
		rb.velocity = new Vector2(currentMoveSpeed, rb.velocity.y);
		//ResetAttackCounter();
	}
	void Jump()
	{
		if (isGrounded)
		{
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			ResetAttackCounter();
		}
	}

	private void ResetAttackCounter()
	{
		attackCounter = 0;
		isAttacking = false;
	}

	void Dash()
	{
		if (dashTime >= 0)
		{
			rb.velocity = new Vector2(dashSpeed * dashDiretion, rb.velocity.y);
			ResetAttackCounter();
		}
	}


	protected override void Attack()
	{
		if (!isGrounded) return;
		isAttacking = true;
	}

	public void AttackOver()
	{
		isAttacking = false;
		attackComboTimer = attackComboTime;
		if (attackCounter >= 2)
		{
			attackCounter = 0;
		}
		else
		{
			attackCounter++;
		}
	}

	private void CheckAnimation()
	{
		ani.SetBool("isMoving", currentMoveSpeed != 0);
		ani.SetBool("isGrounded", isGrounded);
		ani.SetFloat("yVelocity", rb.velocity.y);
		ani.SetBool("isDashing", dashTime >= 0);
		ani.SetBool("isAttacking", isAttacking);
		ani.SetInteger("attackCounter", attackCounter);
	}
	private void CheckInput()
	{
		if (Input.GetKey(KeyCode.Space)) Jump();
		if (Input.GetKey(KeyCode.LeftShift))
		{
			//dash only when cd ends
			if (dashCooldownTimer <= 0)
			{
				//reset timer
				dashCooldownTimer = dashCooldownTime;
				//reset duratoion time
				dashTime = dashDuration;
				//record dash diretion
				dashDiretion = transform.rotation.eulerAngles.y == 0f ? 1 : -1;
			}
		}
		if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.X))
		{
			if (!isAttacking) Attack();
		}
	}
	private void CheckTimer()
	{
		if (dashCooldownTimer >= 0) dashCooldownTimer -= Time.deltaTime;
		if (dashTime >= 0) dashTime -= Time.deltaTime;

		if (attackComboTimer >= 0)
		{
			attackComboTimer -= Time.deltaTime;
		}
		else
		{
			//if the attacking isn't finished, do not change attackCounter
			attackCounter = isAttacking? attackCounter:0;
		}
	}



}

