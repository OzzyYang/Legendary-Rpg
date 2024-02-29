using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
	public PlayerMoveState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

		player.SetVelocity(xInput * player.playerSpeed, rb.velocity.y);
		if (Input.GetAxis("Horizontal") == 0)
		{
			stateMachine.ChangeState(player.idleState);
		}
	}
}
