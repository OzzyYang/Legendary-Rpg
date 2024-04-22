using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuPageController : MonoBehaviour
{
	[SerializeField] private List<GameObject> stashSlotsParents;
	[SerializeField] private List<GameObject> inventorySlotsParents;
	[SerializeField] private List<GameObject> equipmentSlotsParents;
	[SerializeField] GameObject itemSlotPrefab;
	[SerializeField] GameObject equipmentSlotPrefab;

	[Header("Character page")]
	[SerializeField] List<GameObject> statsSlotParentList;
	[SerializeField] GameObject itemToolTip;
	[SerializeField] GameObject character;

	[Header("Skill Tree page")]
	[SerializeField] private GameObject skillTreeToolTip;

	[Header("Craft Page")]
	[SerializeField] GameObject craftItemInfoPanel;
	[SerializeField] GameObject craftSlotPrefab;
	[SerializeField] Transform CraftSlotsParent;
	[SerializeField] List<EquipmentData> craftWeaponList;
	[SerializeField] List<EquipmentData> craftArmorList;
	[SerializeField] List<EquipmentData> craftAmuletList;
	[SerializeField] List<EquipmentData> craftFlaskList;
	private ItemData selectedItemInfo;

	private void Awake()
	{
		InventoryManager.instance.OnInventoryListChanged += UpdateInventorySlots;
		InventoryManager.instance.OnEquipmentListChanged += UpdateEquipmentSlots;
		InventoryManager.instance.OnStashListChanged += UpdateStashSlots;
		InventoryManager.instance.OnInventoryListChanged += UpdateCraftButtonStatus;
		InventoryManager.instance.OnStashListChanged += UpdateCraftButtonStatus;

	}

	void Start()
	{
		character = PlayerManager.instance.player.gameObject;
		UpdateStatsFrom(character.GetComponent<CharacterStats>());
		itemToolTip.SetActive(false);
		ShowCraftItemInfo(null);
		ShowCraftSlotsListByType(EquipmentType.Weapon);
	}

	void Update()
	{
		UpdateStatsFrom(character.GetComponent<CharacterStats>());
	}

	private void UpdateInventorySlots()
	{
		var inventoryItemsList = InventoryManager.instance.GetInventoryItemsList();
		foreach (var inventorySlotsParent in inventorySlotsParents)
		{
			//clear all the slots
			for (int i = 0; i < inventorySlotsParent.transform.childCount; i++)
			{
				Destroy(inventorySlotsParent.transform.GetChild(i).gameObject);
			}
			//respawn all the slots
			foreach (var inventory in inventoryItemsList)
			{
				Instantiate(itemSlotPrefab, inventorySlotsParent.transform).GetComponent<UIItemSlotController>().UpdateData(inventory);
			}
		}
	}

	private void UpdateStashSlots()
	{
		var stashItemsList = InventoryManager.instance.GetStashItemsList();
		foreach (var stashSlotsParent in stashSlotsParents)
		{
			//clear all the slots
			for (int i = 0; i < stashSlotsParent.transform.childCount; i++)
			{
				Destroy(stashSlotsParent.transform.GetChild(i).gameObject);
			}
			//respawn all the slots
			foreach (var stashItem in stashItemsList)
			{
				Instantiate(itemSlotPrefab, stashSlotsParent.transform).GetComponent<UIItemSlotController>().UpdateData(stashItem);
			}
		}
	}

	private void UpdateEquipmentSlots()
	{
		var equipmentItemsList = InventoryManager.instance.GetEquipmentItemsList();
		foreach (var equipmentSlotsParent in equipmentSlotsParents)
		{
			for (int i = 0; i < equipmentSlotsParent.transform.childCount; i++)
			{
				//There are only four types of equipment available, so only updating slots rather than destroying them;
				equipmentSlotsParent.transform.GetChild(i).GetComponent<UIEquipmentSlotController>().UpdateData(null);
			}

			var equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<UIEquipmentSlotController>();
			foreach (var equipment in equipmentItemsList)
			{

				switch ((equipment.itemData as EquipmentData).equipmentType)
				{
					case EquipmentType.Weapon:
						{
							equipmentSlots[0].UpdateData(equipment);
							break;
						}
					case EquipmentType.Armor:
						{
							equipmentSlots[1].UpdateData(equipment);
							break;
						}
					case EquipmentType.Amulet:
						{
							equipmentSlots[2].UpdateData(equipment);
							break;
						}
					case EquipmentType.Flask:
						{
							equipmentSlots[3].UpdateData(equipment);
							break;
						}
				}

			}
		}
	}
	private void UpdateCraftButtonStatus()
	{
		if (selectedItemInfo != null)
			EnableCraftButton(InventoryManager.instance.CanCraftItem(selectedItemInfo));
	}


	public void SwitchToPage(GameObject pageToSwitch)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(false);
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

	#region Skill Tree Page
	public void ShowSkillToolTip(string newSkillName, string newSkillDescription)
	{
		TextMeshProUGUI skillName = skillTreeToolTip.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
		TextMeshProUGUI skillDescription = skillTreeToolTip.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
		skillName.text = newSkillName;
		skillDescription.text = newSkillDescription;
		skillTreeToolTip.GetComponent<UIToolTipController>().Show();
	}
	public void HideSkillTreeToolTip() => skillTreeToolTip.GetComponent<UIToolTipController>().Hide();
	#endregion

	#region Craft Page
	public void ShowCraftItemInfo(ItemData itemInfo)
	{
		var itemName = craftItemInfoPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
		var itemDescription = craftItemInfoPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
		var itemMaterialsParent = craftItemInfoPanel.transform.GetChild(2);
		for (int i = 0; i < itemMaterialsParent.childCount; i++)
		{
			Destroy(itemMaterialsParent.GetChild(i).gameObject);
		}
		if (itemInfo == null)
		{
			itemName.text = "";
			itemDescription.text = "";
			EnableCraftButton(false);
			return;
		}
		itemName.text = itemInfo.itemName;
		itemDescription.text = FormatContentByType(itemInfo).ToString();
		foreach (var material in itemInfo.ingredients)
		{
			Instantiate(itemSlotPrefab, itemMaterialsParent).GetComponent<UIItemSlotController>().UpdateData(material);
		}
		EnableCraftButton(itemInfo.canBeCrafted && InventoryManager.instance.CanCraftItem(itemInfo));
		selectedItemInfo = itemInfo;
	}

	private void EnableCraftButton(bool enable)
	{
		var craftButton = craftItemInfoPanel.transform.GetChild(3);
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
		EnableCraftButton(selectedItemInfo.canBeCrafted && InventoryManager.instance.CanCraftItem(selectedItemInfo));
	}
	#endregion

	#region Item Tool Tip
	public void ShowItemToolTip(ItemData itemInfo)
	{
		if (itemInfo == null) return;
		//this.itemToolTip.transform.position = Input.mousePosition;
		TextMeshProUGUI itemName, itemType, itemDescription;
		itemName = itemToolTip.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
		itemType = itemToolTip.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
		itemDescription = itemToolTip.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
		itemName.text = itemInfo.itemName.ToString();
		itemType.text = itemInfo.itemType.ToString();

		StringBuilder sb = FormatContentByType(itemInfo);
		itemDescription.text = sb.ToString();

		itemToolTip.GetComponent<UIToolTipController>().Show();
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
		if (itemInfo.effectDescription != null && itemInfo.effectDescription.Length > 0) sb.Append("\nSpecial Effect: " + itemInfo.effectDescription);
		return sb;
	}

	public void HideItemToolTip() => itemToolTip.GetComponent<UIToolTipController>().Hide();

	private string FormateContentFromStat(StatType statType, Stat stat)
	{
		if (stat == null || stat.GetValue() <= 0) return "";
		return statType.ToString() + ": " + stat.GetValue().ToString() + "\n";
	}

	#endregion
}
