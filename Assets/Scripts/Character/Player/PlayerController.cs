using System;
using UnityEngine;

public class PlayerController :CharacterController
{

	#region State machine info
	public PlayerStateMachine stateMachine {  get; private set; }
	public PlayerGroundedState groundedState { get; private set; }
	public PlayerIdleState idleState { get; private set; }
	public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
	public PlayerLevitationState levitateState { get; private set; }
	#endregion

	#region Player info
	[Header("Movement info")]
	public float playerSpeed = 8f;
	public float jumpForce = 10f;
	#endregion
	// Start is called before the first frame update
	void Start()
	{
		base.Start();
		stateMachine.Initialize(idleState);
	}

	private void Awake()
	{
		stateMachine=new PlayerStateMachine();
		groundedState = new PlayerGroundedState(this, stateMachine, "isGrounded");
		idleState = new PlayerIdleState(this,stateMachine,"isIdling");
		moveState = new PlayerMoveState(this, stateMachine, "isMoving");
		jumpState = new PlayerJumpState(this, stateMachine, "isJumping");
		levitateState = new PlayerLevitationState(this, stateMachine, "isFloating");

	}

	// Update is called once per frame
	void Update()
	{
		base.Update();
		stateMachine.currentState.Update();

	}
	void Jump()
	{

	}

	private void ResetAttackCounter()
	{

	}

	void Dash()
	{

	}


	protected override void Attack()
	{

	}

	public void AttackOver()
	{

	}

	private void CheckAnimation()
	{

	}
	private void CheckInput()
	{

	}
	private void CheckTimer()
	{

	}



}

