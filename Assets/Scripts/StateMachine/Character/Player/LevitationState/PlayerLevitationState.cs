public class PlayerLevitationState : PlayerState
{
	public PlayerLevitationState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		player.animator.SetFloat("yVelocity", rb.velocity.y / player.jumpForce);

		//Player can move in lower speed when levitating
		if (xInput != 0)
		{
			player.SetVelocity(xInput * player.playerSpeed * 0.8f, rb.velocity.y);
			player.FlipController(xInput);
		}

		if (player.isGroundedDetected())
		{
			//player.SetVelocity(0, 0);
			stateMachine.ChangeState(player.idleState);

		}
		else
		{
			if (player.isWallDetected() && rb.velocity.y < 0 && yInput >= 0)
			{
				stateMachine.ChangeState(player.wallSlideState);
			}
		}
	}
}
