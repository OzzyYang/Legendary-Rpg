using System.Text;
using UnityEngine;
public enum EquipmentType
{
	Weapon,
	Armor,
	Amulet,
	Flask
}

[CreateAssetMenu(fileName = "New Equipment Data", menuName = "Data/Equipment")]
public class EquipmentData : ItemData
{
	[Header("Equipment Info")]
	public EquipmentType equipmentType;

	[Header("Major Stats")]
	public Stat strength;// 1 point increase damage by 1 and critical multiplier by 1%
	public Stat agility;// 1 point increase evasion by 1% and critical rate by 1%
	public Stat intelligence;// 1 point increase magic damage by 1 and magic resistance by 3
	public Stat vitality;// 1 point increase health by 3 or 5 points

	[Header("Offensive Stats")]
	public Stat damage;
	public Stat criticalRate;
	public Stat criticalMultiplier;

	[Header("Defensive Stats")]
	public Stat maxHealth;
	public Stat evasionRate;
	public Stat armor;
	public Stat magicResistance;

	[Header("Magic Stats")]
	public Stat fireDamage;
	public Stat frostDamage;
	public Stat lightningDamge;

	public void AddModifiersToPlayer() => this.AddModifiers(PlayerManager.Instance.Player.GetComponent<CharacterStats>());

	public void RemoveModifiersFromPlayer() => this.RemoveModifiers(PlayerManager.Instance.Player.GetComponent<CharacterStats>());


	public void AddModifiers(CharacterStats target)
	{
		target.strength.AddModifier(this.strength.GetValue());
		target.agility.AddModifier(this.agility.GetValue());
		target.intelligence.AddModifier(this.intelligence.GetValue());
		target.vitality.AddModifier(this.vitality.GetValue());

		target.damage.AddModifier(this.damage.GetValue());
		target.criticalRate.AddModifier(this.criticalRate.GetValue());
		target.criticalMultiplier.AddModifier(this.criticalMultiplier.GetValue());

		target.maxHealth.AddModifier(this.maxHealth.GetValue());
		target.evasionRate.AddModifier(this.evasionRate.GetValue());
		target.armor.AddModifier(this.armor.GetValue());
		target.magicResistance.AddModifier(this.magicResistance.GetValue());

		target.fireDamage.AddModifier(this.fireDamage.GetValue());
		target.frostDamage.AddModifier(this.frostDamage.GetValue());
		target.lightningDamge.AddModifier(this.lightningDamge.GetValue());
	}

	public void RemoveModifiers(CharacterStats target)
	{
		target.strength.RemoveModifier(this.strength.GetValue());
		target.agility.RemoveModifier(this.agility.GetValue());
		target.intelligence.RemoveModifier(this.intelligence.GetValue());
		target.vitality.RemoveModifier(this.vitality.GetValue());

		target.damage.RemoveModifier(this.damage.GetValue());
		target.criticalRate.RemoveModifier(this.criticalRate.GetValue());
		target.criticalMultiplier.RemoveModifier(this.criticalMultiplier.GetValue());

		target.maxHealth.RemoveModifier(this.maxHealth.GetValue());
		target.evasionRate.RemoveModifier(this.evasionRate.GetValue());
		target.armor.RemoveModifier(this.armor.GetValue());
		target.magicResistance.RemoveModifier(this.magicResistance.GetValue());

		target.fireDamage.RemoveModifier(this.fireDamage.GetValue());
		target.frostDamage.RemoveModifier(this.frostDamage.GetValue());
		target.lightningDamge.RemoveModifier(this.lightningDamge.GetValue());
	}

	public override string GetItemDescription()
	{
		var result = new StringBuilder();

		if (itemDescription != null || haveEffect)
			result.Append("¡ñ Properties" + "\n");
		result.Append(FormatContent() + "\n");
		if (itemDescription.Length > 0)
		{
			result.AppendLine();
			result.Append("¡ñ Description" + "\n");
			result.Append(itemDescription + "\n");
		}
		if (haveEffect)
		{
			result.AppendLine();
			if (effectDatas.Count <= 1)
			{
				result.Append("¡ñ Unique Effect" + "\n");
				result.Append(effectDatas[0].GetEffectDescription());
			}
			else
			{
				result.Append("¡ñ Unique Effects" + "\n");
				int count = 0;
				foreach (var effect in effectDatas)
				{
					result.Append($"{++count}.{effect.GetEffectDescription()}" + "\n");
				}
			}
		}

		return result.ToString();
	}


	private string FormatContent()
	{
		StringBuilder sb = new StringBuilder();

		sb.Append(FormateContentFromStat(StatType.Strength, strength));
		sb.Append(FormateContentFromStat(StatType.Agility, agility));
		sb.Append(FormateContentFromStat(StatType.Intelligence, intelligence));
		sb.Append(FormateContentFromStat(StatType.Vitality, vitality));
		sb.Append(FormateContentFromStat(StatType.Damage, damage));
		sb.Append(FormateContentFromStat(StatType.CriticalRate, criticalRate));
		sb.Append(FormateContentFromStat(StatType.CriticalMultiplier, criticalMultiplier));
		sb.Append(FormateContentFromStat(StatType.FireDamage, fireDamage));
		sb.Append(FormateContentFromStat(StatType.FrostDamage, frostDamage));
		sb.Append(FormateContentFromStat(StatType.LightningDamge, lightningDamge));
		sb.Append(FormateContentFromStat(StatType.Armor, armor));
		sb.Append(FormateContentFromStat(StatType.EvasionRate, evasionRate));
		sb.Append(FormateContentFromStat(StatType.MagicResistance, magicResistance));
		sb.Append(FormateContentFromStat(StatType.MaxHealth, maxHealth));
		//delete the last '\n'
		sb.Remove(sb.Length - 1, 1);
		return sb.ToString();
	}

	private string FormateContentFromStat(StatType statType, Stat stat)
	{
		if (stat == null || stat.GetValue() == 0) return "";
		return $"{statType}: {(stat.GetValue() >= 0 ? "+" : "")}{stat.GetValue()}\n";
	}
}


