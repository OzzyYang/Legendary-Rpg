using System.Collections;
using UnityEngine;

public class PlayerController : CharacterController
{

	#region State machine info
	public PlayerStateMachine stateMachine { get; private set; }
	public PlayerGroundedState groundedState { get; private set; }
	public PlayerIdleState idleState { get; private set; }
	public PlayerMoveState moveState { get; private set; }
	public PlayerDashState dashState { get; private set; }
	public PlayerJumpState jumpState { get; private set; }
	public PlayerLevitationState levitateState { get; private set; }
	public PlayerWallSlideState wallSlideState { get; private set; }
	public PlayerWallJumpState wallJumpState { get; private set; }
	public PlayerAttackState attackState { get; private set; }
	#endregion

	#region Player info
	public bool isBusy { get; private set; }
	public Vector2 initialPos { get; private set; }
	[Header("Attack Info")]
	public Vector2[] attackMovement;
	[Header("Movement info")]
	public float playerSpeed = 8f;
	public float jumpForce = 10f;
	public float dashSpeed = 25f;
	public float dashDuration = 0.2f;
	public float dashDirection { get; private set; } = 1;

	public float dashCoolDownTime = 1.0f;
	public float dashCoolDownTimer;
	#endregion
	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		stateMachine.Initialize(idleState);
		initialPos = transform.position;
	}

	protected override void Awake()
	{
		base.Awake();
		stateMachine = new PlayerStateMachine();
		groundedState = new PlayerGroundedState(this, stateMachine, "isGrounded");
		idleState = new PlayerIdleState(this, stateMachine, "isIdling");
		moveState = new PlayerMoveState(this, stateMachine, "isMoving");
		dashState = new PlayerDashState(this, stateMachine, "isDashing");
		jumpState = new PlayerJumpState(this, stateMachine, "isLevitating");
		levitateState = new PlayerLevitationState(this, stateMachine, "isLevitating");
		wallSlideState = new PlayerWallSlideState(this, stateMachine, "isWallSliding");
		wallJumpState = new PlayerWallJumpState(this, stateMachine, "isLevitating");
		attackState = new PlayerAttackState(this, stateMachine, "isAttacking");

	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
		stateMachine.currentState.Update();

		CheckForInput();

		if (dashCoolDownTimer > 0)
		{
			dashCoolDownTimer -= Time.deltaTime;
		}

	}

	private void CheckForInput()
	{
		//check dash input
		if (Input.GetKeyDown(KeyCode.LeftShift) && dashCoolDownTimer <= 0)
		{
			if (isWallDetected()) return;
			DashDirectionController(Input.GetAxisRaw("Horizontal"));
			Debug.Log(Input.GetAxisRaw("Horizontal") + "     " + dashDirection);
			dashCoolDownTimer = dashCoolDownTime;
			stateMachine.ChangeState(dashState);
		}

		if (Input.GetKeyDown(KeyCode.R))
			transform.position = initialPos;

	}

	public IEnumerator BusyFor(float _seconds)
	{
		isBusy = true;
		yield return new WaitForSeconds(_seconds);
		isBusy = false;
	}

	public void AnimationTrrier() => stateMachine.currentState.AnimationFinishTrigger();

	public void DashDirectionController(float _xInput)
	{
		if (_xInput == 0)
		{
			dashDirection = facingDirection;
			return;
		}
		dashDirection = _xInput > 0 ? 1 : -1;

	}




}

