using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour, ISaveManager
{
	public static InventoryManager Instance { get; private set; }

	[SerializeField] private List<InventoryItem> inventoryItems;
	private Dictionary<ItemData, InventoryItem> inventoryItemsDict;
	[SerializeField] private List<InventoryItem> stashItems;
	private Dictionary<ItemData, InventoryItem> stashItemsDict;
	[SerializeField] private List<InventoryItem> equipmentItems;
	private Dictionary<EquipmentType, InventoryItem> equipmentItemsDict;

	[SerializeField] private int stashSize = 16;

	public Action OnInventoryListChanged { get; set; }
	public Action OnStashListChanged { get; set; }
	public Action OnEquipmentListChanged { get; set; }

	[SerializeField] private int currency;
	public int Currency
	{
		get { return currency; }
		private set { currency = value; }
	}
	public Action<int> OnCurrencyChanged { get; set; }
	public int IncreaseCurrency(int numToIncrease)
	{
		Currency += numToIncrease;
		OnCurrencyChanged?.Invoke(Currency);
		return Currency;
	}

	public int DecreaseCurrency(int numToDecrease)
	{
		Currency -= numToDecrease;
		if (Currency < 0) Currency = 0;
		OnCurrencyChanged?.Invoke(Currency);
		return Currency;
	}
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
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
	}

	private void Start()
	{
		Currency = 30000;
		OnCurrencyChanged?.Invoke(currency);

	}

	public List<InventoryItem> GetEquipmentItemsList() => new(this.equipmentItems);

	public List<InventoryItem> GetInventoryItemsList() => new(this.inventoryItems);
	public List<InventoryItem> GetStashItemsList() => new(this.stashItems);


	public bool CanCraftItem(ItemData itemInfo)
	{
		if (!itemInfo.canBeCrafted || itemInfo == null) return false;

		foreach (var ingredient in itemInfo.ingredients)
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
		return true;
	}

	//Before craft item, better to estimate if there is enough ingredients by using CanCraftItem();
	public bool CraftItem(ItemData itemInfo)
	{
		if (CanCraftItem(itemInfo))
		{
			foreach (var ingredient in itemInfo.ingredients)
			{
				RemoveItem(ingredient.itemData, ingredient.stackSize);
			}
			AddItem(itemInfo);
			return true;
		}
		else
		{
			return false;
		}
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
			//UpdateInventorySlots();
			return inventoryItemsDict[itemData];
		}
		else if (itemData.itemType == ItemType.Equipment)
		{
			AddToStash(itemData, size);
			//UpdateStashSlots();
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
		OnEquipmentListChanged?.Invoke();
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
		OnEquipmentListChanged?.Invoke();
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
		if (OnInventoryListChanged != null) OnInventoryListChanged();
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
		OnStashListChanged?.Invoke();
	}

	private bool CanAddToStash(ItemData itemData, int size)
	{
		if (itemData == null || size <= 0 || stashItems.Count >= stashSize) return false;
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
			//UpdateInventorySlots();
		}
		else if (itemData.itemType == ItemType.Equipment)
		{
			result = RemoveItemFromStash(itemData, size);
			//UpdateStashSlots();
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
		OnInventoryListChanged?.Invoke();
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
		OnStashListChanged?.Invoke();
		return result;
	}

	public void LoadData(GameData data)
	{
		currency = data.currency;
		OnCurrencyChanged?.Invoke(currency);
		var allItemData = GetAllItemData();

		if (allItemData != null)
		{
			foreach (var pair in data.inventory)
			{
				AddItem(allItemData[pair.Key], pair.Value);
			}
			foreach (var pair in data.stash)
			{
				AddItem(allItemData[pair.Key], pair.Value);
			}
			foreach (var id in data.equipments)
			{
				EquipItem(allItemData[id]);
			}
		}
	}

	public void SaveData(ref GameData data)
	{
		data.currency = currency;
		foreach (var item in inventoryItems)
		{
			if (!data.inventory.TryAdd(item.itemData.itemId, item.stackSize))
			{
				data.inventory[item.itemData.itemId] = item.stackSize;
			}
		}
		foreach (var item in stashItems)
		{
			if (!data.stash.TryAdd(item.itemData.itemId, item.stackSize))
			{
				data.stash[item.itemData.itemId] = item.stackSize;
			}
		}
		foreach (var item in equipmentItems)
		{
			data.equipments.Add(item.itemData.itemId);
		}
	}

	private Dictionary<string, ItemData> GetAllItemData()
	{
		var result = new Dictionary<string, ItemData>();
		string[] allItemId = AssetDatabase.FindAssets("", new[] { "Assets/Scripts/Inventory&Item/ItemData/ItemDataBase" });

		if (allItemId.Length == 0) return null;

		foreach (var itemId in allItemId)
		{
			if (itemId == null) continue;
			var path = AssetDatabase.GUIDToAssetPath(itemId);
			var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(path);
			result.Add(itemId, itemData);
		}
		return result;
	}
}
