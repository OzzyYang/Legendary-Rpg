using UnityEngine;

public class EnemyController : CharacterController
{
	#region State Info
	public EnemyState idleState { get; protected set; }
	public EnemyStateMachine stateMachine { get; protected set; }
	#endregion

	#region Enemy Info
	[Header("Movement Info")]
	public float moveSpeed = 4.0f;
	#endregion

	protected override void Awake()
	{
		base.Awake();
		stateMachine = new EnemyStateMachine();

	}
	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
		stateMachine.currentState.Update();
	}

	public virtual void TriggerCalled()
	{
		stateMachine.currentState.AnimationFnishedTrigger();
	}
}
