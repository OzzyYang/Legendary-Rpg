using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
	public static SkillManager instance;
	public PlayerController player;
	public DashSkill dashSkill { get; private set; }
	public MirageSkill cloneSkill { get; private set; }
	public SwordSkill swordSkill { get; private set; }
	public ParrySkill parrySkill { get; private set; }
	public CrystalSkill crystalSkill { get; private set; }
	public BlackHoleSkill blackHoleSkill { get; private set; }
	public DodgeSkill dodgeSkill { get; private set; }

	public Dictionary<BasicSkillData, Skill> skillList { get; private set; }
	private void Awake()
	{
		if (instance != null)
			Destroy(instance.gameObject);
		else
		{
			instance = this;
			skillList = new Dictionary<BasicSkillData, Skill>();
			dashSkill = GetComponent<DashSkill>();
			cloneSkill = GetComponent<MirageSkill>();
			swordSkill = GetComponent<SwordSkill>();
			parrySkill = GetComponent<ParrySkill>();
			crystalSkill = GetComponent<CrystalSkill>();
			dodgeSkill = GetComponent<DodgeSkill>();
			blackHoleSkill = GetComponent<BlackHoleSkill>();
			skillList.Add(dashSkill.data, dashSkill);
			skillList.Add(blackHoleSkill.data, blackHoleSkill);
			skillList.Add(crystalSkill.data, crystalSkill);

		}

	}
	private void Update()
	{

	}
}
