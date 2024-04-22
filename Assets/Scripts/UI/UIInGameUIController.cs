using TMPro;
using UnityEngine;

public class UIInGameUIController : MonoBehaviour
{
	[SerializeField] private Transform skillBar;
	[SerializeField] private GameObject skillBarSlotPrefab;
	[SerializeField] private SkillManager skillManager;
	[SerializeField] private GameObject currencyUI;


	private void Start()
	{
		this.UpdateSkillBar();
		this.UpdateCurrencyUI(InventoryManager.instance.currency);
		InventoryManager.instance.OnCurrencyChanged += UpdateCurrencyUI;
	}

	private void UpdateSkillBar()
	{
		foreach (var skill in skillManager.skillList)
		{
			UISkillBarSlotController skillSlot =
			Instantiate(skillBarSlotPrefab, skillBar).GetComponent<UISkillBarSlotController>();
			skillSlot.Setup(skill.Key);
			skill.Value.OnAvailableTimesChanged += skillSlot.UpdateSkillAvailableTimes;
			skill.Value.OnSkillUpdated += skillSlot.Setup;
		}
	}

	public void UpdateCurrencyUI(int currency)
	{
		if (currencyUI != null)
			currencyUI.GetComponentInChildren<TextMeshProUGUI>().text = currency.ToString("#,#");
	}

	public void ShowInGameUI() => gameObject.SetActive(true);

	public void HideInGameUI() => gameObject.SetActive(false);

}
