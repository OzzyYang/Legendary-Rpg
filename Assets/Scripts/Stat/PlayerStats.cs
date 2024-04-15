public class PlayerStats : CharacterStats
{

	public override void DoDamage(CharacterStats _target)
	{
		base.DoDamage(_target);
	}


	protected override void Die()
	{
		base.Die();
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

	public void IncreaseBaseEvasionRate(float increaseAmount)
	{
		this.evasionRate.SetDefaultValue(increaseAmount + this.evasionRate.GetValue());
		this.OnBaseEvasionRateChanged?.Invoke();
	}
}
