using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillBarSlotController : MonoBehaviour
{
	[SerializeField] private BasicSkillData skillData;
	[SerializeField] private Image background;
	[SerializeField] private Image mask;
	[SerializeField] private TextMeshProUGUI shortcutText;
	[SerializeField] private TextMeshProUGUI availableTimesText;
	[SerializeField] private SkillManager skillManager;

	private void Awake()
	{
		skillManager = SkillManager.instance;
	}
	void Start()
	{
		UpdateFromSkillData();
	}

	void Update()
	{
		RefreshCoolDownUI();
	}

	private void RefreshCoolDownUI()
	{
		if (skillData == null) return;
		if (!UIManager.instance.GetMenuPageController().GetUISkillTreeSlotBySkillData(skillData).IsUnlocked())
		{
			mask.fillAmount = 1;
			return;
		}

		if (skillData is UpgradeSkillData)
		{
			mask.fillAmount = skillManager.SkillList[(skillData as UpgradeSkillData).baseSkill].CoolDownTimer / skillData.skillCoolDownTime;
		}
		else
		{
			mask.fillAmount = skillManager.SkillList[skillData].CoolDownTimer / skillData.skillCoolDownTime;
		}
	}

	private void OnValidate()
	{
		this.UpdateFromSkillData();
	}
	private void UpdateFromSkillData()
	{
		if (skillData != null)
		{
			background.sprite = skillData.skillIcon;
			mask.sprite = skillData.skillIcon;
			shortcutText.text = skillData.shortCut.ToString();
			UpdateSkillAvailableTimes(0);
			this.name = "Skill Bar Slot - " + skillData.skillName;
		}
	}

	public void UpdateSkillAvailableTimes(int availableTimes)
	{
		availableTimesText.text = skillData.maxAvailableTimes > 1 ? availableTimes + "/" + skillData.maxAvailableTimes.ToString() : "";
	}

	public void Setup(BasicSkillData skillData)
	{
		this.skillData = skillData;
		UpdateFromSkillData();
	}

}
