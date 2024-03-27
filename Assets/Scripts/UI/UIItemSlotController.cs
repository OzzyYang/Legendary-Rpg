using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlotController : MonoBehaviour
{
	[SerializeField] private Image itemIconSlot;
	[SerializeField] private TextMeshProUGUI itemAmountSlot;

	private InventoryItem inventoryItem;

	private void Awake()
	{
		//UpdateData(this.inventoryItem);

	}

	public void UpdateData(InventoryItem inventoryItem)
	{
		this.inventoryItem = inventoryItem;
		if (this.inventoryItem != null && this.inventoryItem.stackSize >= 1)
		{
			itemIconSlot.color = Color.white;
			this.itemIconSlot.sprite = inventoryItem.itemData.Icon;
			this.itemAmountSlot.text = this.inventoryItem.stackSize.ToString();
		}
		else
		{
			this.itemAmountSlot.text = "";
			itemIconSlot.sprite = null;
			itemIconSlot.color = new Color(0, 0, 0, 0);
		}
	}

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{

	}
}
