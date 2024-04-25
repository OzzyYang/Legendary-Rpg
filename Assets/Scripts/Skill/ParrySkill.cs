using System.Collections;
using UnityEngine;

public class ParrySkill : Skill
{
	[SerializeField] private UISkillTreeSlotController unlockParryButton;
	[SerializeField] private bool canRestoreOnParry;
	[SerializeField] private float restoreAmount;
	[SerializeField] private UISkillTreeSlotController unlockRestoreOnParryButton;
	[SerializeField] private bool canCreateCloneOnParry;
	[SerializeField] private UISkillTreeSlotController unlockCreateCloneOnParryButton;

	public override void UseSkill()
	{
		if (CanUseSkill())
		{
			base.UseSkill();
			player.stateMachine.ChangeState(player.counterAttackState);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (unlockParryButton != null)
		{
			var buttonController = unlockParryButton.GetComponent<UISkillTreeSlotController>();
			void UnlockParry()
			{
				Unlocked = unlockParryButton.IsUnlocked();
			}
			buttonController.OnUnlockedChanged += UnlockParry;
		}
		if (unlockRestoreOnParryButton != null)
		{
			var buttonController = unlockRestoreOnParryButton.GetComponent<UISkillTreeSlotController>();
			void UnlockRestoreOnParry()
			{
				canRestoreOnParry = unlockRestoreOnParryButton.IsUnlocked();
			}
			buttonController.OnUnlockedChanged += UnlockRestoreOnParry;
		}
		if (unlockCreateCloneOnParryButton != null)
		{
			var buttonController = unlockCreateCloneOnParryButton.GetComponent<UISkillTreeSlotController>();
			void UnlockCreateCloneOnParry()
			{
				canCreateCloneOnParry = unlockCreateCloneOnParryButton.IsUnlocked();
			}
			buttonController.OnUnlockedChanged += UnlockCreateCloneOnParry;
		}
		player.OnCounterAttackSuccessful += RestoreOnCounter;
		player.OnCounterAttackSuccessful += CreateCloneOnParry;
	}

	protected override void Start()
	{
		base.Start();
	}

	private void RestoreOnCounter(Transform enemyTarget)
	{
		if (canRestoreOnParry)
			player.GetComponent<CharacterStats>().IncreseHealth(restoreAmount, GetType().ToString());
	}

	protected override void Update()
	{
		base.Update();
	}

	public void CreateCloneOnParry(Transform enemyTarget)
	{
		if (canCreateCloneOnParry)
			StartCoroutine(nameof(CreateCloneDelayFor), enemyTarget);
	}

	private IEnumerator CreateCloneDelayFor(Transform _target)
	{
		yield return new WaitForSeconds(0.5f);
		player.skill.CloneSkill.UseSkill(_target, new Vector2(1.5f * player.facingDirection, 0));
	}
}
