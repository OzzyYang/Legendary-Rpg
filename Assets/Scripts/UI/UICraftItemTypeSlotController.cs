using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICraftItemTypeSlotController : MonoBehaviour, IPointerDownHandler
{
	[SerializeField] private EquipmentType equipmentType;
	[SerializeField] private Sprite icon;

	private void Start()
	{
		this.UpdateInfo();
	}
	public void OnPointerDown(PointerEventData eventData)
	{
		UIManager.instance.GetMenuPageController().ShowCraftSlotsListByType(this.equipmentType);
	}

	private void UpdateInfo()
	{
		this.GetComponent<Image>().sprite = icon;
		this.name = "CraftItemTypeSlot - " + equipmentType.ToString();
	}
	private void OnValidate()
	{
		this.UpdateInfo();
	}
}
