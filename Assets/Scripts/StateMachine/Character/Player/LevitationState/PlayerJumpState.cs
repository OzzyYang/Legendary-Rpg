

public class PlayerJumpState : PlayerLevitationState
{
	public PlayerJumpState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		player.SetPosition(rb.position.x, rb.position.y + 0.2f);
		player.SetVelocity(rb.velocity.x, player.jumpForce);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
	}
}
