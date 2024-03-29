using UnityEngine;

public class ItemObjectController : MonoBehaviour
{

	[SerializeField] private ItemData itemData;
	private Rigidbody2D rb;

	private bool isGroundDetected;
	// Start is called before the first frame update

	private void OnValidate()
	{
		Reset();
	}

	private void Start()
	{

	}
	private void Reset()
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

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (isGroundDetected)

		{
			rb.velocity = Vector2.zero;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{

		if (collision.GetComponent<PlayerController>() != null)
		{
			if (collision.GetComponent<PlayerController>().isDead) return;
			collision.GetComponent<PlayerController>().PickupItem(this.itemData);
			Destroy(this.gameObject);
		}
		if (collision.CompareTag("Ground"))
		{
			isGroundDetected = true;
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{

	}



	public void Setup(ItemData itemData, Vector2 movement)
	{
		this.itemData = itemData;
		rb.velocity = movement;
		Reset();
	}
}
