using UnityEngine;

public class SkeletonBattleState : EnemyState
{
	protected GameObject player;
	protected int moveDirection;

	public SkeletonBattleState(EnemyController _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		enemy.SetVelocity(0, rb.velocity.y);
		stateTimer = (enemy as SkeletonController).battleTime;
		player = PlayerManager.instance.player.gameObject;
	}

	public override void Exit()
	{
		base.Exit();
		enemy.SetVelocity(0, rb.velocity.y);
	}

	public override void Update()
	{
		base.Update();
		if (enemy.isPlayerDetected().collider != null)
		{
			if (CanAttack() && (enemy.isPlayerDetected().distance <= ((SkeletonController)enemy).attackCheckDistance))
			{
				stateMachine.ChangeState(((SkeletonController)enemy).attackState);
				return;
			}
		}
		else
		{
			//if battle state lasting some time or player is far enough from skeleton when skeleton cannot attack player, exit battle state.
			if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 12)
			{
				stateMachine.ChangeState(((SkeletonController)enemy).idleState);
				return;
			}
		}

		moveDirection = player.transform.position.x > rb.position.x ? 1 : -1;
		enemy.FlipController(moveDirection);
		enemy.SetVelocity(enemy.currentMoveSpeed * 1.2f * moveDirection, rb.velocity.y);
	}

	public bool CanAttack()
	{
		if ((enemy as SkeletonController).lastAttackTime == 0) return true;
		if (Time.time - (enemy as SkeletonController).lastAttackTime >= (enemy as SkeletonController).attackCooldownTime)
			return true;
		return false;
	}
}
