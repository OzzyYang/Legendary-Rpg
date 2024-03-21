using UnityEngine;
public class DashSkill : Skill
{

	[SerializeField] bool createCloneStart;
	[SerializeField] bool createCloneOver;
	public override bool CanUseSkill()
	{
		return base.CanUseSkill();
	}

	public override void UseSkill()
	{

	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

	public void CreateCloneStart()
	{
		if (createCloneStart)
		{
			player.skill.cloneSkill.CreateClone(player.transform, Vector2.zero);
		}
	}

	public void CreateCloneOver()
	{
		if (createCloneOver)
		{
			player.skill.cloneSkill.CreateClone(player.transform, Vector2.zero);
		}
	}
}
