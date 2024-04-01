using System.Collections;
using UnityEngine;
public enum BuffType
{
	Strength,
	Agility,
	Intelligence,
	Vitality,
	Damage,
	CriticalRate,
	CriticalMultiplier,
	MaxHealth,
	EvasionRate,
	Armor,
	MagicResistance,
	FireDamage,
	FrostDamage,
	LightningDamge
}

[CreateAssetMenu(fileName = "New Buff Effect", menuName = "Data/Item Effect/Buff Effect")]
public class BuffEffect : ItemEffectData
{
	[SerializeField] private BuffType buffType;
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
		Stat statToModify = GetStatByType(target.GetComponent<CharacterStats>());
		if (target == null || value > 0) return;
		target.GetComponent<CharacterStats>().StartCoroutine(addModifierFor(duration, statToModify));
	}

	public override void PositiveEffect(Transform target)
	{
		Stat statToModify = GetStatByType(target.GetComponent<CharacterStats>());
		if (target == null || value < 0) return;
		target.GetComponent<CharacterStats>().StartCoroutine(addModifierFor(duration, statToModify));
	}

	private Stat GetStatByType(CharacterStats targetStats)
	{
		switch (this.buffType)
		{
			case BuffType.Strength: return targetStats.strength;
			case BuffType.Agility: return targetStats.agility;
			case BuffType.Intelligence: return targetStats.intelligence;
			case BuffType.Vitality: return targetStats.vitality;
			case BuffType.Damage: return targetStats.damage;
			case BuffType.CriticalRate: return targetStats.criticalRate;
			case BuffType.CriticalMultiplier: return targetStats.criticalMultiplier;
			case BuffType.MaxHealth: return targetStats.maxHealth;
			case BuffType.EvasionRate: return targetStats.evasionRate;
			case BuffType.Armor: return targetStats.armor;
			case BuffType.MagicResistance: return targetStats.magicResistance;
			case BuffType.FireDamage: return targetStats.fireDamage;
			case BuffType.FrostDamage: return targetStats.frostDamage;
			case BuffType.LightningDamge: return targetStats.lightningDamge;
			default: return null;
		}
	}

	private IEnumerator addModifierFor(float seconds, Stat statToModify)
	{
		statToModify.AddModifier(value);
		yield return new WaitForSeconds(seconds);
		statToModify.RemoveModifier(value);
	}
}
