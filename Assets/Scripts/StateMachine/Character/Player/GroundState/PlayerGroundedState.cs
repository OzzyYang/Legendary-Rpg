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
			stateMachine.ChangeState(player.levitateState);

		if (Input.GetKeyDown(KeyCode.Space) && player.isGroundedDetected())
			stateMachine.ChangeState(player.jumpState);

	}
}
