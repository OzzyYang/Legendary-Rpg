using UnityEngine;

public class EnemyStat : CharacterStats
{
	[SerializeField] private int enemyLevel = 1;

	[Range(0, 1)]
	[SerializeField] private float modifierPercentage = 0.4f;
	[SerializeField] private float basePoint = 4;

	public override void DoDamage(CharacterStats _target)
	{
		base.DoDamage(_target);
	}


	protected override void Start()
	{
		base.Start();
		ApplyModifier();
	}

	protected override void Update()
	{
		base.Update();
	}

	public void ApplyModifier()
	{
		this.AddModifier(this.strength);
	}

	public void AddModifier(Stat stat)
	{
		for (int i = 0; i < enemyLevel; i++)
		{
			stat.AddModifier(Mathf.RoundToInt(basePoint * modifierPercentage));

		}
	}
}
