using UnityEngine;

public class ItemEffectData : ScriptableObject
{
	[SerializeField] protected string effectName;
	[SerializeField] protected ItemType effectType;
	[TextArea]
	[SerializeField] protected string effectDescription;

	// Stats down effect, including take damage.
	public virtual void NegativeEffect(Transform target)
	{
		Debug.Log(effectName + " has been executed!");
	}
	// Stats up effect.
	public virtual void PositiveEffect(Transform target)
	{
		Debug.Log(effectName + " has been executed!");
	}

	public virtual bool canExecutePositiveEffect(Transform target)
	{
		return true;
	}

	public virtual bool canExecuteNegativeEffect(Transform target)
	{
		return true;
	}

	public virtual string GetEffectDescription() => effectDescription;
}
