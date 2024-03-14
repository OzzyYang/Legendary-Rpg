using UnityEngine;

public class PlayerCloneController : CloneObjectController
{
	[SerializeField] protected Transform attackCheck;
	[SerializeField] protected float attackCheckRadius;
	protected override void Awake()
	{
		base.Awake();
		animator.SetInteger("attackCounter", Random.Range(1, 4));
	}

	protected override void Start()
	{
		base.Start();
		FaceToEnemy();
	}

	protected override void Update()
	{
		base.Update();


	}

	protected void FaceToEnemy()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, 25);
		Collider2D enemy = null;
		foreach (var hit in colliders)
		{
			if (hit.GetComponent<EnemyController>() == null) { continue; }
			if (enemy == null)
			{
				enemy = hit;
			}
			else
			{
				enemy = Vector2.Distance(transform.position, enemy.transform.position) <= Vector2.Distance(transform.position, hit.transform.position) ? enemy : hit;
			}
		}

		if (enemy == null)
		{
			transform.rotation = PlayerManager.instance.player.transform.rotation;
		}
		else if (enemy.transform.position.x < transform.position.x)
		{
			transform.Rotate(0, 180, 0);
		}
	}

	protected void AttackTrigger()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
		foreach (var hit in colliders)
		{
			EnemyController enemy = hit.GetComponent<EnemyController>();
			if (enemy != null)
			{
				enemy.Damage();
			}
		}
	}

	private void AnimationTrigger()
	{
		timer = 0;
	}

}
