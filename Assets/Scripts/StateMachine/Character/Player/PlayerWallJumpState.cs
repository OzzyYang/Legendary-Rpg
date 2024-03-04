public class PlayerWallJumpState : PlayerState
{
	public PlayerWallJumpState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		stateTimer = 0.4f;
		player.Flip();
		player.SetVelocity(5 * player.facingDirection, player.jumpForce);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();

		if (player.isGroundedDetected())
		{
			stateMachine.ChangeState(player.idleState);
			return;
		}

	}

}
