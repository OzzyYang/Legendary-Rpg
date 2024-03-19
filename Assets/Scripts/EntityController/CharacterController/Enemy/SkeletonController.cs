using UnityEngine;

public class SkeletonController : EnemyController
{
	#region State Info
	public SkeletonMoveState moveState { get; protected set; }
	public SkeletonBattleState battleState { get; protected set; }
	public SkeletonAttackState attackState { get; protected set; }
	public SkeletonStunnedState stunnedState { get; protected set; }
	#endregion


	protected override void Awake()
	{
		base.Awake();
		idleState = new SkeletonIdleState(this, stateMachine, "isIdling");
		moveState = new SkeletonMoveState(this, stateMachine, "isMoving");
		battleState = new SkeletonBattleState(this, stateMachine, "isMoving");
		attackState = new SkeletonAttackState(this, stateMachine, "isAttacking");
		stunnedState = new SkeletonStunnedState(this, stateMachine, "isStunned");
	}

	protected override void Start()
	{
		base.Start();
		stateMachine.Initialize(idleState);
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
		stateMachine.currentState.FreezeState(_needFreeze);
	}
}

