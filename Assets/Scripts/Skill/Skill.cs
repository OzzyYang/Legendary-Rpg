using UnityEngine;

public class Skill : MonoBehaviour
{
	[SerializeField] protected float skillCoolDownTime;
	protected PlayerController player;
	protected float coolDownTimer;

	protected virtual void Awake()
	{

	}
	// Start is called before the first frame update
	protected virtual void Start()
	{
		player = PlayerManager.instance.player.GetComponent<PlayerController>();
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		if (coolDownTimer >= 0)
			coolDownTimer -= Time.deltaTime;
	}

	public virtual bool CanUseSkill()
	{
		if (coolDownTimer <= 0)
		{
			coolDownTimer = skillCoolDownTime;
			UseSkill();
			return true;
		}

		return false;
	}

	public virtual void UseSkill()
	{
		Debug.Log("Skill Used.");
	}
}
