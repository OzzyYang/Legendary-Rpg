using UnityEngine;

public class UIInGameUIController : MonoBehaviour
{
	[SerializeField] private Transform skillBar;
	[SerializeField] private GameObject skillBarSlotPrefab;
	[SerializeField] private SkillManager skillManager;


	private void Awake()
	{

	}

	private void Start()
	{
		this.UpdateSkillBar();
	}

	private void UpdateSkillBar()
	{
		foreach (var skill in skillManager.skillList)
		{
			UISkillBarSlotController skillSlot =
			Instantiate(skillBarSlotPrefab, skillBar).GetComponent<UISkillBarSlotController>();
			skillSlot.Setup(skill.Key);
			skill.Value.OnAvailableTimesChanged += skillSlot.UpdateSkillAvailableTimes;
		}
	}
}
