using System.Collections.Generic;
using UnityEngine;

public class CloneObjectController : MonoBehaviour
{
	protected SpriteRenderer sR;
	[SerializeField] protected float loosingSpeed;
	[SerializeField] protected Animator animator;

	protected float timer;
	protected bool needToFadeAway;
	protected bool canDuplicate;
	protected float duplicateProbability;

	protected int facingDirection;

	protected virtual void Awake()
	{
		sR = GetComponentInChildren<SpriteRenderer>();
		animator = GetComponentInChildren<Animator>();
	}
	// Start is called before the first frame update
	protected virtual void Start()
	{

	}

	// Update is called once per frame
	protected virtual void Update()
	{
		timer -= Time.deltaTime;

		if (needToFadeAway) FadeAway();
	}

	protected virtual void FadeAway()
	{
		sR.color = new Color(sR.color.r, sR.color.g, sR.color.b, sR.color.a - (Time.deltaTime) * loosingSpeed);

		if (sR.color.a <= 0)
			DestrotSelf();
	}

	protected virtual Transform FindClosestEnemyIn(Vector3 _checkPosition, float _radius)
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkPosition, _radius);
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
		return enemy?.transform;
	}

	protected virtual Transform FindEnemyRandomlyIn(Vector3 _checkPosition, float _radius)
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkPosition, _radius);
		List<Transform> enemiesList = new List<Transform>();
		foreach (var hit in colliders)
		{
			if (hit.GetComponent<EnemyController>() != null) enemiesList.Add(hit.transform);
		}

		return enemiesList.Count > 0 ? enemiesList[Random.Range(0, enemiesList.Count)].transform : null;
	}

	public virtual void SetUpClone(Transform _newTransform, Vector3 _offSet, float _cloneObjectDuration, bool _canDuplicate, float _duplicateProbability)
	{
		transform.position = _newTransform.position + _offSet;
		this.timer = _cloneObjectDuration;
		this.canDuplicate = _canDuplicate;
		this.duplicateProbability = _duplicateProbability;
	}

	public void DestrotSelf() => Destroy(gameObject);
}
