using UnityEngine;

public class LightningController : MonoBehaviour
{
	[SerializeField] private float lightningDamage;

	[SerializeField] private Animator animator;

	private void Awake()
	{

	}
	private void Start()
	{
	}
	public void TakeDamage()
	{
		Collider2D[] colliders = Physics2D.OverlapBoxAll(animator.transform.position, animator.GetComponent<BoxCollider2D>().bounds.size, 0);
		foreach (var hit in colliders)
		{
			if (hit.GetComponent<EnemyController>() != null)
			{
				EnemyController enemy = hit.GetComponent<EnemyController>();
				CharacterStats enemyStats = hit.GetComponent<CharacterStats>();
				enemyStats.ReduceHealth(PlayerManager.Instance.Player.GetComponent<CharacterStats
					>().lightningDamge.GetValue(), this.name);
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(animator.transform.position, animator.GetComponent<BoxCollider2D>().bounds.size);
	}

	public void DestroySelf()
	{
		Destroy(gameObject);
	}

	public void SetupDamage(float _damage)
	{
		this.lightningDamage = _damage;
	}
}
