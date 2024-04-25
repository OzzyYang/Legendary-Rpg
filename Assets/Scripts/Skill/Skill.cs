using System;
using UnityEngine;

public class Skill : MonoBehaviour
{
	[Header("Skill Basic Info")]
	[SerializeField] protected BasicSkillData skillData;
	[SerializeField] protected float skillCoolDownTime;
	[SerializeField] protected KeyCode shortcut;
	[SerializeField] private bool unlocked;
	public bool Unlocked
	{
		get { return unlocked; }
		protected set { unlocked = value; }
	}
	public Action<int> OnAvailableTimesChanged { get; set; }
	public Action<UpgradeSkillData> OnSkillUpdated { get; set; }
	public int AvailableTimes { get; protected set; }
	public int MaxAvailableTimes { get; protected set; }
	public BasicSkillData Data
	{
		get { return skillData; }
		protected set { skillData = value; }
	}

	protected PlayerController player;
	public float CoolDownTimer { get; protected set; }

	protected virtual void Awake()
	{
		Debug.Log(GetType());
		UpdateFromSkillData();
		player = PlayerManager.Instance.Player;
	}
	private void OnValidate()
	{
		UpdateFromSkillData();
	}

	protected virtual void UpdateFromSkillData()
	{
		if (skillData == null) return;
		skillCoolDownTime = skillData.skillCoolDownTime;
		shortcut = skillData.shortCut;
		MaxAvailableTimes = skillData.maxAvailableTimes;
	}

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
		if (CoolDownTimer >= 0) CoolDownTimer -= Time.deltaTime;
	}

	public virtual bool CanUseSkill()
	{
		if (!Unlocked)
		{
			Debug.Log(GetType() + " needs to be unlocked.");
			return false;
		}
		if (CoolDownTimer > 0)
		{
			Debug.Log(GetType() + " is on cooldown.");
			return false;
		}
		return true;
	}

	public virtual void UseSkill()
	{
		if (CanUseSkill())
		{
			CoolDownTimer = skillCoolDownTime;
			Debug.Log(GetType() + " Used.");
		}
	}

	public virtual void LockSkill() => Unlocked = false;


	public virtual void UnlockSkill() => Unlocked = true;

	public virtual void SetSkillUnlocked(bool unlocked)
	{
		if (unlocked) UnlockSkill();
		else LockSkill();
	}

	protected virtual void InitializeSkill() => UpdateFromSkillData();

}
