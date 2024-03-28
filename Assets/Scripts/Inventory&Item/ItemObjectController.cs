using UnityEngine;

public class ItemObjectController : MonoBehaviour
{

	[SerializeField] private ItemData itemData;
	// Start is called before the first frame update

	private void OnValidate()
	{
		if (itemData == null) return;
		if (itemData.GetType() == typeof(EquipmentData))
		{
			this.name = "Equipment Object - " + itemData.itemName;
		}
		else
		{
			this.name = "Item Object - " + itemData.itemName;
		}
		GetComponent<SpriteRenderer>().sprite = itemData.Icon;

	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		InventoryManager.instance.AddItem(this.itemData);
		Destroy(this.gameObject);
	}
}
