public class PlayerMoveState : PlayerGroundedState
{
	public PlayerMoveState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		player.SetVelocity(xInput * player.playerSpeed, rb.velocity.y);
	}

	public override void Exit()
	{
		base.Exit();
		player.SetVelocity(0, rb.velocity.y);
	}

	public override void Update()
	{
		base.Update();

		if (xInput == 0 || player.isWallDetected())
		{
			stateMachine.ChangeState(player.idleState);
			//player.ShowInfo("Change Idle");
			return;
		}




	}
}
