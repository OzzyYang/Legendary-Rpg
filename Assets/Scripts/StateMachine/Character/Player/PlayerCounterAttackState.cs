using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
	private bool hasCreatedClone = false;
	public PlayerCounterAttackState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		stateTimer = player.counterAttackDuration;
		player.animator.SetBool("CASuccessful", false);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
		foreach (var hit in colliders)
		{
			EnemyController enemy = hit.GetComponent<EnemyController>();
			if (enemy != null)
			{
				if (enemy.CanBeStunned())
				{
					stateTimer = 10;//any value bigger than 1
					player.animator.SetBool("CASuccessful", true);
					if (!hasCreatedClone)
					{
						player.skill.counterAttackSkill.CreateCloneOver(enemy.transform);
						hasCreatedClone = true;
					}
				}
			}

		}
		hasCreatedClone = false;
		if (stateTimer < 0 || isTrrigerCalled)
			stateMachine.ChangeState(player.idleState);
	}
}
