using UnityEngine;

public class SwordController : MonoBehaviour
{
	[Header("Sword Info")]
	[SerializeField] private Animator animator;
	private Rigidbody2D rb;
	private PlayerController player;
	private Vector2 throwDirection;
	private Vector2 throwPosition;
	private float throwForce = 10f;



	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		player = PlayerManager.instance.player;
		transform.position = throwPosition;
		rb.velocity = throwDirection.normalized * throwForce;

	}

	// Update is called once per frame
	void Update()
	{

		if (rb.velocity.y <= 0)
		{
			animator.SetBool("isIdling", true);
			//rb.velocity = new Vector2(0, 0);
		}

	}

	public void SetupSword(Vector2 _direction, Vector2 _position, float _force)
	{
		throwPosition = _position;
		throwDirection = _direction;
		throwForce = _force;
	}

}
