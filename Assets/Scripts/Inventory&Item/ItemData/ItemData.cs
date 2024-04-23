using System.Collections.Generic;
using System.Text;
using UnityEditor;
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
	public string itemId;
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
	[SerializeField] protected string itemDescription;

	private void OnValidate()
	{
		canBeCrafted = ingredients?.Count != 0;
		haveEffect = effectDatas != null && effectDatas.Count > 0;
#if UNITY_EDITOR
		itemId = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(this));
#endif
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

	public virtual string GetItemDescription()
	{
		var result = new StringBuilder();
		result.Append(itemDescription + "\n");
		if (haveEffect)
		{
			if (effectDatas.Count <= 1)
			{
				result.Append($"Usage:{effectDatas[0].GetEffectDescription()}");

			}
			else
			{
				int count = 0;
				foreach (var effect in effectDatas)
				{
					result.Append($"Usage{++count}: {effect.GetEffectDescription()}" + "\n");
				}
			}
		}
		return result.ToString();
	}




}
