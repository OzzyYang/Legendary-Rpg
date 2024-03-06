using UnityEngine;

public class PlayerAttackState : PlayerState
{
	private int attackCounter = 0;
	private float lastAttackTime;
	private float attackComboWindow = 1.2f;
	public PlayerAttackState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		if (attackCounter > 2 || (attackComboWindow <= Time.time - lastAttackTime))
			attackCounter = 0;

		player.animator.SetInteger("attackCounter", attackCounter);

		player.FlipController(xInput);

		//make player more alive when attacking
		player.SetVelocity(player.attackMovement[attackCounter].x * player.facingDirection, player.attackMovement[attackCounter].y);

		lastAttackTime = Time.time;
	}

	public override void Exit()
	{
		base.Exit();
		attackCounter++;
		player.StartCoroutine("BusyFor", 0.2f);
	}

	public override void Update()
	{
		base.Update();
		if (isTrrigerCalled)
		{
			stateMachine.ChangeState(player.idleState);
		}
	}
}
