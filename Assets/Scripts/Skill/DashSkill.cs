using UnityEngine;

public class DashSkill : Skill
{
	[SerializeField] GameObject unlockDashButton;

	[SerializeField] bool createCloneOnStart;
	[SerializeField] GameObject unlockCreateCloneOnStartButton;
	[SerializeField] bool createCloneOnEnd;
	[SerializeField] GameObject unlockCreateCloneOnEndButton;

	public override void UseSkill()
	{
		if (CanUseSkill())
		{
			player.stateMachine.ChangeState(player.dashState);
			CoolDownTimer = skillCoolDownTime;
			Debug.Log(GetType() + " Used.");
		}

	}

	protected override void Awake()
	{
		base.Awake();
		if (unlockDashButton != null)
		{
			var buttonController = unlockDashButton.GetComponent<UISkillTreeSlotController>();
			void UnlockDashSkill()
			{
				Unlocked = buttonController.IsUnlocked();
			}
			buttonController.OnUnlockedChanged += UnlockDashSkill;
		}
		if (unlockCreateCloneOnStartButton != null)
		{
			var buttonController = unlockCreateCloneOnStartButton.GetComponent<UISkillTreeSlotController>();
			void UnlockCreateCloneOnStart()
			{
				createCloneOnStart = buttonController.IsUnlocked();
			}
			buttonController.OnUnlockedChanged += UnlockCreateCloneOnStart;
		}
		if (unlockCreateCloneOnEndButton != null)
		{
			var buttonController = unlockCreateCloneOnEndButton.GetComponent<UISkillTreeSlotController>();
			void UnlockCreateCloneOnEnd()
			{
				createCloneOnEnd = unlockCreateCloneOnEndButton.GetComponent<UISkillTreeSlotController>().IsUnlocked();
			}
			buttonController.OnUnlockedChanged += UnlockCreateCloneOnEnd;
		}
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

	public void CreateCloneOnStart()
	{
		if (createCloneOnStart)
		{
			player.skill.CloneSkill.UseSkill(player.transform, Vector2.zero);
		}
	}

	public void CreateCloneOnEnd()
	{
		if (createCloneOnEnd)
		{
			player.skill.CloneSkill.UseSkill(player.transform, Vector2.zero);
		}
	}

}
