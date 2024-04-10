using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UIMenuPageController : MonoBehaviour
{
	[Header("Character page")]
	[SerializeField] List<GameObject> statsSlotParentList;
	[SerializeField] GameObject toolTip;
	[SerializeField] GameObject character;
	// Start is called before the first frame update
	void Start()
	{
		this.character = PlayerManager.instance.player.gameObject;
		UpdateStatsFrom(character.GetComponent<CharacterStats>());
		this.toolTip.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		UpdateStatsFrom(character.GetComponent<CharacterStats>());
	}


	public void SwitchToPage(GameObject pageToSwitch)
	{
		for (int i = 0; i < this.transform.childCount; i++)
		{
			this.transform.GetChild(i).gameObject.SetActive(false);
		}
		pageToSwitch?.SetActive(true);
	}

	#region Character Page
	public void UpdateStatsFrom(CharacterStats characterStats)
	{
		foreach (var statSlots in statsSlotParentList)
		{
			foreach (var statSlot in statSlots.GetComponentsInChildren<UIStatSlotController>())
			{
				statSlot.SetupSlotByCharacter(character.GetComponent<CharacterStats>());
			}
		}
	}

	public void ShowItemToolTip(ItemData itemInfo)
	{
		if (itemInfo == null) return;
		this.toolTip.SetActive(true);
		TextMeshProUGUI itemName, itemType, itemDescription;
		itemName = toolTip.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
		itemType = toolTip.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
		itemDescription = toolTip.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
		itemName.text = itemInfo.itemName.ToString();
		itemType.text = itemInfo.itemType.ToString();


		StringBuilder sb = new StringBuilder();
		sb.Append(FormateContentFromStat(StatType.Strength, (itemInfo as EquipmentData).strength));
		sb.Append(FormateContentFromStat(StatType.Agility, (itemInfo as EquipmentData).agility));
		sb.Append(FormateContentFromStat(StatType.Intelligence, (itemInfo as EquipmentData).intelligence));
		sb.Append(FormateContentFromStat(StatType.Vitality, (itemInfo as EquipmentData).vitality));
		sb.Append(FormateContentFromStat(StatType.Damage, (itemInfo as EquipmentData).damage));
		sb.Append(FormateContentFromStat(StatType.CriticalRate, (itemInfo as EquipmentData).criticalRate));
		sb.Append(FormateContentFromStat(StatType.CriticalMultiplier, (itemInfo as EquipmentData).criticalMultiplier));
		sb.Append(FormateContentFromStat(StatType.FireDamage, (itemInfo as EquipmentData).fireDamage));
		sb.Append(FormateContentFromStat(StatType.FrostDamage, (itemInfo as EquipmentData).frostDamage));
		sb.Append(FormateContentFromStat(StatType.LightningDamge, (itemInfo as EquipmentData).lightningDamge));
		sb.Append(FormateContentFromStat(StatType.Armor, (itemInfo as EquipmentData).armor));
		sb.Append(FormateContentFromStat(StatType.EvasionRate, (itemInfo as EquipmentData).evasionRate)); sb.Append(FormateContentFromStat(StatType.MagicResistance, (itemInfo as EquipmentData).magicResistance));
		sb.Append(FormateContentFromStat(StatType.MaxHealth, (itemInfo as EquipmentData).maxHealth));
		itemDescription.text = sb.ToString();
	}
	public void HideItemToolTip() => this.toolTip.SetActive(false);

	private string FormateContentFromStat(StatType statType, Stat stat)
	{
		if (stat == null || stat.GetValue() <= 0) return "";
		return statType.ToString() + ": " + stat.GetValue().ToString() + "\n";
	}
	#endregion
}
