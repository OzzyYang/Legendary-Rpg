using System.Collections.Generic;
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
	[SerializeField] UIToolTipController itemToolTip;
	[SerializeField] GameObject character;

	[Header("Skill Tree page")]
	[SerializeField] private GameObject skillTreeToolTip;
	[SerializeField] private GameObject skillTreeSlotsParent;

	[Header("Craft Page")]
	[SerializeField] GameObject craftItemInfoPanel;
	[SerializeField] GameObject craftSlotPrefab;
	[SerializeField] Transform CraftSlotsParent;
	[SerializeField] List<EquipmentData> craftWeaponList;
	[SerializeField] List<EquipmentData> craftArmorList;
	[SerializeField] List<EquipmentData> craftAmuletList;
	[SerializeField] List<EquipmentData> craftFlaskList;
	private ItemData selectedItemInfo;
	[SerializeField] private GameObject selectedPage;

	private void Awake()
	{
		InventoryManager.Instance.OnInventoryListChanged += UpdateInventorySlots;
		InventoryManager.Instance.OnEquipmentListChanged += UpdateEquipmentSlots;
		InventoryManager.Instance.OnStashListChanged += UpdateStashSlots;
		InventoryManager.Instance.OnInventoryListChanged += UpdateCraftButtonStatus;
		InventoryManager.Instance.OnStashListChanged += UpdateCraftButtonStatus;

	}
	void Start()
	{
		character = PlayerManager.Instance.Player.gameObject;
		UpdateStatsFrom(character.GetComponent<CharacterStats>());
		itemToolTip.Hide();
		ShowCraftItemInfo(null);
		ShowCraftSlotsListByType(EquipmentType.Weapon);
		UpdateInventorySlots();
		UpdateEquipmentSlots();
		UpdateStashSlots();
		SwitchToPage(selectedPage);
	}
	private void OnValidate()
	{
		SwitchToPage(selectedPage);
	}

	void Update()
	{
		UpdateStatsFrom(character.GetComponent<CharacterStats>());
	}


	#region Update UI
	private void UpdateInventorySlots()
	{
		var inventoryItemsList = InventoryManager.Instance.GetInventoryItemsList();
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
		var stashItemsList = InventoryManager.Instance.GetStashItemsList();
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
		var equipmentItemsList = InventoryManager.Instance.GetEquipmentItemsList();
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
			EnableCraftButton(InventoryManager.Instance.CanCraftItem(selectedItemInfo));
	}


	public void SwitchToPage(GameObject pageToSwitch)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(false);
		}
		if (pageToSwitch == null)
		{
			transform.GetChild(0).gameObject.SetActive(true);
		}
		else
		{
			pageToSwitch.SetActive(true);
		}
	}

	#endregion

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
		skillTreeToolTip.GetComponent<UIToolTipController>().Show(newSkillName, newSkillDescription);
	}
	public void HideSkillTreeToolTip() => skillTreeToolTip.GetComponent<UIToolTipController>().Hide();

	public UISkillTreeSlotController[] GetAllUISkillTreeSlots()
	{
		return skillTreeSlotsParent.GetComponentsInChildren<UISkillTreeSlotController>();
	}

	public UISkillTreeSlotController GetUISkillTreeSlotBySkillData(BasicSkillData skillData)
	{
		foreach (var skillTreeSlot in GetAllUISkillTreeSlots())
		{
			if (skillData == skillTreeSlot.Skill) return skillTreeSlot;
		}
		return null;
	}
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
		itemDescription.text = itemInfo.GetItemDescription();
		foreach (var material in itemInfo.ingredients)
		{
			Instantiate(itemSlotPrefab, itemMaterialsParent).GetComponent<UIItemSlotController>().UpdateData(material);
		}
		EnableCraftButton(itemInfo.canBeCrafted && InventoryManager.Instance.CanCraftItem(itemInfo));
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
		InventoryManager.Instance.CraftItem(selectedItemInfo);
		EnableCraftButton(selectedItemInfo.canBeCrafted && InventoryManager.Instance.CanCraftItem(selectedItemInfo));
	}
	#endregion

	#region Item Tool Tip
	public void ShowItemToolTip(ItemData itemInfo)
	{
		if (itemInfo == null || itemToolTip == null) return;
		itemToolTip.Show(itemInfo.itemName.ToString(), itemInfo.itemType.ToString(), itemInfo.GetItemDescription());
	}

	public void HideItemToolTip() => itemToolTip.Hide();

	#endregion
}
