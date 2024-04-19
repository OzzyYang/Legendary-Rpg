using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	}
	protected override void Start()
	{
		base.Start();
		coolDownTimer = skillCoolDownTime;
		if (unlockCreateCrystalButton != null)
		{
			unlockCreateCrystalButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				SetSkillUnlocked(unlockCreateCrystalButton.IsUnlocked());
				if (unlocked)
				{
					skillData.maxAvailableTimes = 1;
					OnAvailableTimesChanged?.Invoke(crystalStack.Count);
				}
			});
		}
		if (unlockMirageBlinkButton != null)
		{
			unlockMirageBlinkButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				canMirageBlink = unlockMirageBlinkButton.IsUnlocked();

			});
		}
		if (unlockExplosiveCrystalButton != null)
		{
			unlockExplosiveCrystalButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				canExplode = unlockExplosiveCrystalButton.IsUnlocked();
			});
		}
		if (unlockExplosiveCrystalButton != null)
		{
			unlockExplosiveCrystalButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				canExplode = unlockExplosiveCrystalButton.IsUnlocked();
			});
		}
		if (unlockHomingCrystalButton != null)
		{
			this.unlockHomingCrystalButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.canMoveToEnemy = this.unlockHomingCrystalButton.IsUnlocked();
			});
		}
		if (this.unlockMultipleCrystalsButton != null)
		{
			this.unlockMultipleCrystalsButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.CanUseMultiStack = this.unlockMultipleCrystalsButton.IsUnlocked();
				this.stackSize = 3;
				this.skillData.maxAvailableTimes = 3;
				if (coolDownTimer < 0) coolDownTimer = skillCoolDownTime;
				base.OnAvailableTimesChanged?.Invoke(crystalStack.Count);
			});
		}
	}
	public override bool CanUseSkill()
	{
		if (CanUseMultiStack && crystalStack.Count >= 1) return true;
		if (base.CanUseSkill()) return (crystalStack.Count > 0 || crystal != null);
		else return false;
	}

	public override void UseSkill()
	{
		base.UseSkill();

		GameObject tempCrystal = null;

		if (crystalStack.Count > 0)
		{
			tempCrystal = Instantiate(crystalStack[crystalStack.Count - 1], player.transform.position, Quaternion.identity);
			crystalStack.Remove(crystalStack[crystalStack.Count - 1]);
			base.OnAvailableTimesChanged?.Invoke(crystalStack.Count);
			tempCrystal.GetComponent<CrystalController>().SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, 0, null);
			if (coolDownTimer < 0) coolDownTimer = skillCoolDownTime;
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
				player.skill.cloneSkill.UseSkill(player.transform, Vector2.zero);
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
		canMoveToEnemy = CanUseMultiStack ? true : canMoveToEnemy;

		if (coolDownTimer >= 0 && crystalStack.Count < stackSize)
			coolDownTimer -= Time.deltaTime;

		if (coolDownTimer < 0 && crystalStack.Count < stackSize)
		{
			crystalStack.Add(crystalPrefab);
			base.OnAvailableTimesChanged?.Invoke(crystalStack.Count);
			if (CanUseMultiStack) coolDownTimer = skillCoolDownTime;
		}
		//To make cooldown UI works.
		if (stackSize == crystalStack.Count) coolDownTimer = -0.1f;
		if (Input.GetKeyDown(KeyCode.F) && CanUseSkill())
		{
			this.UseSkill();
		}
	}

	//just for clone skill creating crystall instead of player clone in use.
	public float getCrystalDuration() => this.crystalDuration;
}
