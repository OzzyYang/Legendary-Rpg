using UnityEngine;

public class CharacterController : MonoBehaviour
{

	public Animator animator { get; private set; }
	public Rigidbody2D rb { get; private set; }

	public int facingDirection { get; private set; } = 1;
	private bool isFacingRight = true;


	[Header("Collision Check Info")]
	[SerializeField] protected Transform groundCheck;
	[SerializeField] protected float groundCheckDistance = 0.1f;
	[SerializeField] protected Transform wallCheck;
	[SerializeField] protected float wallCheckDistance = 0.1f;
	[SerializeField] protected LayerMask whatIsGround;



	// Start is called before the first frame update
	protected virtual void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponentInChildren<Animator>();
		if (groundCheck == null) groundCheck = transform;
		if (wallCheck == null) wallCheck = transform;
	}

	// Update is called once per frame
	protected virtual void Update()
	{
	}

	public bool isGroundedDetected()
	{
		return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround) && (rb.velocity.y == 0);
	}

	public bool isWallDectected()
	{
		return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
	}
	protected virtual void OnDrawGizmos()
	{
		Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
		Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
	}

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

	protected virtual void Attack() { }


	public virtual void SetVelocity(float _xVelocity, float _yVelocity)
	{
		rb.velocity = new Vector2(_xVelocity, _yVelocity);
	}



}
