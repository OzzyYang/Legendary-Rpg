using UnityEngine;

public class PlayerCloneController : CloneObjectController
{
	[SerializeField] protected Transform attackCheck;
	[SerializeField] protected float attackCheckRadius;

	private float damageMultiplier;
	private bool applyWeaponEffect;
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
			transform.rotation = PlayerManager.Instance.Player.transform.rotation;
		}
		else if (enemy.position.x < transform.position.x)
		{
			transform.Rotate(0, 180, 0);
		}

		facingDirection = transform.rotation.eulerAngles.y == 0 ? 1 : -1;
	}

	protected void AttackTrigger()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
		foreach (var hit in colliders)
		{
			EnemyController enemy = hit.GetComponent<EnemyController>();
			if (enemy != null)
			{
				CharacterStats playerStats = PlayerManager.Instance.Player.GetComponent<CharacterStats>();
				playerStats.DoDamageWithMultiplier(enemy.GetComponent<CharacterStats>(), damageMultiplier);
				if (this.applyWeaponEffect)
				{
					foreach (var item in InventoryManager.Instance.GetEquipmentItemsList())
					{
						if (item.itemData.haveEffect)
						{
							item.itemData.ExecuteNegativeEffect(hit.transform);
							playerStats.DoMagicalDamage(enemy.stats);
						}
					}
				}
				if (Random.Range(0, 1.0f) < duplicateProbability && canDuplicate)
					SkillManager.instance.cloneSkill.UseSkill(enemy.transform, new Vector2(.5f * facingDirection, 0));
			}
		}
	}

	private void AnimationTrigger()
	{
		needToFadeAway = true;
	}

	public void SetUpClone(Transform _newTransform, Vector3 _offSet, float _cloneObjectDuration, bool _canDuplicate, float _duplicateProbability, float damageMultiplier, bool applyWeaponEffect)
	{
		base.SetUpClone(_newTransform, _offSet, _cloneObjectDuration, _canDuplicate, _duplicateProbability);
		this.damageMultiplier = damageMultiplier;
		this.applyWeaponEffect = applyWeaponEffect;
	}
}
