using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Skill Data", menuName = "Data/Upgrade Skill")]
public class UpgradeSkillData : BasicSkillData
{
	public BasicSkillData baseSkill;

	protected override void OnValidate()
	{
		base.OnValidate();
		if (baseSkill == null) return;
		this.skillCoolDownTime = baseSkill.skillCoolDownTime;
		this.shortCut = baseSkill.shortCut;
	}
}
