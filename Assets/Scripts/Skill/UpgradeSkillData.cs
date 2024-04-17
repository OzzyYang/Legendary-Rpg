using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Skill Data", menuName = "Data/Upgrade Skill")]
public class UpgradeSkillData : BasicSkillData
{
	public BasicSkillData baseSkill;

	private void OnValidate()
	{
		if (baseSkill == null) return;
		this.skillCoolDownTime = baseSkill.skillCoolDownTime;
		this.shortCut = baseSkill.shortCut;
	}
}
