using UnityEngine;

public class Player : MonoBehaviour
{
	private Rigidbody2D rb;
	private Animator ani;

	private bool isFacingRight = true;
	private bool isGrounded = false;
	private float currentMoveSpeed;

	[Header("Movement Info")]
	[SerializeField] private float jumpForce = 10f;
	[SerializeField] private float moveSpeed = 450f;

	[Header("Check Ground Info")]
	[SerializeField] private float checkGroundDistance = 0.9f;
	[SerializeField] private LayerMask whatIsGround;
	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		ani = GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		CheckInput();
		CheckGround();
		Move();

		CheckAnimation();
	}

	private bool CheckGround()
	{
		return isGrounded = Physics2D.Raycast(transform.position, Vector2.down, checkGroundDistance, whatIsGround);
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - checkGroundDistance));
	}

	void Move()
	{
		currentMoveSpeed = Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;
		switch (Input.GetAxisRaw("Horizontal"))
		{
			case 1:
				{
					Flip(true);
					break;
				}
			case -1:
				{
					Flip(false);
					break;
				}
			default:
				{
					break;
				}
		}
		rb.velocity = new Vector2(currentMoveSpeed, rb.velocity.y);
	}

	void Jump()
	{

		if (isGrounded)

			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
	}

	private void CheckAnimation()
	{
		ani.SetBool("isMoving", currentMoveSpeed != 0);

		ani.SetBool("isGrounded", isGrounded);
		ani.SetFloat("yVelocity", rb.velocity.y);
	}

	void Flip(bool facingRight)
	{
		if (facingRight == isFacingRight) return;
		else
		{
			isFacingRight = facingRight;
		}
		transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, facingRight ? 0 : 180, 0));
	}

	void CheckInput()
	{
		if (Input.GetKey(KeyCode.Space)) Jump();
	}
}

