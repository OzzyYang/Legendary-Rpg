using UnityEngine;

public class UIManager : MonoBehaviour, ISaveManager
{
	[SerializeField] private GameObject uiParent;
	[SerializeField] private GameObject menuPages;
	[SerializeField] private UIInGameUIController inGameUI;
	public static UIManager instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = GetComponent<UIManager>();
		}
		else
		{
			Destroy(gameObject);
		}
		menuPages.SetActive(false);
	}

	public void SwitchToPage(GameObject pageToSwitch)
	{
		for (int i = 0; i < menuPages.transform.childCount; i++)
		{
			menuPages.transform.GetChild(i).gameObject.SetActive(false);
		}

		pageToSwitch?.SetActive(true);
	}

	public UIMenuPageController GetMenuPageController() => menuPages.GetComponent<UIMenuPageController>();

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			menuPages.SetActive(!menuPages.activeSelf);
			inGameUI.gameObject.SetActive(!menuPages.activeSelf);
		}
	}

	public void LoadData(GameData data)
	{
		foreach (var skillTreeSlot in GetMenuPageController().GetAllUISkillTreeSlots())
		{
			if (data.skills.TryGetValue(skillTreeSlot.Skill.skillId, out bool unlocked))
			{
				skillTreeSlot.SetSkillUnlockedIgnoreConditions(unlocked);
			}
		}
	}

	public void SaveData(ref GameData data)
	{
		foreach (var skillTreeSlot in GetMenuPageController().GetAllUISkillTreeSlots())
		{
			data.skills.Add(skillTreeSlot.Skill.skillId, skillTreeSlot.IsUnlocked());
		};
	}
}
