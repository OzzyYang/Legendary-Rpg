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
	public ItemEffectData effectData;

	public ItemData(ItemType itemType, string itemName, Sprite icon)
	{
		this.itemType = itemType;
		this.itemName = itemName;
		Icon = icon;
	}

	private void OnValidate()
	{
		canBeCrafted = ingredients?.Count != 0;
		//haveEffect = effectData?.GetType() == typeof(ItemEffectData);
		//Debug.Log(effectData==null);
		haveEffect= effectData != null;
	}

	public void ExecuteEffect(Transform target)
	{
		if (!haveEffect || target == null) return;
		this.effectData.Effect(target);
	}

}
