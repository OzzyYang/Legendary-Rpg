using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItemSlotController : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
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
		switch (inventoryItem.itemData.itemType)
		{
			case ItemType.Material:
				{
					if (inventoryItem.itemData.haveEffect && inventoryItem.itemData.ExecutePositiveEffect(PlayerManager.instance.player.transform))
					{
						InventoryManager.instance.RemoveItem(inventoryItem.itemData);
					}
					break;
				}
			case ItemType.Equipment:
				{
					InventoryManager.instance.EquipItem(this.inventoryItem.itemData);
					break;
				}
		}
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		UIManager.instance.GetMenuPageController().HideItemToolTip();
	}

	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		UIManager.instance.GetMenuPageController().ShowItemToolTip(inventoryItem?.itemData);
	}
}
