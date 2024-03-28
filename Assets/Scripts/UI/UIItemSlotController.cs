using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItemSlotController : MonoBehaviour, IPointerDownHandler
{
	[SerializeField] protected Image itemIconSlot;
	[SerializeField] protected TextMeshProUGUI itemAmountSlot;
	[SerializeField] protected ItemType itemType;

	protected InventoryItem inventoryItem;

	protected virtual void OnValidate()
	{
		this.UpdateData(inventoryItem);
		this.name = "Item Slot - " + itemType;
	}

	public virtual void UpdateData(InventoryItem inventoryItem)
	{
		this.inventoryItem = inventoryItem;
		if (this.inventoryItem != null && this.inventoryItem.stackSize >= 1)
		{
			this.itemIconSlot.color = Color.white;
			this.itemIconSlot.sprite = this.inventoryItem.itemData.Icon;
			this.itemAmountSlot.text = this.inventoryItem.stackSize == 1 ? "" : this.inventoryItem.stackSize.ToString();
			this.itemType = this.inventoryItem.itemData.itemType;
		}
		else
		{
			this.itemAmountSlot.text = "";
			itemIconSlot.sprite = null;
			itemIconSlot.color = new Color(0, 0, 0, 0);
		}
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if (this.inventoryItem == null) return;
		InventoryManager.instance.EquipItem(this.inventoryItem.itemData);
	}
}
