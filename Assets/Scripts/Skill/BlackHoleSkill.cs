using UnityEngine;

public class BlackHoleSkill : Skill
{
	[SerializeField] private GameObject blackHoleObject;

	private GameObject blackHole;
	public override bool CanUseSkill()
	{
		blackHole = blackHole == null ? Instantiate(blackHoleObject) : blackHole;
		blackHole.SetActive(false);

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
		blackHole.SetActive(true);
		blackHole.transform.position = player.transform.position;
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
		if (Input.GetKeyDown(KeyCode.Z))
		{
			CanUseSkill();
		}
	}
}
