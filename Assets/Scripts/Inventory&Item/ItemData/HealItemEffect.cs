using UnityEngine;


public enum IncreaseType
{
	ByPercentage,
	ByPoints
}
[CreateAssetMenu(fileName = "Heal Item Effect", menuName = "Data/Item Effect/Heal Item Effect")]
public class HealItemEffect : ItemEffectData
{
	[SerializeField] public IncreaseType increaseType;
	[Range(0f, 1f)]
	[SerializeField] public float increasePercentage;
	[SerializeField] public float increasePoints;

	public override bool canExecutePositiveEffect(Transform target)
	{
		if (target == null) return false;
		if (target.GetComponent<CharacterStats>().currentHealth == target.GetComponent<CharacterStats>().maxHealth.GetValue()) return false;
		return true;
	}

	public override void PositiveEffect(Transform target)
	{
		CharacterStats targetStats = target.GetComponent<CharacterStats>();

		if (targetStats.currentHealth == targetStats.maxHealth.GetValue()) return;

		switch (increaseType)
		{
			case IncreaseType.ByPercentage:
				{
					targetStats.IncreseHealth(targetStats.maxHealth.GetValue() * increasePercentage, this.name);
					break;
				}
			case IncreaseType.ByPoints:
				{
					targetStats.IncreseHealth(increasePoints, this.name);
					break;
				}
		}
	}
}
