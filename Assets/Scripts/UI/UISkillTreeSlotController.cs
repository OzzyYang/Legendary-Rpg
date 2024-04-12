using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkillTreeSlotController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] private Sprite skillIcon;
	[SerializeField] private string skillName;
	[TextArea]
	[SerializeField] private string skillDescription;

	[SerializeField] private List<UISkillTreeSlotController> prerequisiteSkills;
	[SerializeField] private List<UISkillTreeSlotController> exclusiveSkills;

	[SerializeField] private bool unlocked = false;


	public bool IsUnlocked() => unlocked;
	public Action OnUnlockedChanged { get; private set; }
	private Button skillButton;

	private void Start()
	{
		skillButton = GetComponent<Button>();
		this.OnUnlockedChanged += this.UpdateSkillSlot;
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
			this.unlocked = true;
			OnUnlockedChanged?.Invoke();
		}
		else
			Debug.Log("Can't unlock skill " + this.skillName);
	}

	public void LockSkill()
	{
		this.unlocked = false;
		OnUnlockedChanged?.Invoke();
	}

	public void UpdateSkillSlot()
	{
		skillButton.interactable = !unlocked;
	}

	private void OnValidate()
	{
		this.UpdateSkillContent();
	}
	private void UpdateSkillContent()
	{
		this.name = "Skill Tree Slot - " + this.skillName;
		this.GetComponent<Image>().sprite = this.skillIcon;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		UIManager.instance.GetComponent<UIManager>().GetMenuPageController().ShowSkillToolTip(this.skillName, this.skillDescription);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		UIManager.instance.GetComponent<UIManager>().GetMenuPageController().HideSkillTreeToolTip();
	}
}
