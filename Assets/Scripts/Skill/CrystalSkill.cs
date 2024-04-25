using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
	[Header("Unlock Info")]
	[SerializeField] private UISkillTreeSlotController unlockCreateCrystalButton;
	//Create Clone Instead of Crystal
	public bool canMirageBlink;
	[SerializeField] private UISkillTreeSlotController unlockMirageBlinkButton;
	public bool canExplode;
	[SerializeField] private UISkillTreeSlotController unlockExplosiveCrystalButton;
	public bool canMoveToEnemy;
	[SerializeField] private UISkillTreeSlotController unlockHomingCrystalButton;
	public bool CanUseMultiStack;
	[SerializeField] private UISkillTreeSlotController unlockMultipleCrystalsButton;
	[Space]
	[Space]
	[SerializeField] private GameObject crystalPrefab;
	[SerializeField] private float crystalDuration;
	private GameObject crystal;
	[SerializeField] private List<GameObject> crystalStack;
	[SerializeField] private int stackSize = 1;

	protected override void Awake()
	{
		base.Awake();
		if (unlockCreateCrystalButton != null)
		{
			var buttonController = unlockCreateCrystalButton.GetComponent<UISkillTreeSlotController>();
			void UnlockCreateCrystal()
			{
				Unlocked = buttonController.IsUnlocked();
				if (Unlocked)
				{
					skillData.maxAvailableTimes = 1;
					OnAvailableTimesChanged?.Invoke(crystalStack.Count);
				}
			}
			buttonController.OnUnlockedChanged += UnlockCreateCrystal;
		}
		if (unlockMirageBlinkButton != null)
		{
			var buttonController = unlockMirageBlinkButton.GetComponent<UISkillTreeSlotController>();
			void UnlockMirageBlink()
			{
				canMirageBlink = buttonController.IsUnlocked();
				if (canMirageBlink)
				{
					skillData = unlockMirageBlinkButton.Skill;
					OnSkillUpdated?.Invoke(skillData as UpgradeSkillData);
				}
			}
			buttonController.OnUnlockedChanged += UnlockMirageBlink;
		}
		if (unlockExplosiveCrystalButton != null)
		{
			var buttonController = unlockExplosiveCrystalButton.GetComponent<UISkillTreeSlotController>();
			void UnlockExplosiveCrystal()
			{
				canExplode = buttonController.IsUnlocked();
				if (canExplode)
				{
					skillData = unlockExplosiveCrystalButton.Skill;
					OnSkillUpdated?.Invoke(skillData as UpgradeSkillData);
				}
			}
			buttonController.OnUnlockedChanged += UnlockExplosiveCrystal;
		}
		if (unlockHomingCrystalButton != null)
		{
			var buttonController = unlockHomingCrystalButton.GetComponent<UISkillTreeSlotController>();
			void UnlockHomingCrystal()
			{
				canMoveToEnemy = buttonController.IsUnlocked();
				if (canMoveToEnemy)
				{
					skillData = unlockHomingCrystalButton.Skill;
					OnSkillUpdated?.Invoke(skillData as UpgradeSkillData);
				}
			}
			buttonController.OnUnlockedChanged += UnlockHomingCrystal;
		}
		if (unlockMultipleCrystalsButton != null)
		{
			var buttonController = unlockMultipleCrystalsButton.GetComponent<UISkillTreeSlotController>();
			void UnlockMultipleCrystals()
			{
				CanUseMultiStack = buttonController.IsUnlocked();
				if (CanUseMultiStack)
				{
					skillData = unlockMultipleCrystalsButton.Skill;
					MaxAvailableTimes = stackSize = skillData.maxAvailableTimes;
					if (CoolDownTimer < 0) CoolDownTimer = skillCoolDownTime;
					OnSkillUpdated?.Invoke(skillData as UpgradeSkillData);
					OnAvailableTimesChanged?.Invoke(crystalStack.Count);
				}
			}
			buttonController.OnUnlockedChanged += UnlockMultipleCrystals;
		}
	}

	protected override void Start()
	{
		base.Start();
		CoolDownTimer = skillCoolDownTime;
	}
	public override bool CanUseSkill()
	{
		if (CanUseMultiStack && crystalStack.Count >= 1) return true;
		if (crystal != null) return true;
		return base.CanUseSkill();
	}

	public override void UseSkill()
	{
		if (!CanUseSkill()) return;
		else Debug.Log(GetType() + " Used.");

		GameObject tempCrystal = null;

		if (crystalStack.Count > 0)
		{
			tempCrystal = Instantiate(crystalStack[crystalStack.Count - 1], player.transform.position, Quaternion.identity);
			crystalStack.Remove(crystalStack[crystalStack.Count - 1]);
			base.OnAvailableTimesChanged?.Invoke(crystalStack.Count);
			tempCrystal.GetComponent<CrystalController>().SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, 0, null);
			if (CoolDownTimer < 0) CoolDownTimer = skillCoolDownTime;
		}

		if (CanUseMultiStack) return;

		if (crystal == null)
		{
			crystal = tempCrystal;
		}
		else
		{
			if (crystal.GetComponent<CrystalController>().isExploding || canMoveToEnemy) return;

			if (canMirageBlink)
			{
				//create a clone at the player's position,and the player will move to crystal's location, and then detroy the crystal.
				player.skill.CloneSkill.UseSkill(player.transform, Vector2.zero);
				player.transform.position = crystal.transform.position;
				crystal.GetComponent<CrystalController>().DestrotSelf();
			}
			else
			{
				//swap the position between player and crystal
				Vector2 temp = player.transform.position;
				player.transform.position = crystal.transform.position;
				crystal.transform.position = temp;
				crystal.GetComponent<CrystalController>().Explode();
			}
		}

	}

	protected override void Update()
	{
		stackSize = (!CanUseMultiStack || stackSize == 0) ? 1 : stackSize;
		//canMoveToEnemy = CanUseMultiStack ? true : canMoveToEnemy;

		if (crystalStack.Count < stackSize)
		{
			if (CoolDownTimer >= 0) CoolDownTimer -= Time.deltaTime;
			else
			{
				crystalStack.Add(crystalPrefab);
				base.OnAvailableTimesChanged?.Invoke(crystalStack.Count);
				if (CanUseMultiStack) CoolDownTimer = skillCoolDownTime;
			}
		}
		//To make cooldown UI works.
		if (stackSize == crystalStack.Count) CoolDownTimer = -0.1f;
		if (Input.GetKeyDown(KeyCode.F) && CanUseSkill())
		{
			UseSkill();
		}
	}

	//just for clone skill creating crystall instead of player clone in use.
	public float GetCrystalDuration() => crystalDuration;
}
