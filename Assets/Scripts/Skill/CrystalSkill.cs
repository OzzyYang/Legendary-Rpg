using UnityEngine;

public class CrystalSkill : Skill
{
	[SerializeField] private GameObject crystalPrefab;
	[SerializeField] private float crystalDuration;

	[SerializeField] private bool canExplode;
	[SerializeField] private bool canMoveToEnemy;

	private GameObject crystal;
	public override bool CanUseSkill()
	{
		if (coolDownTimer <= 0)
		{
			UseSkill();
			return true;
		}
		return false;
	}

	public override void UseSkill()
	{
		base.UseSkill();

		if (crystal == null)
		{
			crystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
		}
		else
		{
			if (crystal.GetComponent<CrystalController>().isExploding || canMoveToEnemy) return;
			Vector2 Temp = player.transform.position;
			player.transform.position = crystal.transform.position;
			crystal.transform.position = Temp;
			crystal.GetComponent<CrystalController>().Explode();
			return;
		}
		crystal.GetComponent<CrystalController>().SetupCrystal(crystalDuration, canExplode, canMoveToEnemy);

	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
		if (Input.GetKeyDown(KeyCode.F))
		{
			UseSkill();
		}
	}
}
