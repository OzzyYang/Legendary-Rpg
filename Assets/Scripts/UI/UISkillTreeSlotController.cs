using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillTreeSlotController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private BasicSkillData skillData;
	[SerializeField] private Sprite skillIcon;
	[SerializeField] private string skillName;
	[TextArea]
	[SerializeField] private string skillDescription;

	[SerializeField] private List<BasicSkillData> prerequisiteSkills;
	[SerializeField] private List<BasicSkillData> exclusiveSkills;

	[SerializeField] private bool unlocked = false;


	private void Awake()
	{
	}

	public bool IsUnlocked() => unlocked;
	public Action OnUnlockedChanged { get; private set; }
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
			if (!prerequisiteSkills[i].unlocked) return false;
		}
		for (int i = 0; i < exclusiveSkills.Count; i++)
		{
			if (exclusiveSkills[i].unlocked) return false;
		}
		return true;
	}

	public void UnlockSkill()
	{
		if (CanUnlock())
		{
			unlocked = skillData.unlocked = true;
			OnUnlockedChanged?.Invoke();
		}
		else
			Debug.Log("Can't unlock skill " + skillName);
	}

	public void LockSkill()
	{
		unlocked = skillData.unlocked = false;
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
	private void UpdateSkillContent()
	{
		name = "Skill Tree Slot - " + skillName;
		this.GetComponent<Image>().sprite = skillIcon;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		UIManager.instance.GetComponent<UIManager>().GetMenuPageController().ShowSkillToolTip(skillName, skillDescription);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		UIManager.instance.GetComponent<UIManager>().GetMenuPageController().HideSkillTreeToolTip();
	}

	private void UpdateFromSkillData()
	{
		if (skillData != null)
		{
			skillIcon = skillData.skillIcon;
			skillName = skillData.skillName;
			skillDescription = skillData.skillDescription;
			unlocked = skillData.unlocked;
			prerequisiteSkills = skillData.prerequisiteSkills;
			exclusiveSkills = skillData.exclusiveSkills;
			name = "Skill Tree Slot - " + skillName;
			GetComponent<Image>().sprite = skillIcon;
		}
	}
}
