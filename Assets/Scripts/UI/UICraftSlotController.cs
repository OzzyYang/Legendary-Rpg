
using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftSlotController : UIItemSlotController
{
	[SerializeField] private ItemData itemData;
	private void Start()
	{
		this.UpdateData(null);
	}
	public override void OnPointerDown(PointerEventData eventData)
	{
		//if (InventoryManager.instance.CanCraftItem(this.itemData))
		//{
		//	Debug.Log("Can Craft Item!");
		//}
		//else
		//{
		//	Debug.Log("Not enough ingredients!");
		//}
		UIManager.instance.GetMenuPageController().ShowCraftItemInfo(itemData);
	}

	public override void UpdateData(InventoryItem inventoryItem)
	{
		if (inventoryItem==null) return;

		//this.inventoryItem = new InventoryItem(this.itemData);
		this.inventoryItem = inventoryItem;
		this.itemData = inventoryItem.itemData;
		base.UpdateData(this.inventoryItem);
		this.name = "Craft Slot - " + this.itemType;
	}

	protected override void OnValidate()
	{
		this.UpdateData(this.inventoryItem);
	}
}
