using UnityEngine;

public class SkeletonAttackState : EnemyState
{
	public SkeletonAttackState(EnemyController _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
	{
	}

	public override void AnimationFnishedTrigger()
	{
		base.AnimationFnishedTrigger();
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
			stateMachine.ChangeState(enemy.idleState);
			return;
		}
	}
}
