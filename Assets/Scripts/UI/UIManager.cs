using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ISaveManager
{
	[SerializeField] private GameObject uiParent;
	[SerializeField] private GameObject menuPages;
	[SerializeField] private UIInGameUIController inGameUI;
	[SerializeField] private UIVFX endScreen;
	//[SerializeField] private TextMeshProUGUI endText;
	public static UIManager instance;

	//public UIVFX EndScreen { get { return endScreen; } private set { endScreen = value; } }

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

	public async void PlayeEndScreen()
	{
		endScreen.gameObject.SetActive(true);
		endScreen.GetComponent<Image>().color = new Color(0, 0, 0, 0);
		for (int i = 0; i < endScreen.transform.childCount; i++)
		{
			endScreen.transform.GetChild(i).gameObject.SetActive(false);
		}
		await endScreen.FadeInAsync(1.2f);
		for (int i = 0; i < endScreen.transform.childCount; i++)
		{
			endScreen.transform.GetChild(i).gameObject.SetActive(true);
		}
	}
}
