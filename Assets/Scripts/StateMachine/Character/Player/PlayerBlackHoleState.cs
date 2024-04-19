public class PlayerBlackHoleState : PlayerLevitationState
{

	private float playerOriginGravityScale;
	public PlayerBlackHoleState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		playerOriginGravityScale = player.rb.gravityScale;
		rb.gravityScale = 0;
	}

	public override void Exit()
	{
		base.Exit();
		rb.gravityScale = playerOriginGravityScale;
	}

	public override void Update()
	{
		base.Update();
		player.SetVelocity(0, player.rb.velocity.y);
	}
}
