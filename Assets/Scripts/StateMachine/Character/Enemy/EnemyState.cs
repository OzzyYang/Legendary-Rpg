using UnityEngine;

public class EnemyState
{
	protected EnemyController enemy;
	protected EnemyStateMachine stateMachine;
	protected Rigidbody2D rb;
	protected Animator animator;
	protected string animBoolName;

	//public virtual float test { get; private set; }

	protected float StateTime;
	protected float stateTimer;
	protected bool isTriggerCalled;

	protected bool needFreeze;

	private readonly bool needDebug = false;

	public EnemyState(EnemyController _enemy, EnemyStateMachine _stateMachine, string _animBoolName)
	{
		this.enemy = _enemy;
		this.stateMachine = _stateMachine;
		this.animBoolName = _animBoolName;

	}

	public virtual void Enter()
	{
		animator = enemy.animator;
		rb = enemy.rb;
		animator.SetBool(animBoolName, true);
		isTriggerCalled = false;
		stateTimer = StateTime;
	}

	public virtual void Exit()
	{
		animator.SetBool(animBoolName, false);
		stateTimer = 0;
	}

	public virtual void Update()
	{
		if (needFreeze) return;
		if (stateTimer >= 0) stateTimer -= Time.deltaTime;
	}


	public virtual void AnimationFnishedTrigger()
	{
		isTriggerCalled = true;
	}

	public virtual void FreezeState(bool _needFreeze)
	{
		this.needFreeze = _needFreeze;
	}

}
