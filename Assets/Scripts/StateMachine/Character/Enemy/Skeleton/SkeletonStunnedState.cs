public class SkeletonStunnedState : EnemyState
{
	protected EntityFX fX;
	public SkeletonStunnedState(EnemyController _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		fX = enemy.GetComponentInChildren<EntityFX>();
		stateTimer = enemy.stunnedDuration;
		enemy.SetVelocity(enemy.stunnedMovement.x * -enemy.facingDirection, enemy.stunnedMovement.y);
		fX.InvokeRepeating("RedColorBlink", 0, 0.1f);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		if (stateTimer < 0)
		{
			stateMachine.ChangeState(enemy.idleState);
			fX.Invoke("CancelRedColorBlink", 0);
		}
	}
}
