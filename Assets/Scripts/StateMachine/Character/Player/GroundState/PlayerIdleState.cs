public class PlayerIdleState : PlayerGroundedState
{
	public PlayerIdleState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
		if (xInput != 0 && player.isGroundedDetected())
		{
			stateMachine.ChangeState(player.moveState);
			player.FlipController(xInput);
		}
	}

}
