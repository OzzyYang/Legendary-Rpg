using UnityEngine;

public class SkillManager : MonoBehaviour
{
	public static SkillManager instance;
	public PlayerController player;
	public DashSkill dashSkill { get; private set; }
	public CloneSkill cloneSkill { get; private set; }
	public SwordSkill swordSkill { get; private set; }
	public ParrySkill parrySkill { get; private set; }
	public CrystalSkill crystalSkill { get; private set; }
	public BlackHoleSkill blackHoleSkill { get; private set; }
	public DodgeSkill dodgeSkill { get; private set; }
	private void Awake()
	{
		if (instance != null)
			Destroy(instance.gameObject);
		else
			instance = this;

		dashSkill = GetComponent<DashSkill>();
		cloneSkill = GetComponent<CloneSkill>();
		swordSkill = GetComponent<SwordSkill>();
		parrySkill = GetComponent<ParrySkill>();
		crystalSkill = GetComponent<CrystalSkill>();
		blackHoleSkill = GetComponent<BlackHoleSkill>();
		dodgeSkill = GetComponent<DodgeSkill>();
	}
}
