using UnityEngine;

public class EnemyState
{
	protected EnemyController enemy;
	protected EnemyStateMachine stateMachine;
	protected Rigidbody2D rb;
	protected Animator animator;
	protected string animBoolName;

	protected float stateTimer;
	protected bool isTriggerCalled;

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
	}

	public virtual void Exit()
	{
		animator.SetBool(animBoolName, false);
		stateTimer = 0;
	}

	public virtual void Update()
	{
		if (stateTimer >= 0) stateTimer -= Time.deltaTime;
	}


	public virtual void AnimationFnishedTrigger()
	{
		isTriggerCalled = true;
	}

}
