using UnityEngine;

public class Skill : MonoBehaviour
{
	[Header("Skill Basic Info")]
	[SerializeField] protected BasicSkillData skillData;
	[SerializeField] protected float skillCoolDownTime;
	[SerializeField] protected KeyCode shortcut;

	public BasicSkillData data
	{
		get { return skillData; }
		protected set { skillData = value; }
	}

	protected PlayerController player;
	public float coolDownTimer { get; protected set; }

	protected virtual void Awake()
	{
		UpdateFromSkillData();
	}
	private void OnValidate()
	{
		UpdateFromSkillData();
	}

	protected virtual void UpdateFromSkillData()
	{
		if (this.skillData == null) return;
		this.skillCoolDownTime = skillData.skillCoolDownTime;
		this.shortcut = skillData.shortCut;
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
