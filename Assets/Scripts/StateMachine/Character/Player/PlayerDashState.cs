public class PlayerDashState : PlayerState
{
	public PlayerDashState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		stateDuration = player.dashDuration;
		stateTimer = stateDuration;
		player.FlipController(player.dashDirection);
		player.skill.cloneSkill.CreateClone(player.transform);
	}

	public override void Exit()
	{
		base.Exit();
		player.SetVelocity(0, rb.velocity.y);

	}

	public override void Update()
	{
		base.Update();
		player.SetVelocity(player.dashSpeed * player.dashDirection, 0);
		if (stateTimer < 0)
		{
			stateMachine.ChangeState(player.idleState);
		}

	}
}
