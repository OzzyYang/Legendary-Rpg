using UnityEngine;
public class PlayerState
{
	protected PlayerStateMachine stateMachine;
	protected PlayerController player;
	protected Rigidbody2D rb;

	protected string animBoolName;
	protected float xInput;
	protected float yInput;

	protected float stateTimer;
	protected float stateDuration;

	protected bool isTrrigerCalled;
	private readonly bool needDebug = false;


	public PlayerState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName)
	{
		this.player = _player;
		this.stateMachine = _stateMachine;
		this.animBoolName = _animBoolName;
	}

	public virtual void Enter()
	{
		if (needDebug) player.ShowInfo(animBoolName + " enter");
		player.animator.SetBool(animBoolName, true);
		rb = player.rb;
		isTrrigerCalled = false;
	}

	public virtual void Update()
	{
		xInput = Input.GetAxisRaw("Horizontal");
		yInput = Input.GetAxisRaw("Vertical");

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
		if (needDebug) player.ShowInfo(animBoolName + " exit");
		player.animator.SetBool(animBoolName, false);

	}

	public virtual void AnimationFinishTrigger()
	{
		isTrrigerCalled = true;
	}
}
