using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	//public static InventoryController instance;
	public static InventoryManager instance { get; private set; }

	[SerializeField] private List<InventoryItem> inventoryItems;
	private Dictionary<ItemData, InventoryItem> inventoryItemsDict;
	[SerializeField] private List<InventoryItem> stashItems;
	private Dictionary<ItemData, InventoryItem> stashItemsDict;
	[SerializeField] private List<InventoryItem> equipmentItems;
	private Dictionary<EquipmentType, InventoryItem> equipmentItemsDict;

	[SerializeField] private Transform inventorySlotsParent;
	private UIItemSlotController[] inventorySlots;
	[SerializeField] private Transform stashSlotsParent;
	private UIItemSlotController[] stashSlots;
	[SerializeField] private Transform equipmentSlotsParent;
	private UIItemSlotController[] equipmentSlots;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
		inventoryItems = new List<InventoryItem>();
		inventoryItemsDict = new Dictionary<ItemData, InventoryItem>();
		stashItems = new List<InventoryItem>();
		stashItemsDict = new Dictionary<ItemData, InventoryItem>();
		equipmentItems = new List<InventoryItem>();
		equipmentItemsDict = new Dictionary<EquipmentType, InventoryItem>();
		this.UpdateEquipmentSlots();
		this.UpdateInventorySlots();
		this.UpdateStashSlots();
	}

	public List<InventoryItem> GetEquipmentItemsList() => new(this.equipmentItems);

	private void UpdateInventorySlots()
	{
		inventorySlots = inventorySlotsParent.GetComponentsInChildren<UIItemSlotController>();
		for (int i = 0; i < inventorySlots.Length; i++)
		{
			inventorySlots[i].UpdateData((i < inventoryItems.Count) ? inventoryItems[i] : null);
		}
	}

	private void UpdateStashSlots()
	{
		stashSlots = stashSlotsParent.GetComponentsInChildren<UIItemSlotController>();
		for (int i = 0; i < stashSlots.Length; i++)
		{
			stashSlots[i].UpdateData((i < stashItems.Count) ? stashItems[i] : null);
		}
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.B))
		{
			RemoveItem(stashItems[0].itemData);
			UpdateStashSlots();
		}
	}

	private void UpdateEquipmentSlots()
	{
		equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<UIItemSlotController>();
		for (int i = 0; i < equipmentSlots.Length; i++)
		{
			equipmentSlots[i].UpdateData(null);
		}
		foreach (var item in equipmentItems)
		{
			switch ((item.itemData as EquipmentData).equipmentType)
			{
				case EquipmentType.Weapon:
					{
						equipmentSlots[0].UpdateData(item);
						break;
					}
				case EquipmentType.Armor:
					{
						equipmentSlots[1].UpdateData(item);
						break;
					}
				case EquipmentType.Amulet:
					{
						equipmentSlots[2].UpdateData(item);
						break;
					}
				case EquipmentType.Flask:
					{
						equipmentSlots[3].UpdateData(item);
						break;
					}
			}
		}
	}


	public bool CanCraftItem(ItemData objectItem)
	{
		if (!objectItem.canBeCrafted || objectItem == null) return false;

		foreach (var ingredient in objectItem.ingredients)
		{
			if (ingredient.itemData.itemType == ItemType.Material)
			{
				if (!(inventoryItemsDict.TryGetValue(ingredient.itemData, out var availableInventoryItem) && availableInventoryItem.stackSize >= ingredient.stackSize)) return false;
			}
			else if (ingredient.itemData.itemType == ItemType.Equipment)
			{
				if (!(stashItemsDict.TryGetValue(ingredient.itemData, out var availableStashItem) && availableStashItem.stackSize >= ingredient.stackSize)) return false;
			}
		}
		foreach (var ingredient in objectItem.ingredients)
		{
			this.RemoveItem(ingredient.itemData, ingredient.stackSize);
		}
		this.AddItem(objectItem);
		return true;
	}

	public bool CanAddItem(ItemData itemData) => this.CanAddItem(itemData, 1);

	public bool CanAddItem(ItemData itemData, int size)
	{
		if (itemData == null || size <= 0) return false;
		switch (itemData.itemType)
		{
			case ItemType.Material:
				{
					return this.CanAddToInventory(itemData, size);
				}
			case ItemType.Equipment:
				{
					return this.CanAddToStash(itemData, size);
				}
		}
		return true;
	}

	public InventoryItem AddItem(ItemData itemData) => AddItem(itemData, 1);


	public InventoryItem AddItem(ItemData itemData, int size)
	{
		if (itemData == null) throw new ArgumentNullException(nameof(itemData));
		if (itemData.itemType == ItemType.Material)
		{
			AddToInventory(itemData, size);
			UpdateInventorySlots();
			return inventoryItemsDict[itemData];
		}
		else if (itemData.itemType == ItemType.Equipment)
		{
			AddToStash(itemData, size);
			UpdateStashSlots();
			return stashItemsDict[itemData];
		}
		return null;
	}

	public InventoryItem EquipItem(ItemData itemData)
	{
		if (itemData == null || itemData.itemType != ItemType.Equipment) return null;

		InventoryItem newItem = new InventoryItem(itemData);
		if (equipmentItemsDict.TryGetValue((newItem.itemData as EquipmentData).equipmentType, out var oldItem))
		{
			equipmentItemsDict[(newItem.itemData as EquipmentData).equipmentType] = newItem;
			equipmentItems.Remove(oldItem);
			equipmentItems.Add(newItem);
			AddItem(oldItem.itemData);
			RemoveItem(newItem.itemData);
			(oldItem.itemData as EquipmentData).RemoveModifiersFromPlayer();
		}
		else
		{
			var newEquipmentItem = new InventoryItem(itemData);
			equipmentItems.Add(newEquipmentItem);
			equipmentItemsDict.Add((itemData as EquipmentData).equipmentType, newEquipmentItem);
			RemoveItem(itemData);
		}
		(newItem.itemData as EquipmentData).AddModifiersToPlayer();
		UpdateEquipmentSlots();
		return equipmentItemsDict[(itemData as EquipmentData).equipmentType];
	}

	public bool RemoveEquipment(ItemData itemData)
	{
		if (itemData == null || itemData.itemType != ItemType.Equipment) return false;
		bool result = false;
		if (equipmentItemsDict.TryGetValue((itemData as EquipmentData).equipmentType, out InventoryItem equipmentItem))
		{
			equipmentItems.Remove(equipmentItem);
			equipmentItemsDict.Remove((itemData as EquipmentData).equipmentType);
			AddItem(itemData);
			result = true;
		}
		(itemData as EquipmentData).RemoveModifiersFromPlayer();
		UpdateEquipmentSlots();
		return result;
	}

	private void AddToInventory(ItemData itemData, int size)
	{
		if (inventoryItemsDict.TryGetValue(itemData, out InventoryItem item))
		{
			item.AddStack(size);
		}
		else
		{
			InventoryItem newItem = new InventoryItem(itemData, size);
			inventoryItems.Add(newItem);
			inventoryItemsDict.Add(itemData, newItem);
		}
	}

	private void AddToStash(ItemData itemData, int size)
	{
		if (stashItemsDict.TryGetValue(itemData, out InventoryItem item))
		{
			item.AddStack(size);
		}
		else
		{
			InventoryItem newItem = new InventoryItem(itemData, size);
			stashItems.Add(newItem);
			stashItemsDict.Add(itemData, newItem);
		}
	}

	private bool CanAddToStash(ItemData itemData, int size)
	{
		if (itemData == null || size <= 0 || stashItems.Count >= stashSlots.Length) return false;
		return true;
	}

	private bool CanAddToInventory(ItemData itemData, int size)
	{
		if (itemData == null || size <= 0) return false;
		return true;
	}

	public InventoryItem RemoveItem(ItemData itemData) => RemoveItem(itemData, 1);

	public InventoryItem RemoveItem(ItemData itemData, int size)
	{
		InventoryItem result = null;
		if (itemData.itemType == ItemType.Material)
		{
			result = RemoveItemFromInventory(itemData, size);
			UpdateInventorySlots();
		}
		else if (itemData.itemType == ItemType.Equipment)
		{
			result = RemoveItemFromStash(itemData, size);
			UpdateStashSlots();
		}
		return result;
	}
	private InventoryItem RemoveItemFromInventory(ItemData itemData, int size)
	{
		InventoryItem result = null;
		if (inventoryItemsDict.TryGetValue(itemData, out InventoryItem item))
		{
			if (item.RemoveStack(size) == 0)
			{
				inventoryItems.Remove(item);
				inventoryItemsDict.Remove(itemData);
			}
			else result = item;
		}
		return result;
	}

	private InventoryItem RemoveItemFromStash(ItemData itemData, int size)
	{
		InventoryItem result = null;
		if (stashItemsDict.TryGetValue(itemData, out InventoryItem item))
		{
			if (item.RemoveStack(size) == 0)
			{
				stashItems.Remove(item);
				stashItemsDict.Remove(itemData);
			}
			else result = item;
		}
		return result;
	}


}
