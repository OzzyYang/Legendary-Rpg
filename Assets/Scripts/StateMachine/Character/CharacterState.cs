using UnityEngine;

public class CharacterState
{
	protected CharacterController character;
	protected CharacterStateMachine stateMachine;
	public string animBoolName { get; protected set; }
	//protected string animBoolName;

	protected Rigidbody2D rb;

	protected float StateTime;
	protected float stateTimer;
	protected float stateDuration;


	protected bool isTriggerCalled;
	public CharacterState(CharacterController _character, CharacterStateMachine _stateMachine, string _animBoolName)
	{
		this.character = _character;
		this.stateMachine = _stateMachine;
		this.animBoolName = _animBoolName;
	}

	public virtual void Enter()
	{
		character.animator.SetBool(animBoolName, true);
		rb = character.rb;
		isTriggerCalled = false;
		stateTimer = StateTime;
	}

	public virtual void Update()
	{
		StateTimerController();
	}

	private void StateTimerController()
	{
		if (stateTimer >= 0)
			stateTimer -= Time.deltaTime;
	}

	public virtual void Exit()
	{
		character.animator.SetBool(animBoolName, false);
		stateTimer = 0;
	}

	public virtual void AnimationFinishTrigger()
	{
		isTriggerCalled = true;
	}
}
