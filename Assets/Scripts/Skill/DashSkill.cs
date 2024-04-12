using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
	[SerializeField] bool canDash;
	[SerializeField] GameObject unlockDashButton;

	[SerializeField] bool createCloneOnStart;
	[SerializeField] GameObject unlockCreateCloneOnStartButton;
	[SerializeField] bool createCloneOnEnd;
	[SerializeField] GameObject unlockCreateCloneOnEndButton;

	public override bool CanUseSkill() => base.CanUseSkill() && this.canDash;

	protected override void Awake()
	{
		base.Awake();
		if (unlockDashButton != null)
			unlockDashButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.canDash = unlockDashButton.GetComponent<UISkillTreeSlotController>().IsUnlocked();
			});
		if (unlockCreateCloneOnStartButton != null)
			unlockCreateCloneOnStartButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.createCloneOnStart = unlockCreateCloneOnStartButton.GetComponent<UISkillTreeSlotController>().IsUnlocked();
			});
		if (unlockCreateCloneOnEndButton != null)
			unlockCreateCloneOnEndButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.createCloneOnEnd = unlockCreateCloneOnEndButton.GetComponent<UISkillTreeSlotController>().IsUnlocked();
			});
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
			player.skill.cloneSkill.CreateClone(player.transform, Vector2.zero);
		}
	}

	public void CreateCloneOnEnd()
	{
		if (createCloneOnEnd)
		{
			player.skill.cloneSkill.CreateClone(player.transform, Vector2.zero);
		}
	}
}
