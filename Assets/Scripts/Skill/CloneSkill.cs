using UnityEngine;

public class CloneSkill : Skill
{
	[SerializeField] private GameObject playerCloneObject;
	[SerializeField] private GameObject crystalObject;
	[SerializeField] private float cloneDuration;

	[SerializeField] private bool canDuplicate;
	[SerializeField] private float duplicateProbability;

	[SerializeField] private bool createCrystalInsteadClone;
	[SerializeField] private int ultimateSkillCreateNum;
	private int currentUltimateSkillCreateNum;
	private int createTimes = 1;
	public override bool CanUseSkill()
	{
		return base.CanUseSkill();
	}

	public override void UseSkill()
	{
		base.UseSkill();
	}

	public void CreateClone(Transform _newTransform, Vector3 _offSet)
	{
		if (createCrystalInsteadClone)
		{
			this.currentUltimateSkillCreateNum += ultimateSkillCreateNum;
			InvokeRepeating(nameof(CreateCrystal), 0, 0.1f);
			//int times = player.skill.blackHoleSkill.isReleasingSkill ? ultimateSkillCreateNum : 1;

			//GameObject newClone = Instantiate(crystalObject, player.transform.position, Quaternion.identity);
			//newClone.GetComponent<CrystalController>().SetupCrystal(player.skill.crystalSkill.getCrystalDuration(), true, true, 0, _newTransform);
		}
		else
		{
			GameObject newClone = Instantiate(playerCloneObject);
			newClone.GetComponent<PlayerCloneController>().SetUpClone(_newTransform, _offSet, cloneDuration, canDuplicate, duplicateProbability);
		}

	}

	private void CreateCrystal()
	{
		Debug.Log(this.createTimes);
		GameObject newClone = Instantiate(crystalObject, player.transform.position, Quaternion.identity);
		newClone.GetComponent<CrystalController>().SetupCrystal(player.skill.crystalSkill.getCrystalDuration(), true, true, player.skill.blackHoleSkill.isReleasingSkill ? 1 : 0, null);
		this.createTimes++;
		if (this.createTimes > (player.skill.blackHoleSkill.isReleasingSkill ? currentUltimateSkillCreateNum : 1))
		{
			CancelInvoke(nameof(CreateCrystal));
			this.createTimes = 1;
			this.currentUltimateSkillCreateNum = 0;
		}
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

}
