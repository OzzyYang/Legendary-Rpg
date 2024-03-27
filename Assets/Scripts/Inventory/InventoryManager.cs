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

	[SerializeField] private Transform inventorySlotsParent;
	private UIItemSlotController[] inventorySlots;
	[SerializeField] private Transform stashSlotsParent;
	private UIItemSlotController[] stashSlots;
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
	}

	private void UpdateInventorySlots()
	{
		inventorySlots = inventorySlotsParent.GetComponentsInChildren<UIItemSlotController>();
		for (int i = 0; i < inventoryItems.Count; i++)
		{
			inventorySlots[i].UpdateData(inventoryItems[i]);
		}
	}

	private void UpdateStashSlots()
	{
		stashSlots = stashSlotsParent.GetComponentsInChildren<UIItemSlotController>();
		for (int i = 0; i < stashItems.Count; i++)
		{
			stashSlots[i].UpdateData(stashItems[i]);
		}
	}


	public InventoryItem AddItem(ItemData itemData)
	{
		if (itemData.itemType == ItemType.Material)
		{
			AddToInventory(itemData);
			UpdateInventorySlots();
			return inventoryItemsDict[itemData];
		}
		else if (itemData.itemType == ItemType.Equipment)
		{
			AddToStash(itemData);
			UpdateStashSlots();
			return stashItemsDict[itemData];
		}
		return null;
	}

	public InventoryItem AddItem(ItemData itemData, int size)
	{
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

	private void AddToInventory(ItemData itemData)
	{
		if (inventoryItemsDict.TryGetValue(itemData, out InventoryItem item))
		{
			item.AddStack();
		}
		else
		{
			InventoryItem newItem = new InventoryItem(itemData);
			inventoryItems.Add(newItem);
			inventoryItemsDict.Add(itemData, newItem);
		}
	}
	private void AddToStash(ItemData itemData)
	{
		if (stashItemsDict.TryGetValue(itemData, out InventoryItem item))
		{
			item.AddStack();
		}
		else
		{
			InventoryItem newItem = new InventoryItem(itemData);
			stashItems.Add(newItem);
			stashItemsDict.Add(itemData, newItem);
		}
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

	public InventoryItem RemoveItemFromInventory(ItemData itemData)
	{
		InventoryItem result = null;
		if (inventoryItemsDict.TryGetValue(itemData, out InventoryItem item))
		{
			if (item.RemoveStack() == 0)
			{
				inventoryItems.Remove(item);
				inventoryItemsDict.Remove(itemData);
			}
			else result = item;
		}

		UpdateInventorySlots();
		return result;
	}

	public InventoryItem RemoveItemFromStash(ItemData itemData)
	{
		InventoryItem result = null;
		if (stashItemsDict.TryGetValue(itemData, out InventoryItem item))
		{
			if (item.RemoveStack() == 0)
			{
				stashItems.Remove(item);
				stashItemsDict.Remove(itemData);
			}
			else result = item;
		}

		UpdateInventorySlots();
		return result;
	}


	public InventoryItem RemoveItemFromInventory(ItemData itemData, int size)
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
		UpdateStashSlots();
		return result;
	}

	public InventoryItem RemoveItemFromStash(ItemData itemData, int size)
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
		UpdateStashSlots();
		return result;
	}


}
