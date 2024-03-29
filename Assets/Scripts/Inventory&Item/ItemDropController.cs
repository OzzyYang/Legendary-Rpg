using System.Collections.Generic;
using UnityEngine;

public class ItemDropController : MonoBehaviour
{
	[SerializeField] protected GameObject itemPrefab;
	[SerializeField] protected bool canDropItem;
	[SerializeField] protected List<ItemData> items;
	[SerializeField] protected Vector2 movement;

	protected virtual void OnValidate()
	{
		if (items?.Count > 0)
		{
			canDropItem = true;
		}
		else
		{
			canDropItem = false;
		}
	}

	public virtual void DropItem()
	{
		if (!canDropItem) return;
		foreach (var item in items)
		{
			if (Random.Range(0, 100) < item.dropChance)
			{
				GameObject newDropedItem = Instantiate(itemPrefab, this.transform.position, Quaternion.identity);

				newDropedItem.GetComponent<ItemObjectController>().Setup(item, new Vector2(movement.x * Random.Range(-1.0f, 1.0f), movement.y * Random.Range(1.0f, 3.0f)));
			}
		}

	}
}
