using System;

public class PlayerIdleState : PlayerGroundedState
{
	public PlayerIdleState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		player.SetVelocity(0, 0);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		if (xInput != 0 && player.isGroundedDetected())
		{
			//where a wall standing in front of the player, do not move;
			if (Math.Sign(xInput) == player.facingDirection && player.isWallDectected())
				return;
			stateMachine.ChangeState(player.moveState);
			player.FlipController(xInput);
		}
	}

}
