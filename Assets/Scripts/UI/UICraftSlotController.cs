
using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftSlotController : UIItemSlotController
{
	[SerializeField] private ItemData itemData;
	public override void OnPointerDown(PointerEventData eventData)
	{
		if (InventoryManager.instance.CanCraftItem(this.itemData))
		{
			Debug.Log("Can Craft Item!");
		}
		else
		{
			Debug.Log("Not enough ingredients!");
		}
	}

	public override void UpdateData(InventoryItem inventoryItem)
	{
		base.UpdateData(inventoryItem);
	}

	protected override void OnValidate()
	{
		if (this.itemData == null) return;
		this.inventoryItem = new InventoryItem(this.itemData);
		this.UpdateData(inventoryItem);
		this.name = "Craft Slot - " + this.itemType;
	}
}
