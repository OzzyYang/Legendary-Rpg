using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevitationState : PlayerState
{
	public PlayerLevitationState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		player.animator.SetFloat("yVelocity", rb.velocity.y / player.jumpForce);
		if (player.isGroundedDetected())
			stateMachine.ChangeState(player.idleState);
	}
}
