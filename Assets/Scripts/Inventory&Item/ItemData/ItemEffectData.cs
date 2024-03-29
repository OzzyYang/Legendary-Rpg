using UnityEngine;

public class ItemEffectData : ScriptableObject
{
	[SerializeField] protected string effectName;
	[SerializeField] protected ItemType effectType;



	public virtual void Effect(Transform target)
	{
		UnityEngine.Debug.Log(effectName + " has been executed!");
	}
}
