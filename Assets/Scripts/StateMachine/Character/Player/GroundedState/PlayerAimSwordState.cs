using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
	public PlayerAimSwordState(PlayerController _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void AnimationFinishTrigger()
	{
		base.AnimationFinishTrigger();
	}

	public override void Enter()
	{
		base.Enter();
	}

	public override void Exit()
	{
		base.Exit();
		player.skill.swordSkill.NeedAimLine(false);
	}

	public override void Update()
	{
		base.Update();
		if (Input.GetKeyUp(KeyCode.Mouse1))
		{
			stateMachine.ChangeState(player.throwSwordState);
			return;
		}
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		player.FlipController((mousePosition.x - player.transform.position.x) >= 0 ? 1 : -1);
		player.skill.swordSkill.NeedAimLine(true);
	}
}
