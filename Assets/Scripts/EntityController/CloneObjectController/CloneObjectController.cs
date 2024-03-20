using UnityEngine;

public class CloneObjectController : MonoBehaviour
{
	protected SpriteRenderer sR;
	[SerializeField] protected float loosingSpeed;
	[SerializeField] protected Animator animator;

	protected float timer;
	protected bool needToFadeAway;

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

	public virtual void SetUpClone(Transform _newTransform, Vector3 _offSet, float _cloneObjectDuration)
	{
		transform.position = _newTransform.position + _offSet;
		this.timer = _cloneObjectDuration;
	}

	public void DestrotSelf() => Destroy(gameObject);
}
