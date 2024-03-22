public class EnemyStat : CharacterStats
{
	public override void DoDamage(CharacterStats _target)
	{
		base.DoDamage(_target);
	}

	public override float TakeDamage(float _damage)
	{
		return base.TakeDamage(_damage);
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}
}
