using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
	[SerializeField] private bool canParry;
	[SerializeField] private UISkillTreeSlotController unlockParryButton;
	[SerializeField] private bool canRestoreOnParry;
	[SerializeField] private float restoreAmount;
	[SerializeField] private UISkillTreeSlotController unlockRestoreOnParryButton;
	[SerializeField] private bool canCreateCloneOnParry;
	[SerializeField] private UISkillTreeSlotController unlockCreateCloneOnParryButton;

	public override bool CanUseSkill() => base.CanUseSkill() && this.canParry;

	public override void UseSkill()
	{
		base.UseSkill();
		player.stateMachine.ChangeState(player.counterAttackState);
	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
		if (this.unlockParryButton != null)
		{
			this.unlockParryButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.canParry = this.unlockParryButton.IsUnlocked();
			});
		}
		if (this.unlockRestoreOnParryButton != null)
		{
			this.unlockRestoreOnParryButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.canRestoreOnParry = this.unlockRestoreOnParryButton.IsUnlocked();
			});
		}
		if (this.unlockCreateCloneOnParryButton != null)
		{
			this.unlockCreateCloneOnParryButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.canCreateCloneOnParry = this.unlockCreateCloneOnParryButton.IsUnlocked();
			});
		}
		player.OnCounterAttackSuccessful += this.RestoreOnCounter;
		player.OnCounterAttackSuccessful += this.CreateCloneOnParry;
		//player.OnCounterAttackSuccessful.Invoke
	}

	private void RestoreOnCounter(Transform enemyTarget)
	{
		if (this.canRestoreOnParry)
			player.GetComponent<CharacterStats>().IncreseHealth(this.restoreAmount, this.GetType().ToString());
	}

	protected override void Update()
	{
		base.Update();
	}

	public void CreateCloneOnParry(Transform enemyTarget)
	{
		if (this.canCreateCloneOnParry)
			StartCoroutine("CreateCloneDelayFor", enemyTarget);
	}

	public void CreateCloneOnCounter(Transform _target)
	{
		//if (canCreateCloneOnParry)
		//{
		//	StartCoroutine("CreateCloneDelayFor", _target);
		//}
	}

	private IEnumerator CreateCloneDelayFor(Transform _target)
	{
		yield return new WaitForSeconds(0.5f);
		player.skill.cloneSkill.UseSkill(_target, new Vector2(1.5f * player.facingDirection, 0));
	}
}
