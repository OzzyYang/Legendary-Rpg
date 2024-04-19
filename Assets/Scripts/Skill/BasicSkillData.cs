using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Skill Data", menuName = "Data/Basic Skill")]
public class BasicSkillData : ScriptableObject
{
	public string skillName;
	public Sprite skillIcon;
	[TextArea]
	public string skillDescription;
	public float skillCoolDownTime;
	public KeyCode shortCut;

	public List<BasicSkillData> prerequisiteSkills;
	public List<BasicSkillData> exclusiveSkills;

	public bool unlocked;
	public int maxAvailableTimes = 1;
}
