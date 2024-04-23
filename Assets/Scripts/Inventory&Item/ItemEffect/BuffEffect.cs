using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "New Buff Effect", menuName = "Data/Item Effect/Buff Effect")]
public class BuffEffect : ItemEffectData
{
	[SerializeField] private StatType buffType;
	[SerializeField] private float value;
	[SerializeField] private float duration = 4;

	private Stat statToModify;

	private void OnValidate()
	{
		this.effectName = "Buff Effect - " + this.buffType.ToString();
	}

	public override bool canExecuteNegativeEffect(Transform target)
	{
		return base.canExecuteNegativeEffect(target);
	}

	public override bool canExecutePositiveEffect(Transform target)
	{
		return base.canExecutePositiveEffect(target);
	}

	public override void NegativeEffect(Transform target)
	{
		Stat statToModify = target.GetComponent<CharacterStats>().GetStatByType(this.buffType);
		if (target == null || value > 0) return;
		target.GetComponent<CharacterStats>().StartCoroutine(addModifierFor(duration, statToModify));
	}

	public override void PositiveEffect(Transform target)
	{
		Stat statToModify = target.GetComponent<CharacterStats>().GetStatByType(this.buffType);
		if (target == null || value < 0) return;
		target.GetComponent<CharacterStats>().StartCoroutine(addModifierFor(duration, statToModify));
	}

	private IEnumerator addModifierFor(float seconds, Stat statToModify)
	{
		statToModify.AddModifier(value);
		yield return new WaitForSeconds(seconds);
		statToModify.RemoveModifier(value);
	}
}
