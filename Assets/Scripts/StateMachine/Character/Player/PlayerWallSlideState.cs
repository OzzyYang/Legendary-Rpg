using System;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
	public PlayerWallSlideState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

		//if playe do not controll or input direction is not same as facing direction, exit state.
		if (xInput == 0 || Math.Sign(xInput) != Math.Sign(player.facingDirection))
		{
			stateMachine.ChangeState(player.idleState);
			return;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			stateMachine.ChangeState(player.wallJumpState);
			return;
		}


		if (player.isGroundedDetected())
		{
			stateMachine.ChangeState(player.idleState);
		}
		else
		{
			if (!player.isWallDectected())
			{
				stateMachine.ChangeState(player.levitateState);
			}
		}

		player.SetVelocity(rb.velocity.x, rb.velocity.y * 0.7f);
	}
}
