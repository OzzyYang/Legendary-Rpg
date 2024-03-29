using System.Collections;
using UnityEngine;

public class PlayerController : CharacterController
{

	#region State machine info
	public PlayerGroundedState groundedState { get; private set; }
	public PlayerIdleState idleState { get; private set; }
	public PlayerMoveState moveState { get; private set; }
	public PlayerDashState dashState { get; private set; }
	public PlayerJumpState jumpState { get; private set; }
	public PlayerLevitationState levitateState { get; private set; }
	public PlayerWallSlideState wallSlideState { get; private set; }
	public PlayerWallJumpState wallJumpState { get; private set; }
	public PlayerAttackState attackState { get; private set; }
	public PlayerCounterAttackState counterAttackState { get; private set; }

	public PlayerAimSwordState aimSwordState { get; private set; }
	public PlayerThrowSwordState throwSwordState { get; private set; }
	public PlayerCatchSwordState catchSwordState { get; private set; }
	public PlayerBlackHoleState blackHoleState { get; private set; }
	//public PlayerDeadState dyingState { get; private set; }
	#endregion

	#region Skill Info
	public SkillManager skill { get; private set; }
	public GameObject sword { get; set; }
	#endregion

	#region Player info
	public bool isBusy { get; private set; }
	public Vector2 initialPos { get; private set; }
	[Header("Attack Info")]
	//make character attacking more lively
	public Vector2[] attackMovement;
	[SerializeField] public float attackCheckRadius;
	[SerializeField] public Transform attackCheck;
	public float counterAttackDuration = 1f;
	[Header("Movement info")]
	[SerializeField] private float defaultPlayerSpeed = 8f;
	[SerializeField] private float defaultJumpForce = 19f;
	[SerializeField] private float defaultDashSpeed = 25f;
	[SerializeField] private float defaultDashDuration = 0.2f;
	public float playerSpeed { get; private set; }
	public float jumpForce { get; private set; }
	public float dashSpeed { get; private set; }
	public float dashDuration { get; private set; }

	public float dashDirection { get; private set; } = 1;
	#endregion

	protected override void Awake()
	{
		base.Awake();
		stateMachine = new PlayerStateMachine();
		groundedState = new PlayerGroundedState(this, stateMachine as PlayerStateMachine, "isGrounded");
		idleState = new PlayerIdleState(this, stateMachine as PlayerStateMachine, "isIdling");
		moveState = new PlayerMoveState(this, stateMachine as PlayerStateMachine, "isMoving");
		dashState = new PlayerDashState(this, stateMachine as PlayerStateMachine, "isDashing");
		jumpState = new PlayerJumpState(this, stateMachine as PlayerStateMachine, "isLevitating");
		levitateState = new PlayerLevitationState(this, stateMachine as PlayerStateMachine, "isLevitating");
		wallSlideState = new PlayerWallSlideState(this, stateMachine as PlayerStateMachine, "isWallSliding");
		wallJumpState = new PlayerWallJumpState(this, stateMachine as PlayerStateMachine, "isLevitating");
		attackState = new PlayerAttackState(this, stateMachine as PlayerStateMachine, "isAttacking");
		counterAttackState = new PlayerCounterAttackState(this, stateMachine as PlayerStateMachine, "isCounterAttacking");
		aimSwordState = new PlayerAimSwordState(this, stateMachine as PlayerStateMachine, "isSwordAiming");
		throwSwordState = new PlayerThrowSwordState(this, stateMachine as PlayerStateMachine, "isSwordThrowing");
		catchSwordState = new PlayerCatchSwordState(this, stateMachine as PlayerStateMachine, "isSwordCatching");
		blackHoleState = new PlayerBlackHoleState(this, stateMachine as PlayerStateMachine, "isLevitating");
		dyingState = new PlayerDyingState(this, stateMachine as PlayerStateMachine, "isDying");
	}

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		skill = SkillManager.instance;
		stateMachine.Initialize(idleState);
		initialPos = transform.position;
		RevertSlow();
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
		stateMachine.currentState.Update();
		CheckForInput();
	}

	private void CheckForInput()
	{
		//check dash input
		if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dashSkill.CanUseSkill())
		{
			if (isWallDetected()) return;
			DashDirectionController(Input.GetAxisRaw("Horizontal"));
			stateMachine.ChangeState(dashState);
		}

		if (Input.GetKeyDown(KeyCode.R))
			transform.position = initialPos;

	}

	public IEnumerator BusyFor(float _seconds)
	{
		isBusy = true;
		yield return new WaitForSeconds(_seconds);
		isBusy = false;
	}

	public void AnimationTrrier() => stateMachine.currentState.AnimationFinishTrigger();

	public virtual void AttackTriggerCalled()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
		foreach (var hit in colliders)
		{
			EnemyController enemy = hit.GetComponent<EnemyController>();
			if (enemy != null)

			{
				this.stats.DoDamage(enemy.stats);
				foreach (var item in InventoryManager.instance.GetEquipmentItemsList())
				{
					if (item.itemData.haveEffect) item.itemData.ExecuteEffect(hit.transform);
				}
			}
		}
	}

	public void DashDirectionController(float _xInput)
	{
		if (_xInput == 0)
		{
			dashDirection = facingDirection;
			return;
		}
		dashDirection = _xInput > 0 ? 1 : -1;
	}

	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
	}

	public override void SlowCharacter()
	{
		base.SlowCharacter();
		playerSpeed = defaultPlayerSpeed * 0.7f;
		dashDuration = defaultDashDuration * 0.7f;
		dashSpeed = defaultDashSpeed * 0.7f;
		jumpForce = defaultJumpForce * 0.7f;
	}

	public override void RevertSlow()
	{
		base.RevertSlow();
		playerSpeed = defaultPlayerSpeed;
		dashDuration = defaultDashDuration;
		dashSpeed = defaultDashSpeed;
		jumpForce = defaultJumpForce;
	}

	public override void PickupItem(Object item)
	{
		base.PickupItem(item);

		ItemData itemData = item as ItemData;
		InventoryManager.instance.AddItem(itemData);
	}
}

