using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
	protected GameObject player;
	public SkeletonGroundedState(EnemyController _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		player = PlayerManager.instance.player.gameObject;
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();

		if (stateTimer == Mathf.Infinity) return;

		if (enemy.isPlayerDetected().collider != null || Vector2.Distance(player.transform.position, enemy.transform.position) <= 2)
		{
			stateMachine.ChangeState(((SkeletonController)enemy).battleState);
			return;
		}
	}
}
