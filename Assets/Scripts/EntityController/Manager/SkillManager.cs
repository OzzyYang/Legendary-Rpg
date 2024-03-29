using UnityEngine;

public class SkillManager : MonoBehaviour
{
	public static SkillManager instance;
	public PlayerController player;
	public DashSkill dashSkill { get; private set; }
	public CloneSkill cloneSkill { get; private set; }
	public SwordSkill swordSkill { get; private set; }
	public CounterAttackSkill counterAttackSkill { get; private set; }
	public CrystalSkill crystalSkill { get; private set; }
	public BlackHoleSkill blackHoleSkill { get; private set; }
	private void Awake()
	{
		if (instance != null)
			Destroy(instance.gameObject);
		else
			instance = this;

		dashSkill = GetComponent<DashSkill>();
		cloneSkill = GetComponent<CloneSkill>();
		swordSkill = GetComponent<SwordSkill>();
		counterAttackSkill = GetComponent<CounterAttackSkill>();
		crystalSkill = GetComponent<CrystalSkill>();
		blackHoleSkill = GetComponent<BlackHoleSkill>();
	}
}
