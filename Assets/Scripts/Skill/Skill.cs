using System;
using UnityEngine;

public class Skill : MonoBehaviour
{
	[Header("Skill Basic Info")]
	[SerializeField] protected BasicSkillData skillData;
	[SerializeField] protected float skillCoolDownTime;
	[SerializeField] protected KeyCode shortcut;

	public bool unlocked { get; protected set; }
	public Action<int> OnAvailableTimesChanged { get; set; }
	public int availableTimes { get; protected set; }
	public int maxAvailableTimes { get; protected set; }
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
		this.unlocked = skillData.unlocked;
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
		if (!unlocked)
		{
			Debug.Log(this.GetType() + " needs to be unlocked.");
			return false;
		}
		if (coolDownTimer > 0)
		{
			Debug.Log(this.GetType() + " is on cooldown.");
			return false;
		}
		return true;
	}

	public virtual void UseSkill()
	{
		if (CanUseSkill())
		{
			coolDownTimer = skillCoolDownTime;
			Debug.Log(this.GetType() + " Used.");
		}
	}

	public virtual void LockSkill()
	{
		this.unlocked = skillData.unlocked = false;
	}

	public virtual void UnlockSkill()
	{
		this.unlocked = skillData.unlocked = true;
	}

	public virtual void SetSkillUnlocked(bool unlocked)
	{
		if (unlocked) UnlockSkill();
		else LockSkill();
	}

	protected virtual void InitializeSkill()
	{
		this.UpdateFromSkillData();
	}
}
