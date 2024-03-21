using UnityEngine;

public class CrystalController : CloneObjectController
{
	private CircleCollider2D crystalCollider;

	private Vector3 originLocalScale;
	[SerializeField] private float explodeMaxScale;
	[SerializeField] private AnimationCurve easeCurve;

	private float crytalDuration;

	#region Explode Info
	public bool isExploding { get; private set; }
	private bool canExplode;
	private float explodeDuration = 1F;
	private float explodeTimer;
	private float explodePercentage;
	#endregion

	#region Move Info
	private bool canMoveToEnemy;
	private Transform enemyTarget;
	private float moveSpeed = 4.0F;
	#endregion



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
		//Additionally the crystal  can move toward enemy, but if there are not any enemy nearby,  the crystal will explode automatically.
		if (timer < 0 && !isExploding && (!canMoveToEnemy || enemyTarget == null)) Explode();

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

	public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMoveToEnemy, int _searchStrategy, Transform target)
	{
		this.timer = this.crytalDuration = _crystalDuration;
		this.canExplode = _canExplode;
		this.canMoveToEnemy = _canMoveToEnemy;


		switch (_searchStrategy)
		{

			case 0:// search closest enemy 
				{
					enemyTarget = FindClosestEnemyIn(transform.position, 20);
					break;
				}

			case 1:// search enemy randomly
				{
					GameObject blackHole = SkillManager.instance.blackHoleSkill.blackHole;
					enemyTarget = FindEnemyRandomlyIn(transform.position, blackHole.GetComponent<CircleCollider2D>().bounds.size.x / 2);
					break;
				}
			case 2:// choose given target
				{

					enemyTarget = target;
					break;
				}
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
