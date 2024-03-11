using UnityEngine;

public class CharacterController : MonoBehaviour
{

	public Animator animator { get; private set; }
	public Rigidbody2D rb { get; private set; }

	protected virtual void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		//Debug.Log(animator.ToString());
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
		rb.velocity = new Vector2(_xVelocity, _yVelocity);
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
