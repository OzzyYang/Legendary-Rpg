using UnityEngine;

public class SkeletonController : EnemyController
{
	#region State Info
	public SkeletonMoveState moveState { get; protected set; }
	public SkeletonBattleState battleState { get; protected set; }
	public SkeletonAttackState attackState { get; protected set; }
	#endregion

	#region Enemy Info
	[Header("Attack Info")]
	public float attackDistance;
	[SerializeField] private Transform attackCheck;
	public float attackCooldownTime;
	[HideInInspector] public float lastAttackTime;

	#endregion
	protected override void Awake()
	{
		base.Awake();
		idleState = new SkeletonIdleState(this, stateMachine, "isIdling");
		moveState = new SkeletonMoveState(this, stateMachine, "isMoving");
		battleState = new SkeletonBattleState(this, stateMachine, "isMoving");
		attackState = new SkeletonAttackState(this, stateMachine, "isAttacking");
		stateMachine.Initialize(idleState);
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(attackCheck.position, new Vector3(attackCheck.position.x + attackDistance * facingDirection, attackCheck.position.y));
	}

	public bool CanAttack() => Time.time - lastAttackTime >= attackCooldownTime;
}

