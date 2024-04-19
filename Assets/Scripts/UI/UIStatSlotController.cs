using TMPro;
using UnityEngine;

public class UIStatSlotController : MonoBehaviour
{
	[SerializeField] private string statName;
	[SerializeField] public StatType statType;
	[SerializeField] private TextMeshProUGUI statNameText;
	[SerializeField] private TextMeshProUGUI valueText;

	private Stat stat;
	private CharacterStats characterStats;

	private void OnValidate()
	{
		this.statName = statType.ToString();
		this.name = "Stat Slot - " + this.statType.ToString();
		UpdateContent();
	}

	private void Start()
	{
		UpdateContent();
	}

	public void SetupSlotByStat(Stat stat)
	{
		this.stat = stat;
		this.UpdateContent();
	}

	public void SetupSlotByCharacter(CharacterStats characterStats)
	{
		this.characterStats = characterStats;
		this.SetupSlotByStat(characterStats.GetStatByType(statType));
	}

	private void UpdateContent()
	{
		this.statNameText.text = this.statName;
		if (this.stat != null && this.characterStats != null)
		{
			this.valueText.text = FormateContent();
		}
	}

	private string FormateContent()
	{
		switch (this.statType)
		{
			case StatType.CriticalRate: return (this.stat.GetValue()) + ("(+" + this.characterStats.agility.GetValue() + ")") + "%";
			case StatType.EvasionRate: return (this.stat.GetValue()) + ("(+" + this.characterStats.agility.GetValue() + ")") + "%";
			case StatType.Damage: return (this.stat.GetValue()) + ("(+" + this.characterStats.strength.GetValue() + ")");
			case StatType.CriticalMultiplier: return (this.stat.GetValue()) + ("(+" + this.characterStats.strength.GetValue() + ")");
			case StatType.FireDamage: return (this.stat.GetValue()) + ("(+" + this.characterStats.intelligence.GetValue() + ")");
			case StatType.LightningDamge: return (this.stat.GetValue()) + ("(+" + this.characterStats.intelligence.GetValue() + ")");
			case StatType.FrostDamage: return (this.stat.GetValue()) + ("(+" + this.characterStats.intelligence.GetValue() + ")");
			case StatType.MagicResistance: return (this.stat.GetValue()) + ("(+" + this.characterStats.intelligence.GetValue() * 3 + ")");
			case StatType.MaxHealth: return (this.stat.GetValue()) + ("(+" + this.characterStats.vitality.GetValue() * 5 + ")");
			default: return this.stat.GetValue().ToString();
		}
	}
}
