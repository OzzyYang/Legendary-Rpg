using UnityEngine;
using UnityEngine.UI;

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
			player.skill.cloneSkill.UseSkill(player.transform, Vector3.zero);
			base.UseSkill();
		}
	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
		IncreaseEvasionRateByPercentage();
		player.GetComponent<PlayerStats>().OnBaseEvasionRateChanged += this.IncreaseEvasionRateByPercentage;
		player.GetComponent<PlayerStats>().OnAvoidAttack += this.UseSkill;
		if (this.unlockDodgeButton != null)
		{
			this.unlockDodgeButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.unlocked = this.unlockDodgeButton.IsUnlocked();
				this.IncreaseEvasionRateByPercentage();
			});
		}
		if (this.unlockCreateCloneOnDodgeButton != null)
		{
			this.unlockCreateCloneOnDodgeButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.canCreateCloneOnDodge = this.unlockCreateCloneOnDodgeButton.IsUnlocked();
			});
		}
	}

	private void IncreaseEvasionRateByPercentage()
	{
		if (unlocked)
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
