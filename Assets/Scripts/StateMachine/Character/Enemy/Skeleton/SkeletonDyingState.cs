using UnityEngine;

public class SkeletonDyingState : EnemyState
{
	public SkeletonDyingState(EnemyController _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
	{
	}

	public override void AnimationFinishTrigger()
	{
		base.AnimationFinishTrigger();
	}

	public override void Enter()
	{
		base.Enter();
		enemy.GetComponent<Collider2D>().isTrigger = true;
		enemy.SetVelocity(0, 6);
		animator.SetBool((stateMachine as EnemyStateMachine).currentAnimBoolName, false);

	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
	}
}
