using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
	protected PlayerStateMachine stateMachine;
	protected PlayerController player;

	protected string animBoolName;
	protected float xInput;
	
	protected Rigidbody2D rb;

	public PlayerState(PlayerController _player, PlayerStateMachine _stateMachine,string _animBoolName)
	{
		this.player = _player;
		this.stateMachine = _stateMachine;
		this.animBoolName = _animBoolName;
	}

	public virtual void Enter()
	{
		player.animator.SetBool(animBoolName, true);
		rb = player.rb;
	}

	public virtual void Update()
	{
		xInput = Input.GetAxis("Horizontal");
		player.animator.SetBool("isLevitating", !player.isGroundedDetected());
		player.animator.SetBool("isGrounded", player.isGroundedDetected());
	}

	public virtual void Exit()
	{
		player.animator.SetBool(animBoolName, false);

	}
}
