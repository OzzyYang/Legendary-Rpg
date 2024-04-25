using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour, ISaveManager
{
	public static SkillManager instance;
	public PlayerController player;
	public DashSkill DashSkill { get; private set; }
	public MirageSkill CloneSkill { get; private set; }
	public SwordSkill SwordSkill { get; private set; }
	public ParrySkill ParrySkill { get; private set; }
	public CrystalSkill CrystalSkill { get; private set; }
	public BlackHoleSkill BlackHoleSkill { get; private set; }
	public DodgeSkill DodgeSkill { get; private set; }
	public Dictionary<BasicSkillData, Skill> SkillList { get; private set; }

	private void Awake()
	{
		if (instance != null)
			Destroy(instance.gameObject);
		else
		{
			instance = this;
			Debug.Log(GetType());
			SkillList = new Dictionary<BasicSkillData, Skill>();
			DashSkill = GetComponent<DashSkill>();
			CloneSkill = GetComponent<MirageSkill>();
			SwordSkill = GetComponent<SwordSkill>();
			ParrySkill = GetComponent<ParrySkill>();
			CrystalSkill = GetComponent<CrystalSkill>();
			DodgeSkill = GetComponent<DodgeSkill>();
			BlackHoleSkill = GetComponent<BlackHoleSkill>();
			SkillList.Add(DashSkill.Data, DashSkill);
			SkillList.Add(BlackHoleSkill.Data, BlackHoleSkill);
			SkillList.Add(CrystalSkill.Data, CrystalSkill);

		}
	}

	//private List<BasicSkillData> GetAllSkillData()
	//{
	//	var result = new List<BasicSkillData>();

	//	var allSkillDataId = AssetDatabase.FindAssets("", new[] { "Assets/Scripts/Skill/SkillData" });

	//	if (allSkillDataId != null && allSkillDataId.Length > 0)
	//	{
	//		foreach (var skillId in allSkillDataId)
	//		{
	//			var path = AssetDatabase.GUIDToAssetPath(skillId);
	//			var skillData = AssetDatabase.LoadAssetAtPath<BasicSkillData>(path);
	//			result.Add(skillData);
	//		}
	//	}
	//	return result;
	//}

	public void LoadData(GameData data)
	{
		//foreach (var skillDataPair in data.skills)
		//{

		//}
		Debug.Log("Skill data loaded!");
	}

	public void SaveData(ref GameData data)
	{
		Debug.Log("Skill data saved!");
	}
}
