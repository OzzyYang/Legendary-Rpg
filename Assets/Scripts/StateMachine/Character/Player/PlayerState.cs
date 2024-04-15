using UnityEngine;
public class PlayerState : CharacterState
{
	protected PlayerController player;
	protected float xInput;
	protected float yInput;

	//private readonly bool needDebug = false;

	public PlayerState(PlayerController _character, PlayerStateMachine _stateMachine, string _animBoolName) : base(_character, _stateMachine, _animBoolName)
	{
		this.character = this.player = _character;
		this.stateMachine = _stateMachine;
		this.animBoolName = _animBoolName;
	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Update()
	{
		base.Update();
		xInput = Input.GetAxisRaw("Horizontal");
		yInput = Input.GetAxisRaw("Vertical");

		player.animator.SetBool("isLevitating", !player.isGroundedDetected());
		player.animator.SetBool("isGrounded", player.isGroundedDetected());

	}

	public override void Exit()
	{
		base.Exit();

	}

}
