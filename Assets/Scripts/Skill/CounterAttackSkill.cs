using System.Collections;
using UnityEngine;
public class CounterAttackSkill : Skill
{
	[SerializeField] private bool createCloneWhenSuccessful;
	public override bool CanUseSkill()
	{
		return base.CanUseSkill();
	}

	public override void UseSkill()
	{
		base.UseSkill();
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

	public void CreateCloneOver(Transform _target)
	{
		if (createCloneWhenSuccessful)
		{
			StartCoroutine("CreateCloneDelayFor", _target);
		}
	}

	private IEnumerator CreateCloneDelayFor(Transform _target)
	{
		yield return new WaitForSeconds(0.5f);
		player.skill.cloneSkill.CreateClone(_target, new Vector2(1.5f * player.facingDirection, 0));
	}
}
