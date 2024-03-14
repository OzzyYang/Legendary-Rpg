public class PlayerThrowSwordState : PlayerState
{
	public PlayerThrowSwordState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void AnimationFinishTrigger()
	{
		base.AnimationFinishTrigger();
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
		if (isTrrigerCalled)
		{
			stateMachine.ChangeState(player.idleState);
			player.skill.swordSkill.UseSkill();
		}
	}
}
