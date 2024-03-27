using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class InventoryItem
{
	public int stackSize { get; private set; }
	[SerializeField] public ItemData itemData;
	//[SerializeField] private int stackSize;
	[SerializeField] private Image itemIcon;
	[SerializeField] private TextMeshProUGUI itemAmountText;



	public InventoryItem(ItemData itemData, int stackSize)
	{
		this.itemData = itemData ?? throw new ArgumentNullException(nameof(itemData));
		if (stackSize < 0) throw new ArgumentOutOfRangeException(nameof(stackSize));
		this.stackSize = stackSize;
	}

	public InventoryItem(ItemData itemData)
	{
		this.itemData = itemData ?? throw new ArgumentNullException(nameof(itemData));
		this.stackSize = 1;
	}

	public int AddStack() => stackSize++;

	public int AddStack(int size) => stackSize += size;

	public int RemoveStack() => --stackSize <= 0 ? 0 : stackSize;

	public int RemoveStack(int size) => (stackSize -= size) <= 0 ? 0 : stackSize;

}
