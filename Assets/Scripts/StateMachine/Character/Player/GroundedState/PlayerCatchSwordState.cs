public class PlayerCatchSwordState : PlayerState
{
	public PlayerCatchSwordState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
		if (isTriggerCalled)
			player.stateMachine.ChangeState(player.idleState);
	}

}
