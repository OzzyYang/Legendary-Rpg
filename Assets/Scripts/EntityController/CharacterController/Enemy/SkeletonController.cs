using UnityEngine;

public class SkeletonController : EnemyController
{
	#region State Info
	public SkeletonMoveState moveState { get; protected set; }
	public SkeletonBattleState battleState { get; protected set; }
	public SkeletonAttackState attackState { get; protected set; }
	public SkeletonStunnedState stunnedState { get; protected set; }
	//public SkeletonDyingState dyingState { get; protected set; }
	#endregion


	protected override void Awake()
	{
		base.Awake();
		idleState = new SkeletonIdleState(this, stateMachine as EnemyStateMachine, "isIdling");
		moveState = new SkeletonMoveState(this, stateMachine as EnemyStateMachine, "isMoving");
		battleState = new SkeletonBattleState(this, stateMachine as EnemyStateMachine, "isMoving");
		attackState = new SkeletonAttackState(this, stateMachine as EnemyStateMachine, "isAttacking");
		stunnedState = new SkeletonStunnedState(this, stateMachine as EnemyStateMachine, "isStunned");
		dyingState = new SkeletonDyingState(this, stateMachine as EnemyStateMachine, "isIdling");
	}

	protected override void Start()
	{
		stateMachine.Initialize(idleState);
		base.Start();
	}

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(attackCheck.position, new Vector3(attackCheck.position.x + attackCheckDistance * facingDirection, attackCheck.position.y));
	}

	public override bool CanBeStunned()
	{
		if (base.CanBeStunned())
		{
			stateMachine.ChangeState(stunnedState);
			return true;
		}
		return false;
	}

	public override void FreezeMovement(bool _needFreeze)
	{
		base.FreezeMovement(_needFreeze);
		stateMachine.ChangeState(idleState); stateMachine.ChangeState(idleState);
		(stateMachine.currentState as EnemyState).FreezeState(_needFreeze);
	}


}

