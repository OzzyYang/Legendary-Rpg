public class SkeletonMoveState : SkeletonGroundedState
{
	public SkeletonMoveState(EnemyController _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();
		enemy.SetVelocity(0, rb.velocity.y);

	}

	public override void Update()
	{
		base.Update();
		if ((!enemy.isGroundedDetected() || enemy.isWallDetected()))
		{
			stateMachine.ChangeState(enemy.idleState);
			enemy.Flip();
			return;
		}
		enemy.SetVelocity(enemy.moveSpeed * enemy.facingDirection, rb.velocity.y);

	}
}
