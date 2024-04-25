using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillTreeSlotController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private UIManager uiManager;

	[SerializeField] private BasicSkillData skillData;
	[SerializeField] private Sprite skillIcon;
	[SerializeField] private string skillName;
	[TextArea]
	[SerializeField] private string skillDescription;

	[SerializeField] private List<UISkillTreeSlotController> prerequisiteSkills;
	[SerializeField] private List<UISkillTreeSlotController> exclusiveSkills;

	[SerializeField] private bool unlocked = false;

	public BasicSkillData Skill
	{
		get { return skillData; }
		private set { skillData = value; }
	}
	private void Awake()
	{
	}

	public bool IsUnlocked() => unlocked;
	public Action OnUnlockedChanged { get; set; }
	private Button skillButton;

	private void Start()
	{
		skillButton = GetComponent<Button>();
		UpdateSkillSlotUI();
		OnUnlockedChanged += UpdateSkillSlotUI;
	}


	public bool CanUnlock()
	{
		for (int i = 0; i < prerequisiteSkills.Count; i++)
		{
			if (!prerequisiteSkills[i].IsUnlocked()) return false;
		}
		for (int i = 0; i < exclusiveSkills.Count; i++)
		{
			if (exclusiveSkills[i].IsUnlocked()) return false;
		}
		return true;
	}

	public void UnlockSkill()
	{
		if (CanUnlock())
		{
			unlocked = true;
			OnUnlockedChanged?.Invoke();
		}
		else
			Debug.Log("Can't unlock skill " + skillName);
	}

	public void SetSkillUnlockedIgnoreConditions(bool unlocked)
	{
		this.unlocked = unlocked;
		OnUnlockedChanged?.Invoke();
	}

	public void LockSkill()
	{
		unlocked = false;
		OnUnlockedChanged?.Invoke();
	}

	public void UpdateSkillSlotUI()
	{
		skillButton.interactable = !unlocked;
	}

	private void OnValidate()
	{

		UpdateFromSkillData();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		uiManager.GetMenuPageController().ShowSkillToolTip(skillName, skillDescription);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		uiManager.GetMenuPageController().HideSkillTreeToolTip();
	}

	private void UpdateFromSkillData()
	{
		if (skillData != null)
		{
			skillIcon = skillData.skillIcon;
			skillName = skillData.skillName;
			skillDescription = skillData.skillDescription;
			prerequisiteSkills = new List<UISkillTreeSlotController>();
			exclusiveSkills = new List<UISkillTreeSlotController>();
			foreach (var skillData in skillData.prerequisiteSkills)
			{
				for (int i = 0; i < transform.parent.childCount; i++)
				{
					var sibling = transform.parent.GetChild(i).GetComponent<UISkillTreeSlotController>();
					if (this.skillData == skillData) continue;
					if (skillData == sibling.Skill)
					{
						prerequisiteSkills.Add(sibling);
						break;
					}
				}

			}
			foreach (var skillData in skillData.exclusiveSkills)
			{
				for (int i = 0; i < transform.parent.childCount; i++)
				{
					var sibling = transform.parent.GetChild(i).GetComponent<UISkillTreeSlotController>();
					if (this.skillData == skillData) continue;
					if (skillData == sibling.Skill)
					{
						exclusiveSkills.Add(sibling);
						break;
					}
				}
			}
		}
		else
		{
			skillIcon = null;
			skillDescription = "";
			unlocked = false;
			prerequisiteSkills = null;
			exclusiveSkills = null;

		}
		name = "Skill Tree Slot - " + skillName;
		GetComponent<Image>().sprite = skillIcon;
	}


}
