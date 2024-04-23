using UnityEngine;
using UnityEngine.EventSystems;

public class UIEquipmentSlotController : UIItemSlotController
{

	[SerializeField] private EquipmentType equipmentType;

	protected override void OnValidate()
	{
		this.name = "Equipment Slot - " + equipmentType.ToString();
		this.UpdateData(inventoryItem);
		this.itemType = ItemType.Equipment;

	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		//base.OnPointerDown(eventData);
		//if(this.inventoryItem)
		InventoryManager.Instance.RemoveEquipment(this.inventoryItem?.itemData);
	}

	public override void UpdateData(InventoryItem inventoryItem)
	{
		base.UpdateData(inventoryItem);
	}

}
