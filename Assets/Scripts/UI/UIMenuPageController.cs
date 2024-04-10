using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuPageController : MonoBehaviour
{
	[Header("Character page")]
	[SerializeField] List<GameObject> statsSlotParentList;
	[SerializeField] GameObject itemToolTip;
	[SerializeField] GameObject character;

	[Header("Craft Page")]
	[SerializeField] GameObject craftItemInfoToolTip;
	[SerializeField] GameObject itemSlotPrefab;
	[SerializeField] GameObject craftSlotPrefab;
	[SerializeField] Transform CraftSlotsParent;
	[SerializeField] List<EquipmentData> craftWeaponList;
	[SerializeField] List<EquipmentData> craftArmorList;
	[SerializeField] List<EquipmentData> craftAmuletList;
	[SerializeField] List<EquipmentData> craftFlaskList;
	private ItemData selectedItemInfo;
	// Start is called before the first frame update
	void Start()
	{
		this.character = PlayerManager.instance.player.gameObject;
		UpdateStatsFrom(character.GetComponent<CharacterStats>());
		this.itemToolTip.SetActive(false);
		this.ShowCraftItemInfo(null);
		this.ShowCraftSlotsListByType(EquipmentType.Weapon);
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
	#endregion

	#region Craft Page
	public void ShowCraftItemInfo(ItemData itemInfo)
	{
		var itemName = craftItemInfoToolTip.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		var itemDescription = craftItemInfoToolTip.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
		var itemMaterialsParent = craftItemInfoToolTip.transform.GetChild(2);
		for (int i = 0; i < itemMaterialsParent.childCount; i++)
		{
			Destroy(itemMaterialsParent.GetChild(i).gameObject);
		}
		if (itemInfo == null)
		{
			itemName.text = "";
			itemDescription.text = "";
			this.EnableCraftButton(false);
			return;
		}
		itemName.text = itemInfo.itemName;
		itemDescription.text = FormatContentByType(itemInfo).ToString();
		foreach (var material in itemInfo.ingredients)
		{
			Instantiate(itemSlotPrefab, itemMaterialsParent).GetComponent<UIItemSlotController>().UpdateData(material);
		}
		if (itemInfo.canBeCrafted && InventoryManager.instance.CanCraftItem(itemInfo))
		{
			this.EnableCraftButton(true);
		}
		else
		{
			this.EnableCraftButton(false);
		}
		this.selectedItemInfo = itemInfo;
	}

	private void EnableCraftButton(bool enable)
	{
		var craftButton = craftItemInfoToolTip.transform.GetChild(3);
		if (enable)
		{
			craftButton.GetComponent<Button>().interactable = true;
			craftButton.GetComponentInChildren<TextMeshProUGUI>().text = "Craft";
			craftButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 0.6f, 0, 1);
		}
		else
		{
			craftButton.GetComponent<Button>().interactable = false;
			craftButton.GetComponentInChildren<TextMeshProUGUI>().text = "Can't Craft";
			craftButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color(1, 0.6f, 0, 0.3f);
		}
	}

	public void ShowCraftSlotsListByType(EquipmentType equipmentType)
	{
		for (int i = 0; i < CraftSlotsParent.childCount; i++)
		{
			Destroy(CraftSlotsParent.GetChild(i).gameObject);
		}
		switch (equipmentType)
		{
			case EquipmentType.Weapon:
				{
					foreach (var weapon in craftWeaponList)
					{
						Instantiate(craftSlotPrefab, CraftSlotsParent).GetComponent<UICraftSlotController>().UpdateData(new InventoryItem(weapon));
					}
					break;
				}
			case EquipmentType.Armor:
				{
					foreach (var armor in craftArmorList)
					{
						Instantiate(craftSlotPrefab, CraftSlotsParent).GetComponent<UICraftSlotController>().UpdateData(new InventoryItem(armor));
					}
					break;
				}
			case EquipmentType.Amulet:
				{
					foreach (var amulet in craftAmuletList)
					{
						Instantiate(craftSlotPrefab, CraftSlotsParent).GetComponent<UICraftSlotController>().UpdateData(new InventoryItem(amulet));
					}
					break;
				}
			case EquipmentType.Flask:
				{
					foreach (var flask in craftFlaskList)
					{
						Instantiate(craftSlotPrefab, CraftSlotsParent).GetComponent<UICraftSlotController>().UpdateData(new InventoryItem(flask));
					}
					break;
				}
			default: { break; }
		}
	}
	public void CraftEquipment()
	{
		InventoryManager.instance.CraftItem(selectedItemInfo);
		this.EnableCraftButton(selectedItemInfo.canBeCrafted && InventoryManager.instance.CanCraftItem(selectedItemInfo));
	}
	#endregion
	public void ShowItemToolTip(ItemData itemInfo)
	{
		if (itemInfo == null) return;
		this.itemToolTip.SetActive(true);
		TextMeshProUGUI itemName, itemType, itemDescription;
		itemName = itemToolTip.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
		itemType = itemToolTip.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
		itemDescription = itemToolTip.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
		itemName.text = itemInfo.itemName.ToString();
		itemType.text = itemInfo.itemType.ToString();

		StringBuilder sb = FormatContentByType(itemInfo);
		itemDescription.text = sb.ToString();
	}

	private StringBuilder FormatContentByType(ItemData itemInfo)
	{
		StringBuilder sb = new StringBuilder();
		switch (itemInfo.itemType)
		{
			case ItemType.Material:
				{
					break;
				}
			case ItemType.Equipment:
				{
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
					break;
				}
		}

		return sb;
	}

	public void HideItemToolTip() => this.itemToolTip.SetActive(false);

	private string FormateContentFromStat(StatType statType, Stat stat)
	{
		if (stat == null || stat.GetValue() <= 0) return "";
		return statType.ToString() + ": " + stat.GetValue().ToString() + "\n";
	}
}
