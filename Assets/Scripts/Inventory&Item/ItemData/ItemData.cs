using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
	Material,
	Equipment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
	public ItemType itemType;
	public string itemName;
	public Sprite Icon;
	public bool canBeCrafted;
	public List<InventoryItem> ingredients;

	public ItemData(ItemType itemType, string itemName, Sprite icon)
	{
		this.itemType = itemType;
		this.itemName = itemName;
		Icon = icon;
	}

	private void OnValidate()
	{
		if (ingredients.Count != 0)
		{
			canBeCrafted = true;
		}
		else
		{
			canBeCrafted = false;
		}
	}
}
