public class PlayerMoveState : PlayerGroundedState
{
	public PlayerMoveState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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


		if (xInput == 0 || player.isWallDectected())
		{
			stateMachine.ChangeState(player.idleState);
		}
		else
		{
			player.SetVelocity(xInput * player.playerSpeed, rb.velocity.y);
		}


	}
}
