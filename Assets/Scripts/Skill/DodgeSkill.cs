using UnityEngine;

public class DodgeSkill : Skill
{

	[Range(0f, 1f)]
	[SerializeField] private float increaseByPercentage;
	[SerializeField] private UISkillTreeSlotController unlockDodgeButton;

	public bool canCreateCloneOnDodge;
	[SerializeField] private UISkillTreeSlotController unlockCreateCloneOnDodgeButton;

	public override bool CanUseSkill()
	{
		return base.CanUseSkill() && canCreateCloneOnDodge;
	}

	public override void UseSkill()
	{
		if (this.CanUseSkill())
		{
			player.skill.CloneSkill.UseSkill(player.transform, Vector3.zero);
			base.UseSkill();
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (unlockDodgeButton != null)
		{
			var buttonController = unlockDodgeButton.GetComponent<UISkillTreeSlotController>();
			void UnlockDodge()
			{
				Unlocked = unlockDodgeButton.IsUnlocked();
				if (Unlocked) IncreaseEvasionRateByPercentage();
			}
			buttonController.OnUnlockedChanged += UnlockDodge;
		}
		if (unlockCreateCloneOnDodgeButton != null)
		{
			var buttonController = unlockCreateCloneOnDodgeButton.GetComponent<UISkillTreeSlotController>();
			void UnlockCreateCloneOnDodge()
			{
				canCreateCloneOnDodge = unlockCreateCloneOnDodgeButton.IsUnlocked();
			}
			buttonController.OnUnlockedChanged += UnlockCreateCloneOnDodge;
		}
		player.GetComponent<PlayerStats>().OnBaseEvasionRateChanged += IncreaseEvasionRateByPercentage;
		player.GetComponent<PlayerStats>().OnAvoidAttack += UseSkill;
	}

	protected override void Start()
	{
		base.Start();
	}

	private void IncreaseEvasionRateByPercentage()
	{
		if (Unlocked)
		{
			var playerStats = player.GetComponent<PlayerStats>();
			playerStats.evasionRate.SetDefaultValue(playerStats.evasionRate.GetValue() * (1 + increaseByPercentage));
		}
	}

	protected override void Update()
	{
		base.Update();
	}


}
