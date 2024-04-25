using TMPro;
using UnityEngine;

public class UIInGameUIController : MonoBehaviour
{
	[SerializeField] private Transform skillBar;
	[SerializeField] private GameObject skillBarSlotPrefab;
	[SerializeField] private SkillManager skillManager;
	[SerializeField] private GameObject currencyUI;


	private void Awake()
	{
		InventoryManager.Instance.OnCurrencyChanged += UpdateCurrencyUI;
		InitializeSkillBar();
	}

	private void InitializeSkillBar()
	{
		foreach (var skill in skillManager.SkillList)
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
		currencyUI.GetComponentInChildren<TextMeshProUGUI>().text = currency == 0 ? "0" : currency.ToString("#,#");
	}

	public void ShowInGameUI() => gameObject.SetActive(true);

	public void HideInGameUI() => gameObject.SetActive(false);

}
