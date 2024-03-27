using UnityEngine;

public class ItemObjectController : MonoBehaviour
{

	[SerializeField] private ItemData itemData;
	// Start is called before the first frame update

	private void OnValidate()
	{
		GetComponent<SpriteRenderer>().sprite = itemData.Icon;
		this.name = "Item Object - " + itemData.itemName;
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log(itemData.itemName + " has been picked up by " + collision.name);
		InventoryManager.instance.AddItem(this.itemData);
		Destroy(this.gameObject);
	}
}
