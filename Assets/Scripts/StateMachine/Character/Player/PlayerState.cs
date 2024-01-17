using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
	protected PlayerStateMachine stateMachine;
	protected PlayerController player;

	private string animBoolName;

	public PlayerState(PlayerController _player, PlayerStateMachine _stateMachine,string _animBoolName)
	{
		this.player = _player;
		this.stateMachine = _stateMachine;
		this.animBoolName = _animBoolName;
	}

	public virtual void Enter()
	{
		Debug.Log("I enter " + animBoolName);
	}

	public virtual void Update()
	{
		Debug.Log("I am " + animBoolName);
	}

	public virtual void Exit()
	{
		Debug.Log("I exit " + animBoolName);
	}
}
