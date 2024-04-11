
using TMPro;
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
		UIManager.instance.GetMenuPageController().ShowCraftItemInfo(itemData);
	}

	public override void UpdateData(InventoryItem inventoryItem)
	{
		if (inventoryItem == null) return;
		this.inventoryItem = inventoryItem;
		this.itemData = inventoryItem.itemData;
		this.itemIconSlot.sprite = itemData.Icon;
		this.itemAmountSlot.GetComponent<TextMeshProUGUI>().text = itemData.itemName;
		this.name = "Craft Slot - " + this.itemType;
	}

	protected override void OnValidate()
	{
		this.itemIconSlot.sprite = itemData.Icon;
		this.itemAmountSlot.GetComponent<TextMeshProUGUI>().text = itemData.itemName;
	}
}
