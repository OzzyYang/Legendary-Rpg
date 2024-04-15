using UnityEngine;

public class Skill : MonoBehaviour
{
	[SerializeField] protected float skillCoolDownTime;
	protected PlayerController player;
	protected float coolDownTimer;

	protected virtual void Awake()
	{

	}

	protected virtual void Start()
	{
		player = PlayerManager.instance.player.GetComponent<PlayerController>();
	}

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
			return true;
		}

		return false;
	}

	public virtual void UseSkill()
	{
		Debug.Log(this.GetType() + " Used.");
	}
}
