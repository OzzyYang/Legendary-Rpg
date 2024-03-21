using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
	[SerializeField] private GameObject crystalPrefab;
	[SerializeField] private float crystalDuration;

	[SerializeField] private bool createCloneInsteadCrystal;

	[SerializeField] private bool canExplode;
	[SerializeField] private bool canMoveToEnemy;

	private GameObject crystal;

	[SerializeField] private bool CanUseMultiStack;
	[SerializeField] private List<GameObject> crystalStack;
	[SerializeField] private int stackSize = 1;
	public override bool CanUseSkill()
	{
		if (crystalStack.Count > 0 || crystal != null)
		{
			UseSkill();
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

			if (createCloneInsteadCrystal)
			{
				//create a clone at the player's position,then player will move to crystal's location, and then detroy the crystal.
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
		//crystal.GetComponent<CrystalController>().SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, 0);

	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
		//crystalStack = new List<GameObject>();
		coolDownTimer = skillCoolDownTime;

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
		if (Input.GetKeyDown(KeyCode.F))
		{
			CanUseSkill();
		}
	}

	//just for clone skill create crystall instead player clone in use.
	public float getCrystalDuration() => this.crystalDuration;
}
