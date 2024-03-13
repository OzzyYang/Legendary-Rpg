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
	[Header("Battle Info")]
	public float battleTime;
	public float attackCheckDistance;
	[SerializeField] protected Transform attackCheck;
	[SerializeField] protected float attackCheckRadius;
	public float attackCooldownTime;
	[HideInInspector] public float lastAttackTime;

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

	public virtual void AnimationTriggerCalled()
	{
		stateMachine.currentState.AnimationFnishedTrigger();
	}

	public virtual void AttackTriggerCalled()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
		foreach (var hit in colliders)
		{
			PlayerController player = hit.GetComponent<PlayerController>();
			if (player != null)
			{
				player.Damage();
			}
		}
	}
	public virtual void OpenCounterAttackWindown()
	{
		canBeStunned = true;
		counterImage.SetActive(true);
	}
	public virtual void CloseCounterAttackWindown()
	{
		canBeStunned = false;
		counterImage.SetActive(false);
	}

	public virtual bool CanBeStunned()
	{
		if (canBeStunned)
		{
			CloseCounterAttackWindown();
			return true;
		}
		return false;
	}

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
	}

}
