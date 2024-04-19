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
	[Header("Item Info")]
	public ItemType itemType;
	public string itemName;
	public Sprite Icon;
	[Header("Craft Info")]
	public bool canBeCrafted;
	public List<InventoryItem> ingredients;
	[Header("Drop Info")]
	[Range(0, 100)]
	public int dropChance;
	[Header("Effect Info")]
	public bool haveEffect;
	public List<ItemEffectData> effectDatas;
	[TextArea]
	public string effectDescription;

	public ItemData(ItemType itemType, string itemName, Sprite icon)
	{
		this.itemType = itemType;
		this.itemName = itemName;
		Icon = icon;
	}

	private void OnValidate()
	{
		canBeCrafted = ingredients?.Count != 0;
		haveEffect = effectDatas != null && effectDatas.Count > 0;
	}

	public bool ExecuteNegativeEffect(Transform target)
	{
		if (!haveEffect || target == null) return false;
		foreach (var effectData in effectDatas)
		{
			if (!effectData.canExecuteNegativeEffect(target)) return false;
		}
		foreach (var effectData in effectDatas)
		{
			effectData.NegativeEffect(target);
		}
		return true;
	}

	public bool ExecutePositiveEffect(Transform target)
	{
		if (!haveEffect || target == null) return false;
		foreach (var effectData in effectDatas)
		{
			if (!effectData.canExecutePositiveEffect(target)) return false;
		}
		foreach (var effectData in effectDatas)
		{
			effectData.PositiveEffect(target);
		}
		return true;
	}
}
