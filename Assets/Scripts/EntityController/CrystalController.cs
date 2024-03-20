using UnityEngine;

public class CrystalController : CloneObjectController
{
	private CircleCollider2D crystalCollider;

	private Vector3 originLocalScale;
	[SerializeField] private float explodeMaxScale;
	[SerializeField] private AnimationCurve easeCurve;

	private float crytalDuration;

	public bool isExploding { get; private set; }

	private bool canExplode;
	private float explodeDuration = 1F;
	private float explodeTimer;
	private float explodePercentage;

	private bool canMoveToEnemy;
	private Transform enemyTarget;
	private float moveSpeed = 4.0F;

	// Start is called before the first frame update
	protected override void Start()
	{
		animator = GetComponent<Animator>();
		originLocalScale = transform.localScale;
		crystalCollider = GetComponent<CircleCollider2D>();
	}

	// Update is called once per frame
	protected override void Update()
	{
		if (timer >= 0) timer -= Time.deltaTime;
		if (timer < 0 && !isExploding && !canMoveToEnemy) Explode();

		if (isExploding)
		{
			explodePercentage = explodeTimer / explodeDuration;
			explodePercentage = easeCurve.Evaluate(explodePercentage);
			transform.localScale = Vector2.Lerp(originLocalScale, originLocalScale * explodeMaxScale, explodePercentage);
			explodeTimer += Time.deltaTime;
			if (explodePercentage > 1) isExploding = false;
		}

		if (canMoveToEnemy && enemyTarget != null)
		{
			transform.position = Vector2.MoveTowards(transform.position, enemyTarget.position, moveSpeed * Time.deltaTime);
			if (Vector2.Distance(transform.position, enemyTarget.position) < 1.2f)
			{
				Explode();
			}
		}

	}

	public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMoveToEnemy)
	{
		this.timer = this.crytalDuration = _crystalDuration;
		this.canExplode = _canExplode;
		this.canMoveToEnemy = _canMoveToEnemy;

		if (canMoveToEnemy)
		{
			enemyTarget = FindClosestEnemyIn(transform.position, 20);
		}
	}

	public void Explode()
	{
		if (!canExplode)
		{
			DestrotSelf();
			return;
		}
		animator.SetBool("needExplode", true);
		isExploding = true;
	}


	private void ExplosionDamge()
	{
		if (!canExplode) return;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, crystalCollider.radius);
		foreach (var hit in colliders)
		{
			if (hit.GetComponent<EnemyController>() != null)
			{
				hit.GetComponent<EnemyController>().Damage();
			}
		}
	}


}
