using UnityEngine;

public class EnemyState : CharacterState
{
	protected EnemyController enemy;
	protected Animator animator;

	protected bool needFreeze;

	private readonly bool needDebug = false;



	public EnemyState(EnemyController _enemy, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
	{
		this.character = this.enemy = _enemy;
		this.stateMachine = _stateMachine;
		this.animBoolName = _animBoolName;
	}

	public override void Enter()
	{
		base.Enter();
		animator = enemy.animator;
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		if (needFreeze) return;
		if (stateTimer >= 0) stateTimer -= Time.deltaTime;
	}

	public void FreezeState(bool _needFreeze)
	{
		this.needFreeze = _needFreeze;
	}

}
