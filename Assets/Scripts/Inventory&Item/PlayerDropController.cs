using System.Collections.Generic;
using UnityEngine;

public class PlayerDropController : ItemDropController
{
	private List<InventoryItem> equiptedItems;
	private void Start()
	{
		this.equiptedItems = InventoryManager.Instance.GetEquipmentItemsList();
	}
	public override void DropItem()
	{
		if (!this.canDropItem) return;
		this.equiptedItems = InventoryManager.Instance.GetEquipmentItemsList();
		foreach (var item in equiptedItems)
		{
			if (Random.Range(0, 100) < item.itemData.dropChance)
			{
				GameObject dropedItem = Instantiate(itemPrefab, this.transform.position, Quaternion.identity);
				dropedItem.GetComponent<ItemObjectController>().Setup(item.itemData, new Vector2(movement.x * Random.Range(-1.0f, 1.0f), movement.y * Random.Range(1.0f, 3.0f)));
				InventoryManager.Instance.RemoveEquipment(item.itemData);
				InventoryManager.Instance.RemoveItem(item.itemData);
			}
		}
	}

	protected override void OnValidate()
	{

	}
}
