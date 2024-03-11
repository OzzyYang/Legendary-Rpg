using UnityEngine;

public class PlayerGroundedState : PlayerState
{
	public PlayerGroundedState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

		if (!player.isGroundedDetected())
		{
			stateMachine.ChangeState(player.levitateState);
			return;
		}


		if (Input.GetKeyDown(KeyCode.Space) && player.isGroundedDetected())
		{
			stateMachine.ChangeState(player.jumpState);
			return;
		}



		if (Input.GetKey(KeyCode.Mouse0))
		{
			stateMachine.ChangeState(player.attackState);
			return;
		}

		if (xInput != 0 && player.isGroundedDetected() && !player.isBusy)
		{
			//when a wall standing in front of the player, do not move;
			if (xInput == player.facingDirection && player.isWallDetected())
				return;
			stateMachine.ChangeState(player.moveState);
			player.FlipController(xInput);
		}

	}
}
