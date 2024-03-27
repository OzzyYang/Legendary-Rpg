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
	public EquipmentType equipmentType;

}
