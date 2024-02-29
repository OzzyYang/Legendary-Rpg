using UnityEngine;

public class PlayerState
{
	protected PlayerStateMachine stateMachine;
	protected PlayerController player;
	protected Rigidbody2D rb;

	protected string animBoolName;
	protected float xInput;

	protected float stateTimer;
	protected float stateDuration;


	public PlayerState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName)
	{
		this.player = _player;
		this.stateMachine = _stateMachine;
		this.animBoolName = _animBoolName;
	}

	public virtual void Enter()
	{
		player.animator.SetBool(animBoolName, true);
		rb = player.rb;
	}

	public virtual void Update()
	{
		xInput = Input.GetAxis("Horizontal");

		player.animator.SetBool("isLevitating", !player.isGroundedDetected());
		player.animator.SetBool("isGrounded", player.isGroundedDetected());

		StateTimerController();


	}

	private void StateTimerController()
	{
		if (stateTimer >= 0)
			stateTimer -= Time.deltaTime;
	}

	public virtual void Exit()
	{
		player.animator.SetBool(animBoolName, false);

	}
}
