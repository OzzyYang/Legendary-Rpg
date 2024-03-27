using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
	[Header("Sword Info")]
	[SerializeField] private Animator animator;
	private Rigidbody2D rb;
	private PlayerController player;
	private SwordType swordType;
	private Vector2 throwDirection;
	private Vector2 throwPosition;
	private float throwForce = 10f;

	private bool isFlying = true;
	private bool isReturning = false;


	#region Bounce Info
	private bool isBouncing = false;
	private int targetIndex = 0;
	private List<Transform> targetEnemies = new List<Transform>();
	private int bounceCounter = 0;
	private int bounceTimes = 0;
	#endregion

	#region Pierce Info
	private int pierceTimes;
	private int pierceCounter;
	#endregion

	#region Spin Info
	private float maxMoveDistance;
	private bool isSpinning;
	private bool isStopped;
	private float spinDamageTimer;
	private float spinningDamageTime = 0.5f;
	#endregion
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		player = PlayerManager.instance.player;
	}

	// Start is called before the first frame update
	void Start()
	{


	}

	// Update is called once per frame
	void Update()
	{
		if (isReturning)
		{
			ReturnToPlayer();

		}

		if (isBouncing && targetEnemies.Count > 0)
		{
			BounceToEnemies();
		}

		if (isSpinning)
		{
			if (Vector2.Distance(transform.position, player.transform.position) >= maxMoveDistance && !isStopped)
			{
				rb.constraints = RigidbodyConstraints2D.FreezeAll;
				isStopped = true;
			}
			if (isStopped)
			{
				if (spinDamageTimer <= 0) spinDamageTimer = spinningDamageTime;
				if (spinDamageTimer == spinningDamageTime)
				{
					Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
					foreach (var hit in colliders)
					{
						//hit.GetComponent<EnemyController>()?.playDamageEffect();
						if (hit.GetComponent<EnemyController>() != null)
						{
							player.GetComponent<CharacterStats>().DoDamage(hit.GetComponent<CharacterStats>());
						}
					}
				}
				spinDamageTimer -= Time.deltaTime;
			}
		}

		if (isFlying)
		{
			transform.right = rb.velocity;
		}
	}

	private void ReturnToPlayer()
	{
		rb.constraints = RigidbodyConstraints2D.None;
		FlyTo(player.transform.position);
		if (Vector2.Distance(transform.position, player.transform.position) < 1)
		{
			isReturning = false;
			isFlying = false;

			//Reset sword physical properties
			rb.bodyType = RigidbodyType2D.Dynamic;
			rb.simulated = true;
			transform.parent = null;
			this.gameObject.SetActive(false);

			player.FlipController(this.transform.position.x >= player.transform.position.x ? 1 : -1);
			player.stateMachine.ChangeState(player.catchSwordState);
		}
	}

	private void BounceToEnemies()
	{
		if (bounceCounter > bounceTimes - 1)
		{
			isBouncing = false;
			bounceCounter = 0;
			targetEnemies.Clear();
			isReturning = true;
			return;
		}
		FlyTo(targetEnemies[targetIndex].position);
		if (Vector2.Distance(transform.position, targetEnemies[targetIndex].position) < 0.5f)
		{
			//Debug.Log(targetEnemies.Count + " " + bounceCounter + " " + targetIndex);
			player.GetComponent<CharacterStats>().DoDamage(targetEnemies[targetIndex].GetComponent<CharacterStats>());
			targetIndex++;
			bounceCounter++;
		}
		if (targetIndex >= targetEnemies.Count)
		{
			targetIndex = 0;
		}
	}

	public void SetupSword(Vector2 _direction, Vector2 _position, float _force, SwordType _swordType)
	{
		this.gameObject.SetActive(true);

		throwPosition = _position;
		throwDirection = _direction;
		throwForce = _force;
		swordType = _swordType;
		transform.position = throwPosition;
		rb.velocity = throwDirection.normalized * throwForce;

		isFlying = true;
		animator.SetBool("isIdling", false);
	}

	public void SetupBounce(float _bounceSpeedPercentage, int _bounceTimes)
	{
		this.throwForce *= _bounceSpeedPercentage;
		this.bounceTimes = _bounceTimes;
	}
	public void SetupPierce(float _pierceSpeedPercentage, int _pierceTimes)
	{
		this.throwForce *= _pierceSpeedPercentage;
		this.pierceCounter = this.pierceTimes = _pierceTimes;
	}
	public void SetupSpin(float _spinForcePercentage, float _maxMoveDistance, float _spinningDamageTime)
	{
		this.throwForce *= _spinForcePercentage;
		this.maxMoveDistance = _maxMoveDistance;
		this.spinningDamageTime = _spinningDamageTime;
		isSpinning = true;
		isStopped = false;
	}
	public void SetupRegular()
	{
		isSpinning = false;
		pierceTimes = 0;
		bounceTimes = 1;
	}

	public void NeedReturn(float _flyVelocity)
	{
		if (isBouncing) return;
		isReturning = true;
		isFlying = true;
		isSpinning = false;
		rb.bodyType = RigidbodyType2D.Kinematic;
		rb.simulated = true;
		transform.parent = null;
		throwForce = _flyVelocity;
		animator.SetBool("isIdling", true);
	}

	private void FlyTo(Vector2 _destination)
	{

		Vector2 direction = (Vector3)_destination - transform.position;
		rb.velocity = direction.normalized * throwForce;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!isReturning && collision.GetComponent<EnemyController>() != null)
		{
			//do single damage when isn't Spin Sword
			//Spin Sword have  area damage and implemented in Updates()
			if (!isSpinning) player.GetComponent<CharacterStats>().DoDamage(collision.GetComponent<CharacterStats>());


			if (isSpinning && !isStopped)
			{
				rb.constraints = RigidbodyConstraints2D.FreezeAll;
				isStopped = true;
			}

			if (pierceTimes != 0) pierceCounter = pierceCounter <= 0 ? 0 : --pierceCounter;
			else pierceCounter = 0;

			DetectEnemiesWhenBouncing(collision);
		}

		if (isFlying && !isReturning && (pierceCounter == 0 || collision.GetComponent<EnemyController>() == null))
		{
			StuckInto(collision);
		}
	}

	private void DetectEnemiesWhenBouncing(Collider2D collision)
	{

		if (targetEnemies.Count != 0 || swordType != SwordType.Bounce) return;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 20);
		foreach (var hit in colliders)
		{
			if (hit.GetComponent<EnemyController>() != null)
			{
				targetEnemies.Add(hit.transform);
			}
		}
		if (targetEnemies.Count > 1)
		{
			isBouncing = true;
			bounceCounter = 0;
			targetIndex = 0;
		}
		else
		{
			isBouncing = false;
			isReturning = true;
			targetEnemies.Clear();
		}
	}

	private void StuckInto(Collider2D collision)
	{
		rb.bodyType = RigidbodyType2D.Kinematic;
		if (isBouncing || isSpinning) return;
		rb.simulated = false;
		transform.parent = collision.transform;
		isFlying = false;
		animator.SetBool("isIdling", true);
	}


}
