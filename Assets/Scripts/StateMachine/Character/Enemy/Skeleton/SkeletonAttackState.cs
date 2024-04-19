using UnityEngine;

public class SkeletonAttackState : EnemyState
{
	public SkeletonAttackState(EnemyController _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
	{
	}

	public override void AnimationFinishTrigger()
	{
		base.AnimationFinishTrigger();
	}

	public override void Enter()
	{
		base.Enter();
		((SkeletonController)enemy).lastAttackTime = Time.time;
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		if (isTriggerCalled)
		{
			//if ((enemy.isPlayerDetected().distance <= ((SkeletonController)enemy).attackCheckDistance))
			//{
			//	stateMachine.ChangeState((enemy as SkeletonController).attackState);
			//}
			//else
			//{
			stateMachine.ChangeState(enemy.idleState);

			//}

		}
	}
}
