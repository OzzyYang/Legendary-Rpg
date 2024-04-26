public class PlayerDyingState : PlayerState
{
	public PlayerDyingState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void AnimationFinishTrigger()
	{
		base.AnimationFinishTrigger();
	}

	public override void Enter()
	{
		base.Enter();
		UIManager.instance.PlayeEndScreen();

	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		player.SetVelocity(0, rb.velocity.y);
	}

}
