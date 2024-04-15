using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalSkill : Skill
{
	[Header("Unlock Info")]
	public bool canCreateCrystal;
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
		if (this.unlockCreateCrystalButton != null)
		{
			this.unlockCreateCrystalButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.canCreateCrystal = this.unlockCreateCrystalButton.IsUnlocked();
				Debug.Log(1);
			});
		}
		if (this.unlockMirageBlinkButton != null)
		{
			this.unlockMirageBlinkButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.canMirageBlink = this.unlockMirageBlinkButton.IsUnlocked();
				Debug.Log(2);
			});
		}
		if (this.unlockExplosiveCrystalButton != null)
		{
			this.unlockExplosiveCrystalButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.canExplode = this.unlockExplosiveCrystalButton.IsUnlocked();
			});
		}
		if (this.unlockExplosiveCrystalButton != null)
		{
			this.unlockExplosiveCrystalButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.canExplode = this.unlockExplosiveCrystalButton.IsUnlocked();
			});
		}
		if (this.unlockHomingCrystalButton != null)
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
			});
		}
	}
	public override bool CanUseSkill()
	{
		if (!canCreateCrystal) return false;
		if (crystalStack.Count > 0 || crystal != null)
		{
			return true;
		}
		return false;
	}

	public override void UseSkill()
	{
		base.UseSkill();

		GameObject tempCrystal = null;

		if (crystalStack.Count > 0)
		{
			tempCrystal = Instantiate(crystalStack[crystalStack.Count - 1], player.transform.position, Quaternion.identity);
			crystalStack.Remove(crystalStack[crystalStack.Count - 1]);
			tempCrystal.GetComponent<CrystalController>().SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, 0, null);
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
				player.skill.cloneSkill.CreateClone(player.transform, Vector2.zero);
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

		if (coolDownTimer < 0)
		{
			coolDownTimer = skillCoolDownTime;
			if (crystalStack.Count < stackSize)
				crystalStack.Add(crystalPrefab);
		}
		if (Input.GetKeyDown(KeyCode.F) && CanUseSkill())
		{
			this.UseSkill();
		}
	}

	//just for clone skill create crystall instead player clone in use.
	public float getCrystalDuration() => this.crystalDuration;
}
