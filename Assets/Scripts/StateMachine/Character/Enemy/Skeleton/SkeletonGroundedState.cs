public class SkeletonGroundedState : EnemyState
{
	public SkeletonGroundedState(EnemyController _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
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
		if (enemy.isPlayerDetected().collider != null)
		{
			stateMachine.ChangeState(((SkeletonController)enemy).battleState);
		}
	}
}
