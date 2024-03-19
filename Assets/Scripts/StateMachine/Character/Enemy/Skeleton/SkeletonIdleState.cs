public class SkeletonIdleState : SkeletonGroundedState
{


	public SkeletonIdleState(SkeletonController _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
	{

	}

	public override void Enter()
	{
		StateTime = 1.5f;
		base.Enter();
		enemy.SetVelocity(0, rb.velocity.y);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		if (stateTimer <= 0)
			stateMachine.ChangeState(((SkeletonController)enemy).moveState);
	}
}
