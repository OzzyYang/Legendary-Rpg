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
		Transform enemy = FindClosestEnemyIn(attackCheck.position, 25);

		if (enemy == null)
		{
			transform.rotation = PlayerManager.instance.player.transform.rotation;
		}
		else if (enemy.position.x < transform.position.x)
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
		needToFadeAway = true;
	}

}
