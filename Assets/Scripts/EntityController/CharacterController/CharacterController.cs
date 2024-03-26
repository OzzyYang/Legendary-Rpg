using System.Collections;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
	public CharacterStateMachine stateMachine { get; protected set; }
	public Animator animator { get; private set; }
	public Rigidbody2D rb { get; private set; }
	public CharacterStats state { get; private set; }

	public CharacterState dyingState { get; protected set; }


	[Header("KnockBack Info")]
	[SerializeField] protected Vector2 knockBackMovement;
	[SerializeField] protected float knockBackDuration;
	protected bool isKnockBacking = false;

	[Header("Stunned Info")]
	[SerializeField] public Vector2 stunnedMovement;
	[SerializeField] public float stunnedDuration;
	[SerializeField] protected GameObject counterImage;
	protected bool canBeStunned;

	public System.Action onFlipped;
	protected virtual void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		state = GetComponent<CharacterStats>();
		if (groundCheck == null) groundCheck = transform;
		if (wallCheck == null) wallCheck = transform;
		if (playerCheck == null) playerCheck = transform;
	}

	// Start is called before the first frame update
	protected virtual void Start()
	{

	}

	// Update is called once per frame
	protected virtual void Update()
	{
	}
	protected virtual IEnumerator HitKnockBack()
	{
		//TODO: kncokback direction should be opposite to hit direction 
		SetVelocity(knockBackMovement.x * -facingDirection, knockBackMovement.y);
		isKnockBacking = true;
		yield return new WaitForSeconds(knockBackDuration);
		isKnockBacking = false;
		SetVelocity(0, rb.velocity.y);
	}

	public virtual void playDamageEffect()
	{
		EntityFX fX = GetComponentInChildren<EntityFX>();
		fX.StartCoroutine(nameof(fX.FlashFX));
		//fX.StartCoroutine(nameof)
		StartCoroutine(nameof(HitKnockBack));
	}

	public virtual void BeDead()
	{
		stateMachine.ChangeState(this.dyingState);
	}

	public virtual IEnumerator SlowCharacterFor(float _seconds)
	{
		SlowCharacter();
		yield return new WaitForSeconds(_seconds);
		RevertSlow();
	}


	public virtual void SlowCharacter()
	{
		animator.speed = 0.7f;
	}

	public virtual void RevertSlow()
	{
		animator.speed = 1;
	}

	#region Collision Check
	[Header("Collision Check Info")]
	[SerializeField] protected Transform groundCheck;
	[SerializeField] protected float groundCheckDistance = 0.1f;
	[SerializeField] protected Transform wallCheck;
	[SerializeField] protected float wallCheckDistance = 0.1f;
	[SerializeField] protected LayerMask whatIsGround;
	[SerializeField] protected float playerCheckDistance;
	[SerializeField] protected Transform playerCheck;
	[SerializeField] protected LayerMask whatIsPlayer;

	public bool isGroundedDetected()
	{
		return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
	}

	public bool isWallDetected()
	{
		return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
	}
	public RaycastHit2D isPlayerDetected()
	{
		return Physics2D.Raycast(playerCheck.position, Vector2.right * facingDirection, playerCheckDistance, whatIsPlayer);
	}
	protected virtual void OnDrawGizmos()
	{
		Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
		Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));
		Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + playerCheckDistance * facingDirection, playerCheck.position.y));
	}
	#endregion

	#region Flip Controller
	public int facingDirection { get; private set; } = 1;
	private bool isFacingRight = true;
	public void Flip()
	{
		facingDirection *= -1;
		isFacingRight = !isFacingRight;
		transform.Rotate(0, 180, 0);
		if (onFlipped != null) onFlipped();
	}

	public void FlipController(float _xVelocity)
	{
		if ((_xVelocity > 0 && !isFacingRight) || (_xVelocity < 0 && isFacingRight))
			Flip();
	}
	#endregion

	#region Physical Settings
	public virtual void SetVelocity(float _xVelocity, float _yVelocity)
	{
		if (isKnockBacking) return;
		rb.velocity = new Vector2(_xVelocity, _yVelocity);
		//Debug.Log(_xVelocity + " " + _yVelocity);
	}

	public virtual void SetPosition(float _xPos, float _yPos)
	{
		rb.position = new Vector2(_xPos, _yPos);
	}
	#endregion

	#region Debug Mode
#if DEBUG
	public void ShowInfo(string _info)
	{
		Debug.Log(_info);
	}
#endif
	#endregion
}
