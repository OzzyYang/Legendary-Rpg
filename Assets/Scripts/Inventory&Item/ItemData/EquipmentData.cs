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

	public EquipmentData(ItemType itemType, string itemName, Sprite icon) : base(itemType, itemName, icon)
	{
		this.itemType = ItemType.Equipment;
	}


	public void AddModifiersToPlayer() => this.AddModifiers(PlayerManager.instance.player.GetComponent<CharacterStats>());

	public void RemoveModifiersFromPlayer() => this.RemoveModifiers(PlayerManager.instance.player.GetComponent<CharacterStats>());


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
}


