using UnityEngine;

public class CloneObjectController : MonoBehaviour
{
	protected SpriteRenderer sR;
	[SerializeField] protected float loosingSpeed;
	[SerializeField] protected Animator animator;

	protected float timer;

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

		if (timer <= 0)
		{
			sR.color = new Color(sR.color.r, sR.color.g, sR.color.b, sR.color.a - (Time.deltaTime) * loosingSpeed);
		}

		if (sR.color.a <= 0)
			Destroy(this.gameObject);
	}

	public virtual void SetUpClone(Transform _newTransform, Vector3 _offSet, float _cloneObjectDuration)
	{
		transform.position = _newTransform.position + _offSet;
		this.timer = _cloneObjectDuration;
	}
}
