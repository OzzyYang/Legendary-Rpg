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
		player = GameObject.Find("Player");
	}

	public override void Exit()
	{
		base.Exit();
		enemy.SetVelocity(0, rb.velocity.y);
	}

	public override void Update()
	{
		base.Update();
		if (enemy.isPlayerDetected().distance <= ((SkeletonController)enemy).attackDistance)
		{
			enemy.SetVelocity(0, rb.velocity.y);
			if (((SkeletonController)enemy).CanAttack())
			{
				stateMachine.ChangeState(((SkeletonController)enemy).attackState);
			}
			else
			{
				//stateMachine.ChangeState(((SkeletonController)enemy).idleState);
			}
			return;
		}

		moveDirection = player.transform.position.x > rb.position.x ? 1 : -1;
		enemy.FlipController(moveDirection);
		enemy.SetVelocity(enemy.moveSpeed * 1.5f * moveDirection, rb.velocity.y);
	}
}
